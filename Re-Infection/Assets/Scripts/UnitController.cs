using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ユニットの所属しているグループ
public enum UnitGroup
{
    Player,
    Enemy,
}

public class UnitController : MonoBehaviour
{
    GameObject targetObj;   // 敵オブジェクト

    public Vector3 kingPos { get; private set; }    // 王様の座標
    public Vector3 targetPos { get; private set; }  // 敵の座標
    public Vector3 myPos { get; private set; }      // 自身の座標

    Vector3 moveDirection;  // 移動方向

    float atkInterbal = 0;  // 攻撃後の経過時間

    // ユニットのステータス
    public UnitGroup group { get; private set; }   // 味方か敵か
    public float currentHp { get; private set; }   // 現在HP
    public float maxHp { get; private set; }       // 最大HP
    public float atk { get; private set; }         // 攻撃力
    public float atkRate { get; private set; }     // 攻撃間隔
    public float moveSpeed { get; private set; }   // 移動速度
    public float range { get; private set; }       // 攻撃距離
    bool isDead => currentHp <= 0;                 // 死亡フラグ

    bool isMoving = true;   // 移動フラグ

    // 初期化
    public void SetUnitStats(UnitStats original, UnitGroup group)
    {
        this.group = group;

        // データを複製
        var stats = new UnitStats()
        {
            maxHp = original.maxHp,
            atk = original.atk,
            atkRate = original.atkRate,
            moveSpeed = original.moveSpeed,
            range = original.range,
        };

        // 複製したデータを代入
        currentHp = stats.maxHp;
        maxHp = stats.maxHp;
        atk = stats.atk;
        atkRate = stats.atkRate;
        moveSpeed = stats.moveSpeed;
        range = stats.range;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        kingPos = GameObject.Find("King").transform.position;
        myPos = transform.position;
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        Dead();

        if (isMoving)
        {
            if (group == UnitGroup.Player)
                MovePlayerUnit();
            if (group == UnitGroup.Enemy)
                MoveEnemyUnit();
        }

        targetObj = GetTarget.GetTargetObj(group == UnitGroup.Player ? UnitGroup.Enemy : UnitGroup.Player, myPos);
        if(targetObj != null)
            targetPos = targetObj.transform.position;

        if (Vector3.Distance(targetPos, transform.position) <= range && targetObj != null)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    void MovePlayerUnit()
    {
        myPos += Vector3.up * moveSpeed * Time.deltaTime;
        transform.position = myPos;
    }

    void MoveEnemyUnit()
    {
        moveDirection = (kingPos - myPos).normalized;
        myPos += moveDirection * moveSpeed * Time.deltaTime;
        transform.position = myPos;
    }

    // ダメージ処理
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        Debug.Log($"{damage}ダメージを受けた！");
    }

    // 攻撃
    void Attack()
    {
        atkInterbal += Time.deltaTime;
        if (atkInterbal > atkRate)
        {
            atkInterbal = 0;

            UnitController target = targetObj.GetComponent<UnitController>();
            target.TakeDamage(atk);
        }
    }

    // 死亡処理
    void Dead()
    {
        if (!isDead) return;

        UnitManager um = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        um.RemoveUnitList(gameObject, group);
        Destroy(gameObject);
    }
}
