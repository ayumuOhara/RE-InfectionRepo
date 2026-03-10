using System.Collections;
using UnityEngine;

using VirusPointer;

public abstract class UnitBase : PooledObject, IHealth, IMovable, IAttackable
{
    [SerializeField] GameObject damageEffect;
    [SerializeField] protected GameObject deadEffect;
    [SerializeField] private int precision = 100; // 精度（100倍すれば0.01単位まで反映）

    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Material defaultMaterial;
    public Animator animator {  get; private set; }

    protected UnitStats stats;
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

    [SerializeField]
    private float currentHealth;
    public float CurrentHealth => currentHealth;
    public float HealthRate => currentHealth / stats.maxHp;
    public bool IsDead => currentHealth <= 0;
    // 複製体フラグ
    protected bool isClone;

    public Vector3 MyPos => transform.position;

    public GameObject TargetObj { get; set; }

    public Vector3 TargetPos => GetTargetPos();

    protected MovementBase movementBase;
    public MovementBase Movement => movementBase;

    protected AttackBase attackBase;

    public UnitStateManager stateManager { get; private set; }
    
    private SEManager seManager;

    public virtual void Initialize(UnitStats stats)
    {
        isClone = false;

        SetComponent();
        SetStats(stats);
        SetOutline();

        currentHealth = this.stats.maxHp;
    }

    public virtual void Initialize(UnitStats stats, bool isClone)
    {
        SetComponent();
        SetStats(stats);

        this.isClone = isClone;
        spriteRenderer.material = defaultMaterial;

        currentHealth = 0;
    }

    private void SetComponent()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void SetStats(UnitStats stats)
    {
        this.stats = new UnitStats()
        {
            animatorController = stats.animatorController,
            unitSprite = stats.unitSprite,
            attackEffect = stats.attackEffect,
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
            bossUnit = stats.bossUnit,
            attackSe = stats.attackSe,
        };

        movementBase = stats.MovementBase;
        attackBase = stats.AttackBase;

        if (stats.animatorController != null)
            animator.runtimeAnimatorController = (RuntimeAnimatorController)stats.animatorController;

        spriteRenderer.sprite = this.stats.unitSprite;
    }

    protected void SetOutline()
    {
        spriteRenderer.material = Stats.GetOutline(LayerMask.LayerToName(gameObject.layer) + "Outline");
    }

    public void SetStateManager(UnitStateManager unitStateManager)
    {
        stateManager = unitStateManager;
    }

    public virtual void Start()
    {
        seManager = FindObjectOfType<SEManager>();
        stateManager.StateMachine.Initialize(stateManager.StateMachine.moveState);
        StartCoroutine(UsingVirusSkillTransparency());
    }

    private void Update()
    {
        if (!IsDead)
        {
            Targetting();
        }

        stateManager.StateTransition();
        stateManager.StateMachine.Update();
    }

    private void LateUpdate()
    {
        // Y座標を -100倍して整数に変換
        // 例: Yが 1.23 の場合 -> sortingOrder は -123 になる
        // Yが低い（画面下）ほど、数値が大きくなる（手前に来る）
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -precision + transform.position.x * precision);
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
        if(animator.enabled) animator.SetTrigger("Attack");

        attackBase?.Attack(this);
    }

    public virtual void Damage(float damage)
    {
        seManager.PlaySE(SEManager.SEType.Damage);
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

        Release();
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
        var drag = GameObject.Find("VirusSkillPointer")?.GetComponent<VirusSkillPointer>();
        if (drag != null)
        {
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
        else
        {
            yield break;
        }
    }

    // エフェクト生成
    public void InstanceEffect(Vector3 targetPos)
    {
        if (stats.attackEffect != null)
        {
            Instantiate(stats.attackEffect, targetPos, Quaternion.identity);
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
