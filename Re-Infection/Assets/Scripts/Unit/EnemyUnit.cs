using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : UnitBase, Iinfection
{
    GameObject castleObj;
    [SerializeField] Sprite corpseSprite;   // 死体スプライト
    [SerializeField] GameObject infecitonInfo;
    [SerializeField] Image infectionBar;

    public bool IsInfectioning { get; set; } = false;

    private void Awake()
    {
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
        
        if(Stats.bossUnit)
            FindObjectOfType<WaveSpawner>().SetBoss(this);
    }

    public override void Move()
    {
        transform.position = Movement.Movement(MyPos, TargetPos, Stats.MoveSpeed);
    }

    public override void Dead()
    {
        if (!Stats.bossUnit && !IsInfectioning)
        {
            var unitManager = FindObjectOfType<UnitManager>();
            unitManager.RemoveUnitList(this, IsInfectioning);
            unitManager.AddCorpseList(this);

            FindObjectOfType<WaveSpawner>().DecreaseEnemySum();
            GetComponent<SpriteRenderer>().sprite = corpseSprite;

            StartCoroutine(Infection());
        }
        else
        {
            FindObjectOfType<UnitManager>().RemoveUnitList(this, IsInfectioning);
            Destroy(gameObject);
        }
    }

    // 感染
    public IEnumerator Infection()
    {
        yield return new WaitUntil(() => IsInfectioning);

        var timer = 0f;
        infecitonInfo.SetActive(true);

        while (timer < Stats.infecitonTime)
        {
            timer += Time.deltaTime;

            infectionBar.fillAmount = timer / Stats.infecitonTime;

            yield return null;
        }

        infecitonInfo.SetActive(false);

        stateManager.SetUnitAI(new PlayerUnitDecider(this));

        gameObject.layer = LayerMask.NameToLayer("PlayerUnit");
        targetLayer = LayerMask.GetMask("EnemyUnit");

        Heal(Stats.maxHp * 0.5f);

        FindObjectOfType<UnitManager>().RemoveCorpseList(this);
        FindObjectOfType<UnitManager>().AddUnitList(this, IsInfectioning);

        GetComponent<SpriteRenderer>().sprite = Stats.unitSprite;
    }
}
