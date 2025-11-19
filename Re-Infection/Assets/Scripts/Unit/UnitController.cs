using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;

// ユニットの所属しているグループ
public enum UnitGroup
{
    Player,
    Enemy,
}

public class UnitController : MonoBehaviour
{
    public UnitManager unitManager { get; private set; }
    public WaveSpawner waveSpawner { get; private set; }
    UnitStateManager unitStateManager;

    // 一番近いターゲットオブジェクト
    public GameObject targetObj => GetTarget.GetTargetObj(group == UnitGroup.Player ? UnitGroup.Enemy : UnitGroup.Player, transform.position);
    // 拠点オブジェクト
    public GameObject castleObj { get; private set; }
    // 自機座標(参照用)
    public Vector3 myPos => transform.position;
    // ターゲットオブジェクト座標
    public Vector3 targetPos => targetObj.transform.position;
    // 拠点オブジェクト座標
    public Vector3 castlePos => castleObj.transform.position;

    public AudioSource unitAudio;

    [SerializeField] public GameObject unitUI;            // ユニット専用UIオブジェクト
    [SerializeField] public Image infectionRateGauge;     // 感染度ゲージ
    [SerializeField] GameObject damageTextPrefab;       　// ダメージ数表示テキスト
    [SerializeField] public Sprite corpseSprite;          // 死体スプライト
    
    [SerializeField] AudioClip deadSe;                    // ユニット死亡時の音(後で消す)
    [SerializeField] GameObject deadEffect;               // ユニット死亡時エフェクト(後で消す)
    [SerializeField] GameObject bossDefeatEffect;         // ボス撃破時エフェクト(後で消す)

    const float UNIT_SCALE = 0.3f;
    Vector3 myScale = new Vector3(UNIT_SCALE, UNIT_SCALE, UNIT_SCALE); // ユニットのサイズ

    // ユニットのスタッツ
    public UnitGroup group { get; private set; }   // 味方か敵か
    public string unitName { get; private set; }   // ユニット名
    public Sprite unitSprite { get; private set; }     // ユニットのスプライト
    public float currentHp { get; private set; }   // 現在HP
    public float maxHp { get; private set; }       // 最大HP
    public float atk { get; private set; }         // 攻撃力
    public float atkInterbal { get; private set; }     // 攻撃間隔
    public float moveSpeed { get; private set; }   // 移動速度
    public float range { get; private set; }       // 攻撃距離
    public float infecitonTime { get; private set; }  // 感染するまでの時間
    public bool isInfection { get; private set; }   // 一度感染したか
    public bool bossUnit { get; private set; }      // ボスか
    public AudioClip attackSe { get; private set; }
    public bool isDead => currentHp <= 0;

    // HPの割合
    public float HealthRate => currentHp / maxHp;

    // 初期化
    public void SetUnitStats(UnitStats stats, UnitGroup group)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = stats.unitSprite;
        unitSprite = stats.unitSprite;

        this.group = group;

        unitName = stats.unitName;
        currentHp = stats.maxHp;
        maxHp = stats.maxHp;
        atk = stats.atk;
        atkInterbal = stats.atkInterbal;
        moveSpeed = stats.moveSpeed * 0.1f;
        range = stats.range;
        infecitonTime = stats.infecitonTime;
        bossUnit = stats.bossUnit;
        attackSe = stats.attackSe;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localScale = myScale;

        castleObj = GameObject.Find("CastleWall").gameObject;
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        unitManager.AddUnitList(this, group);
        waveSpawner = GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>();

        unitStateManager = new UnitStateManager(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        unitStateManager.StateTransition();
        unitStateManager.StateMachine.Update();
    }

    // 感染
    public void Infection()
    {
        if(isInfection) return;

        if(unitStateManager.StateMachine.CurrentState == unitStateManager.StateMachine.deadState)
        {
            StartCoroutine(unitStateManager.StateMachine.deadState.Infectioning());
            group = UnitGroup.Player;
            isInfection = true;
        }
    }

    // ダメージ処理
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        if(currentHp < 0)
        {
            currentHp = 0;
        }

        GameObject textObj = InstanceObjHeadUp(damageTextPrefab);
        DrawDamage(textObj, damage);
    }

    public void DestroyUnit()
    {
        // 火曜の試遊に間に合わせる為のもの
        // 試遊が終わったら作り直す
        if (unitName == "ソードマスター")
        {
            GetComponent<SpriteRenderer>().enabled = false;
            Instantiate(bossDefeatEffect, transform.position, Quaternion.identity);
        }

        unitManager.RemoveUnitList(this, group);
        Destroy(gameObject);
    }

    // 死亡処理
    public void Dead()
    {
        Instantiate(deadEffect, transform.position, Quaternion.identity);
        unitAudio.PlayOneShot(deadSe);

        unitManager.RemoveUnitList(this, group);

        if (group == UnitGroup.Enemy)
            waveSpawner.DecreaseEnemySum();
    }

    // ダメージ表示
    void DrawDamage(GameObject textObj, float damage)
    {
        TextMeshProUGUI damageText = textObj.GetComponent<TextMeshProUGUI>();
        damageText.text = damage.ToString();
    }

    // 回復
    public void HealHelth(float value)
    {
        currentHp += value;
        if(currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
    }
    
    // 頭上にUIプレファブを生成
    public GameObject InstanceObjHeadUp(GameObject prefabUI)
    {
        var unitPos = Camera.main.WorldToScreenPoint(transform.position);   // ユニットのワールド座標をスクリーン座標に変換
        unitPos.y += 0.3f;
        GameObject prefab = Instantiate(prefabUI, GameObject.Find("InGameUI").transform, false); // ユニットの少し上にPrefabを生成
        prefab.transform.position = unitPos;

        return prefab;
    }
}
