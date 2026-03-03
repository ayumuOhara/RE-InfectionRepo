using CannonPointer;
using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VirusPointer;

public class EnemyUnit : UnitBase, Iinfection
{
    GameObject castleObj;
    [SerializeField] Sprite corpseSprite;   // 死体スプライト
    [SerializeField] Canvas infecitonInfo;
    [SerializeField] Image infectionBar;
    [SerializeField] GameObject defeatedEffect;

    WaveSpawner waveSpawner;

    public bool IsInfectioning { get; set; } = false;

    public override void Initialize(UnitStats stats)
    {
        base.Initialize(stats);

        FindObjectOfType<UnitManager>().AddEnemyUnitList(this);
    }

    public override void SetStats(UnitStats stats)
    {
        targetLayer = LayerMask.GetMask("PlayerUnit");
        gameObject.layer = 7;

        base.SetStats(stats);

        IsInfectioning = false;

        SetStateManager(new UnitStateManager(this, new EnemyUnitDecider(this)));

        if (Stats.bossUnit)
            FindObjectOfType<WaveSpawner>().SetBoss(this);
    }

    private void Awake()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>();
        castleObj = GameObject.Find("CastleWall");
    }

    public override void Targetting()
    {
        switch (Stats.targetType)
        {
            case Types.TargetType.UNIT_NEAREST:
                var targetN = GetTarget.GetNearestTargetUnit(this);
                TargetObj = targetN != null || IsInfectioning ? targetN : castleObj;
                break;
            case Types.TargetType.UNIT_FARTHEST:
                var targetF = GetTarget.GetFarthestTargetUnit(this);
                TargetObj = targetF != null || IsInfectioning ? targetF : castleObj;
                break;
            case Types.TargetType.BUILDING:
                TargetObj = castleObj;
                break;
        }
    }

    public override void Move()
    {
        transform.position = Movement.Movement(MyPos, TargetPos, Stats.MoveSpeed);
    }

    public override void Dead()
    {
        // 死亡時の処理
        Instantiate(deadEffect, transform.position, Quaternion.identity);

        if (Stats.bossUnit)
        {
            FindObjectOfType<UnitManager>().RemoveEnemyUnitList(this);

            Instantiate(defeatedEffect, transform.position, Quaternion.identity);
            Release();
        }
        else
        {
            if (!IsInfectioning)
            {
                spriteRenderer.material = defaultMaterial;

                var unitManager = FindObjectOfType<UnitManager>();
                if (!isClone)
                {
                    unitManager.RemoveEnemyUnitList(this);

                    FindObjectOfType<WaveSpawner>().DecreaseEnemySum();
                }

                gameObject.layer = LayerMask.NameToLayer("CorpseUnit");
                unitManager.AddCorpseList(this);
                animator.enabled = false;

                GetComponent<SpriteRenderer>().sprite = corpseSprite;

                return;
            }
            else
            {
                FindObjectOfType<UnitManager>().RemovePlayerUnitList(this);
                
                Release();
            }
        }

    }

    public void StartInfection(float healthRate)
    {
        StartCoroutine(Infection(10, healthRate));
    }

    // 感染
    public IEnumerator Infection(float infectionTime, float healthRate)
    {
        IsInfectioning = true;

        var timer = 0f;
        infecitonInfo.enabled = true;

        while (timer < infectionTime)
        {
            yield return new WaitUntil(() => waveSpawner.IsStartWave);

            timer += Time.deltaTime;

            infectionBar.fillAmount = timer / infectionTime;
        }

        infecitonInfo.enabled = false;

        stateManager.SetUnitAI(new PlayerUnitDecider(this));

        gameObject.layer = LayerMask.NameToLayer("PlayerUnit");
        targetLayer = LayerMask.GetMask("EnemyUnit");

        Heal(Stats.maxHp * healthRate);

        FindObjectOfType<UnitManager>().RemoveCorpseList(this);

        FindObjectOfType<UnitManager>().AddPlayerUnitList(this);

        spriteRenderer.sprite = Stats.unitSprite;
        SetOutline();

        animator.enabled = true;
    }
}
