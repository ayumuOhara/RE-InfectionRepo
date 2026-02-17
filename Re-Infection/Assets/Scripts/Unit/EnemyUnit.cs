using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : UnitBase, Iinfection
{
    GameObject castleObj;
    [SerializeField] Sprite corpseSprite;   // 死体スプライト
    [SerializeField] GameObject infecitonInfo;
    [SerializeField] Image infectionBar;
    [SerializeField] GameObject defeatedEffect;
    WaveSpawner waveSpawner;

    public bool IsInfectioning { get; set; } = false;

    public override void Initialize(UnitStats stats, bool isClone = false)
    {
        base.Initialize(stats, isClone);

        spriteRenderer.material = Stats.GetOutline("EnemyUnitOutline");
    }

    private void Awake()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>();
        SetStateManager(new UnitStateManager(this, new EnemyUnitDecider(this)));
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

    public override void Start()
    {
        base.Start();
        if (Stats.bossUnit)
            FindObjectOfType<WaveSpawner>().SetBoss(this);
    }

    public override void Move()
    {
        transform.position = Movement.Movement(MyPos, TargetPos, Stats.MoveSpeed);
    }

    public override void Dead()
    {
        if (Stats.bossUnit)
        {
            FindObjectOfType<UnitManager>().RemoveUnitList(this, IsInfectioning);
            Instantiate(defeatedEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            if (!IsInfectioning)
            {
                spriteRenderer.material = defaultMaterial;

                var unitManager = FindObjectOfType<UnitManager>();
                if (!isClone)
                {
                    unitManager.RemoveUnitList(this, IsInfectioning);
                    FindObjectOfType<WaveSpawner>().DecreaseEnemySum();
                }

                gameObject.layer = LayerMask.NameToLayer("CorpseUnit");
                unitManager.AddCorpseList(this);
                animator.enabled = false;

                GetComponent<SpriteRenderer>().sprite = corpseSprite;
            }
            else
            {
                FindObjectOfType<UnitManager>().RemoveUnitList(this, IsInfectioning);
                Destroy(gameObject);
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
        infecitonInfo.SetActive(true);

        while (timer < infectionTime)
        {
            yield return new WaitUntil(() => waveSpawner.IsStartWave);

            timer += Time.deltaTime;

            infectionBar.fillAmount = timer / infectionTime;
        }

        infecitonInfo.SetActive(false);

        stateManager.SetUnitAI(new PlayerUnitDecider(this));

        gameObject.layer = LayerMask.NameToLayer("PlayerUnit");
        targetLayer = LayerMask.GetMask("EnemyUnit");

        Heal(Stats.maxHp * healthRate);

        FindObjectOfType<UnitManager>().RemoveCorpseList(this);
        FindObjectOfType<UnitManager>().AddUnitList(this, IsInfectioning);

        GetComponent<SpriteRenderer>().sprite = Stats.unitSprite;

        spriteRenderer.material = Stats.GetOutline("PlayerUnitOutline");
        
        animator.enabled = true;
    }
}
