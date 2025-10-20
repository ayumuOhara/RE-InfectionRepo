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

    public GameObject targetObj { get; private set; }   // 敵オブジェクト

    [SerializeField] GameObject damageTextPrefab;　// ダメージ数表示テキスト
    [SerializeField] GameObject deadIconPrefab;    // 死亡時アイコン
    [SerializeField] Sprite corpseSprite;          // 死体スプライト

    const float UNIT_SCALE = 0.4f;
    Vector3 myScale = new Vector3(UNIT_SCALE, UNIT_SCALE, UNIT_SCALE); // ユニットのサイズ

    // ユニットのスタッツ
    public UnitGroup group { get; private set; }   // 味方か敵か
    public Sprite unitSprite { get; private set; }     // ユニットのスプライト
    public float currentHp { get; private set; }   // 現在HP
    public float maxHp { get; private set; }       // 最大HP
    public float atk { get; private set; }         // 攻撃力
    public float atkRate { get; private set; }     // 攻撃間隔
    public float moveSpeed { get; private set; }   // 移動速度
    public float range { get; private set; }       // 攻撃距離

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
        atkRate = stats.atkRate;
        moveSpeed = stats.moveSpeed * 0.1f;
        range = stats.range;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localScale = myScale;

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

        var unitPos = Camera.main.WorldToScreenPoint(transform.position);   // ユニットのワールド座標をスクリーン座標に変換
        unitPos.y += 0.3f;
        GameObject textObj = Instantiate(damageTextPrefab, GameObject.Find("UI").transform, false); // ユニットの少し上にダメージテキストを生成
        textObj.transform.position = unitPos;

        // ダメージを表示する
        TextMeshProUGUI damageText = textObj.GetComponent<TextMeshProUGUI>();
        damageText.text = damage.ToString();
    }
}
