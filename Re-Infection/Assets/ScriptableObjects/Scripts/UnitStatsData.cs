using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Types
{
    // 役職
    public enum JobType
    {
        SOLDIER,    // 剣士
        TANK,       // 盾
        GUNNER,     // 遠距離
        CAVALRY,    // 騎兵
    }

    // 移動方法
    public enum MoveType
    {
        RUN,    // 通常移動
    }

    // 目標
    public enum TargetType
    {
        BOTH,       // どちらも
        UNIT,       // ユニットのみ
        BUILDING,   // 建物のみ
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
    public Sprite JobSprite
    {
        get
        {
            switch (jobType)
            {
                case Types.JobType.SOLDIER:
                    return Resources.Load<Sprite>("Sprites/SoldierIcon");
                case Types.JobType.TANK:
                    return Resources.Load<Sprite>("Sprites/TankIcon");
                case Types.JobType.GUNNER:
                    return Resources.Load<Sprite>("Sprites/GunnerIcon");
                case Types.JobType.CAVALRY:
                    return Resources.Load<Sprite>("Sprites/CavalryIcon");
                default:
                    return Resources.Load<Sprite>("Sprites/DefaultIcon");
            }
        }
    }
    [Header("攻撃/移動 目標")]
    public Types.TargetType targetType; // 攻撃または移動対象

    [Header("最大HP")]
    public float maxHp;                 // 最大HP
    [Header("攻撃データ")]
    public AttackDataBase attackData;     // 攻撃スタッツ
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

    public MovementBase MovementBase
    {
        get
        {
            switch (moveType)
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
