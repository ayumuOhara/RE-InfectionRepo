using Unity.VisualScripting;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour, IHealth, IMovable, IAttackable
{
    [SerializeField] UnitStats stats;
    public UnitStats Stats => stats;

    float currentHealth;
    public float CurrentHealth => currentHealth;
    public float HealthRate => currentHealth / stats.maxHp;
    public bool IsDead => currentHealth <= 0;

    public Vector3 MyPos => transform.position;
    public Vector3 TargetPos { get; set; }

    MovementBase movementBase;
    public MovementBase Movement => movementBase;

    AttackBase attackBase;
    public AttackBase Attacking => attackBase;

    public UnitStateManager stateManager { get; set; }

    public void Initialize(UnitStats stats)
    {
        //this.stats = new UnitStats()
        //{
        //    unitSprite = stats.unitSprite,
        //    unitName = stats.unitName,
        //    jobType = stats.jobType,
        //    targetType = stats.targetType,
        //    maxHp = stats.maxHp,
        //    atk = stats.atk,
        //    atkInterbal = stats.atkInterbal,
        //    moveSpeed = stats.moveSpeed,
        //    range = stats.range,
        //    infecitonTime = stats.infecitonTime,
        //    bossUnit = stats.bossUnit,
        //    attackSe = stats.attackSe,
        //};

        movementBase = stats.MovementBase;
        attackBase = stats.AttackBase;
        GetComponent<SpriteRenderer>().sprite = this.stats.unitSprite;
        currentHealth = stats.maxHp;
    }

    public virtual void Start()
    {
        FindObjectOfType<UnitManager>()?.AddUnitList(this);
        stateManager.StateMachine.Initialize(stateManager.StateMachine.moveState);
    }

    public virtual void Update()
    {
        stateManager.StateTransition();
        stateManager.StateMachine.Update();
    }

    public virtual void Move()
    {
        transform.position += movementBase.Movement(MyPos, TargetPos, stats.MoveSpeed);
    }

    public virtual void Attack()
    {
        GetComponent<AudioSource>().PlayOneShot(stats.attackSe);
        GetComponent<Animator>().SetTrigger("Attack");

        //attackBase.Attacking();
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

    }
}
