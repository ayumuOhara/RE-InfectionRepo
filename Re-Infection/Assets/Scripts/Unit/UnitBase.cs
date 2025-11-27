using System.Collections;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour, IHealth, IMovable, IAttackable
{
    UnitManager unitManager;

    UnitStats stats;
    public UnitStats Stats => stats;

    [SerializeField] LayerMask targetLayer;
    public LayerMask TargetLayer => targetLayer;
    public string TargetLayerStr
    {
        get
        {
            var layerName = LayerMask.LayerToName(gameObject.layer);
            switch (layerName)
            {
                case "PlayerUnit":
                    return "EnemyUnit";
                case "EnemyUnit":
                    return "PlayerUnit";
                default:
                    return null;
            }
        }
    }

    float currentHealth;
    public float CurrentHealth => currentHealth;
    public float HealthRate => currentHealth / stats.maxHp;
    public bool IsDead => currentHealth <= 0;

    public Vector3 MyPos => transform.position;

    public GameObject TargetObj { get; set; }

    public Vector3 TargetPos => GetTargetPos();

    MovementBase movementBase;
    public MovementBase Movement => movementBase;

    AttackBase attackBase;

    public UnitStateManager stateManager { get; private set; }

    public void Initialize(UnitStats stats)
    {
        this.stats = new UnitStats()
        {
            unitSprite = stats.unitSprite,
            unitName = stats.unitName,
            jobType = stats.jobType,
            targetType = stats.targetType,
            maxHp = stats.maxHp,
            atk = stats.atk,
            atkInterbal = stats.atkInterbal,
            moveSpeed = stats.moveSpeed,
            range = stats.range,
            infecitonTime = stats.infecitonTime,
            bossUnit = stats.bossUnit,
            attackSe = stats.attackSe,
        };

        movementBase = stats.MovementBase;
        attackBase = stats.AttackBase;

        GetComponent<SpriteRenderer>().sprite = this.stats.unitSprite;
        currentHealth = stats.maxHp;
    }

    public void SetStateManager(UnitStateManager unitStateManager)
    {
        stateManager = unitStateManager;
    }

    public void Start()
    {
        unitManager = FindObjectOfType<UnitManager>();
        unitManager?.AddUnitList(this);
        stateManager.StateMachine.Initialize(stateManager.StateMachine.moveState);
    }

    public void Update()
    {
        if (!IsDead)
        {
            Targetting();
        }

        stateManager.StateTransition();
        stateManager.StateMachine.Update();
    }

    public virtual void Targetting()
    {
        // ターゲッティング処理
    }

    public virtual void Move()
    {
        // 移動処理
    }

    public virtual void Attack()
    {
        GetComponent<AudioSource>().PlayOneShot(stats.attackSe);
        GetComponent<Animator>().SetTrigger("Attack");

        attackBase?.Attack(this);
    }

    public virtual void Damage(float damage)
    {
        currentHealth -= damage;
    }

    public virtual void Heal(float heal)
    {
        currentHealth += heal;
        if (currentHealth >= stats.maxHp)
        {
            currentHealth = stats.maxHp;
        }
    }

    public virtual void Dead()
    {
        // 死亡時の処理
    }

    Vector3 GetTargetPos()
    {
        if(TargetObj == null)
            return Vector3.zero;
        else
            return TargetObj.transform.position;
    }
}
