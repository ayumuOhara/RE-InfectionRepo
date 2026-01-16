using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Types
{
    // 役職
    public enum JobType
    {
        SOLDIER,    // 剣士
        HAMMER,     // ハンマー
        TANK,       // 盾
        ARCHER,     // 弓兵
        MAGE,       // 魔法使い
        CAVALRY,    // 騎兵
    }

    // 攻撃方法
    public enum AttackType
    {
        SINGLE,         // 単体
        AREA_MELEE,     // 近接範囲
        AREA_RANGE,     // 遠距離範囲
    }

    // 移動方法
    public enum MoveType
    {
        RUN,    // 通常移動
    }

    // 目標
    public enum TargetType
    {
        UNIT_NEAREST,   // 最寄りのユニット
        UNIT_FARTHEST,  // 最遠のユニット
        BUILDING,       // 建物のみ
    }
}

[System.Serializable]
public class UnitStats
{
    [Header("スプライト")]
    public Sprite unitSprite;           // ユニットのスプライト
    [Header("名前")]
    public string unitName;             // ユニット名
    [Header("役職")]
    public Types.JobType jobType;       // 役職
    
    [Header("攻撃/移動 目標")]
    public Types.TargetType targetType; // 攻撃または移動対象

    [Header("最大HP")]
    public float maxHp;                 // 最大HP
    [Header("攻撃タイプ")]
    public Types.AttackType attackType; // 攻撃方法
    
    [Header("-----範囲攻撃用ステータス-----")]
    [Header("ヒットする数")]
    public int hitCnt;                  // ヒットする数
    [Header("攻撃範囲(半径、遠距離のみ)")]
    public float radius;                // 攻撃範囲

    [Header("------汎用攻撃ステータス------")]
    [Header("攻撃力")]
    public float atk;                   // 攻撃力
    [Header("攻撃間隔")]
    public float atkInterbal;           // 攻撃速度
    [Header("射程距離")]
    public float range;                 // 射程距離

    [Header("移動方法")]
    public Types.MoveType moveType;     // 移動方法
    [Header("移動速度")]
    public float moveSpeed;             // 移動速度
    public float MoveSpeed
    {
        get
        {
            return moveSpeed * 0.1f;
        }
    }

    [Header("召喚コスト")]
    public int summonCost;              // 召喚コスト
    [Header("感染が完了する時間")]
    public float infecitonTime;         // 感染するまでの時間
    [Header("ボスユニットか")]
    public bool bossUnit;               // ボスか
    [Header("攻撃時のSE")]
    public AudioClip attackSe;          // 攻撃音

    public Sprite JobSprite
    {
        get
        {
            switch (jobType)
            {
                case Types.JobType.SOLDIER:
                    return Resources.Load<Sprite>("Sprites/SoldierIcon");
                case Types.JobType.HAMMER:
                    return Resources.Load<Sprite>("Sprites/HammerIcon");
                case Types.JobType.TANK:
                    return Resources.Load<Sprite>("Sprites/TankIcon");
                case Types.JobType.ARCHER:
                    return Resources.Load<Sprite>("Sprites/ArcherIcon");
                case Types.JobType.MAGE:
                    return Resources.Load<Sprite>("Sprites/MageIcon");
                case Types.JobType.CAVALRY:
                    return Resources.Load<Sprite>("Sprites/CavalryIcon");
                default:
                    return Resources.Load<Sprite>("Sprites/DefaultIcon");
            }
        }
    }

    public AttackBase AttackBase
    {
        get
        {
            switch(attackType)
            {
                case Types.AttackType.SINGLE:
                    return new AttackOfSingle();
                case Types.AttackType.AREA_MELEE:
                    return new AttackOfAreaMelee();
                case Types.AttackType.AREA_RANGE:
                    return new AttackOfAreaRange();
                default:
                    return null;
            }
        }
    }

    public MovementBase MovementBase
    {
        get
        {
            switch(moveType)
            {
                case Types.MoveType.RUN:
                    return new RunMovement();
                default:
                    return null;
            }
        }
    }
}

[CreateAssetMenu(fileName = "UnitStats", menuName = "Scriptable Objects/UnitStats")]
public class UnitStatsData : ScriptableObject
{
    public UnitStats unitStats;
}
