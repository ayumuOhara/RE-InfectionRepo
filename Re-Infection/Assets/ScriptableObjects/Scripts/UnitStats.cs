using UnityEngine;

public class Types
{
    // 役職
    public enum JobType
    {
        SOLDIER,    // 近接攻撃職
        TANK,       // 近接防御職
        GUNNER,     // 遠距離職
    }

    // 攻撃方法
    public enum AttackType
    {
        ONE_STRIKE,         // 単発
        COMBO_STRIKE,       // 連撃
    }

    // 移動方法
    public enum MoveType
    {
        RUN,    // 通常移動
        WARP,   // ワープ
    }

    // 目標
    public enum TargetType
    {
        BOTH,       // どちらも
        UNIT,       // ユニットのみ
        BUILDING,   // 建物のみ
    }
}

[CreateAssetMenu(fileName = "UnitStats", menuName = "Scriptable Objects/UnitStats")]
public class UnitStats : ScriptableObject
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
    [Header("攻撃方法")]
    public Types.AttackType attackType; // 攻撃方法
    [Header("攻撃対象の数")]
    public int hitTargetCnt;            // 攻撃対象の数
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
                case Types.MoveType.WARP:
                    return null;
                default:
                    return null;
            }
        }
    }

    public AttackBase AttackBase
    {
        get
        {
            switch (attackType)
            {
                case Types.AttackType.ONE_STRIKE:
                    return null;
                case Types.AttackType.COMBO_STRIKE:
                    return null;
                default:
                    return null;
            }
        }
    }
}
