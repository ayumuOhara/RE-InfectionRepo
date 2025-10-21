using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;

// ユニットの所属しているグループ
public enum UnitGroup
{
    Player,
    Enemy,
}

public class UnitController : MonoBehaviour
{
    public UnitManager unitManager { get; private set; }
    UnitStateManager unitStateManager;

    // 一番近いターゲットオブジェクト
    public GameObject targetObj => GetTarget.GetTargetObj(group == UnitGroup.Player ? UnitGroup.Enemy : UnitGroup.Player, transform.position);
    // ターゲットとの距離
    public float targetDistance => Vector3.Distance(targetObj.transform.position, transform.position);
    // 拠点オブジェクト
    public GameObject castleObj { get; private set; }
    // 拠点との距離
    public float castleDistance => Vector3.Distance(castleObj.transform.position, transform.position);

    [SerializeField] GameObject damageTextPrefab;       　// ダメージ数表示テキスト
    [SerializeField] public GameObject deadIconPrefab;    // 死亡時アイコン
    [SerializeField] public Sprite corpseSprite;          // 死体スプライト

    const float UNIT_SCALE = 0.3f;
    Vector3 myScale = new Vector3(UNIT_SCALE, UNIT_SCALE, UNIT_SCALE); // ユニットのサイズ

    // ユニットのスタッツ
    public UnitGroup group { get; private set; }   // 味方か敵か
    public Sprite unitSprite { get; private set; }     // ユニットのスプライト
    public float currentHp { get; private set; }   // 現在HP
    public float maxHp { get; private set; }       // 最大HP
    public float atk { get; private set; }         // 攻撃力
    public float atkInterbal { get; private set; }     // 攻撃間隔
    public float moveSpeed { get; private set; }   // 移動速度
    public float range { get; private set; }       // 攻撃距離

    public bool isDead => currentHp <= 0;

    // 初期化
    public void SetUnitStats(UnitStats stats, UnitGroup group)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = stats.unitSprite;
        unitSprite = stats.unitSprite;

        this.group = group;

        currentHp = stats.maxHp;
        maxHp = stats.maxHp;
        atk = stats.atk;
        atkInterbal = stats.atkInterbal;
        moveSpeed = stats.moveSpeed * 0.1f;
        range = stats.range;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localScale = myScale;

        castleObj = GameObject.Find("CastleWall").gameObject;
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        unitManager.AddUnitList(this, group);

        unitStateManager = new UnitStateManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        unitStateManager.StateTransition();
        unitStateManager.StateMachine.Update();
    }

    // ダメージ処理
    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        GameObject textObj = InstanceObjHeadUp(damageTextPrefab);
        
        // ダメージを表示する
        TextMeshProUGUI damageText = textObj.GetComponent<TextMeshProUGUI>();
        damageText.text = damage.ToString();
    }
    
    // 頭上にUIプレファブを生成
    public GameObject InstanceObjHeadUp(GameObject prefabUI)
    {
        var unitPos = Camera.main.WorldToScreenPoint(transform.position);   // ユニットのワールド座標をスクリーン座標に変換
        unitPos.y += 0.3f;
        GameObject prefab = Instantiate(prefabUI, GameObject.Find("UI").transform, false); // ユニットの少し上にPrefabを生成
        prefab.transform.position = unitPos;

        return prefab;
    }
}
