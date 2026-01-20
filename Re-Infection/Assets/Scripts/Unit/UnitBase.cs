using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Rendering;
using UnityEngine;
using VirusPointer;

public abstract class UnitBase : MonoBehaviour, IHealth, IMovable, IAttackable
{
    [SerializeField] GameObject damageEffect;
    [SerializeField] GameObject deadEffect;

    public Animator animator {  get; private set; }

    UnitStats stats;
    public UnitStats Stats => stats;

    public LayerMask targetLayer;
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
    // 複製体フラグ
    protected bool isClone;

    public Vector3 MyPos => transform.position;

    public GameObject TargetObj { get; set; }

    public Vector3 TargetPos => GetTargetPos();

    MovementBase movementBase;
    public MovementBase Movement => movementBase;

    AttackBase attackBase;

    public UnitStateManager stateManager { get; private set; }

    public void Initialize(UnitStats stats, bool isClone = false)
    {
        this.isClone = isClone;
        animator = GetComponent<Animator>();
        if(stats.animatorController != null)
        animator.runtimeAnimatorController = (RuntimeAnimatorController)stats.animatorController;

        this.stats = new UnitStats()
        {
            unitSprite = stats.unitSprite,
            unitName = stats.unitName,
            jobType = stats.jobType,
            targetType = stats.targetType,
            maxHp = stats.maxHp,
            attackType = stats.attackType,
            hitCnt = stats.hitCnt,
            atk = stats.atk,
            atkInterbal = stats.atkInterbal,
            moveSpeed = stats.moveSpeed,
            range = stats.range,
            radius = stats.radius,
            infecitonTime = stats.infecitonTime,
            bossUnit = stats.bossUnit,
            attackSe = stats.attackSe,
        };

        movementBase = stats.MovementBase;
        attackBase = stats.AttackBase;

        GetComponent<SpriteRenderer>().sprite = this.stats.unitSprite;

        if (!isClone)
            currentHealth = stats.maxHp;
        else
            currentHealth = 0;
    }

    public void SetStateManager(UnitStateManager unitStateManager)
    {
        stateManager = unitStateManager;
    }

    public virtual void Start()
    {
        if(!isClone) FindObjectOfType<UnitManager>().AddUnitList(this);
        stateManager.StateMachine.Initialize(stateManager.StateMachine.moveState);
        StartCoroutine(UsingVirusSkillTransparency());
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
        if(animator.enabled) animator.SetTrigger("Attack");

        attackBase?.Attack(this);
    }

    public virtual void Damage(float damage)
    {
        Instantiate(damageEffect, transform.position, Quaternion.identity);

        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
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
        Instantiate(deadEffect, transform.position, Quaternion.identity);
        FindObjectOfType<UnitManager>().RemoveUnitList(this);
    }

    Vector3 GetTargetPos()
    {
        if(TargetObj == null)
            return Vector3.zero;
        else
            return TargetObj.transform.position;
    }

    // ウイルス使用中、スプライトを透過
    IEnumerator UsingVirusSkillTransparency()
    {
        var drag = GameObject.Find("VirusSkillPointer").GetComponent<VirusSkillPointer>();
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color color = sprite.color;

        while (true)
        {
            if (drag.IsDragging && !IsDead)
            {
                color.a = 0.4f;
                sprite.color = color;
            }
            else
            {
                color.a = 1.0f;
                sprite.color = color;
            }

            yield return null;
        }
    }

    // 円を描画するための補助メソッド
    public static void DrawDebugCircle(Vector2 center, float radius, Color color, float duration)
    {
        int segments = 20; // 円を構成する線の数
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector2(radius, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);

            // Sceneビューに線を描画
            Debug.DrawLine(prevPoint, nextPoint, color, duration);
            prevPoint = nextPoint;
        }
    }
}
