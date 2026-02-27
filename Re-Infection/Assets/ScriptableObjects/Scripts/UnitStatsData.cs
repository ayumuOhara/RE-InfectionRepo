using System;
using System.Text;
using UnityEngine;

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

[Serializable]
public struct StatusScaler
{
    [Header("最大HPのLv補正")]
    [Range(0f, 2f)]
    public float maxHpScaler;

    [Header("攻撃力のLv補正")]
    [Range(0f, 2f)]
    public float atkScaler;

    [Header("次のレベルアップに必要なお金(※最大レベルでは無効)")]
    public uint levelUpCost;
}

//-----------------------------------------------------------------------------------------------------------
// ユニットのレベルにある各関数の使い方に当たっては、GitHubの以下の履歴の説明欄に記載されています。
// 【実装】　『担当：竹下歩』　ユニットのレベル機能を簡易的に実装 #193
// また、それでも各関数の使い方や処理に関して不明な点がある場合は、担当者に質問するなどしてください。
//-----------------------------------------------------------------------------------------------------------

[System.Serializable]
public class UnitStats
{
    [Header("アニメーター")]
    public RuntimeAnimatorController animatorController;           // ユニットのアニメーター
    [Header("スプライト")]
    public Sprite unitSprite;           // ユニットのスプライト
    [Header("攻撃エフェクト")]
    public GameObject attackEffect;     // ユニット攻撃時のエフェクト
    [Header("名前")]
    public string unitName;             // ユニット名
    [Header("役職")]
    public Types.JobType jobType;       // 役職
    
    [Header("攻撃/移動 目標")]
    public Types.TargetType targetType; // 攻撃または移動対象

    [Header("Lv毎の補正値")]
    public StatusScaler[] statusScaler;
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
    [Header("ボスユニットか")]
    public bool bossUnit;               // ボスか
    [Header("攻撃時のSE")]
    public AudioClip attackSe;          // 攻撃音

    private Level level = new Level();
    public int lv => level.lv;  // ユニットのレベル
    public int LvIdx => level.LvIdx;    // 配列の添え字に使うレベル
    public int MaxLevel => statusScaler.Length + 1;     // レベルの最大値 
    public void SetLevel(int lv) => level.SetLevel(lv);     // 引数をレベルに設定
    public void SetMaxLevel(int lv) => level.SetMaxLevel(lv);   // 最大レベルを設定
    public void LevelUP() => level.SetLevel(lv + 1);    // 次のレベルへアップ
    public int GetNextLevelCost() => (int)statusScaler[LvIdx].levelUpCost; // レベルアップに必要なコストを取得

    public Material GetOutline(string targetOutline)
    {
        return this.unitSprite.name switch
        {
            "Archer_0" => Resources.Load<Material>($"Materials/{targetOutline}/Archer"),
            "Warrior_0" => Resources.Load<Material>($"Materials/{targetOutline}/Warrior"),
            "Bow_0" => Resources.Load<Material>($"Materials/{targetOutline}/Bow"),
            "Witch_0" => Resources.Load<Material>($"Materials/{targetOutline}/Witch"),
            "Swordsman_0" => Resources.Load<Material>($"Materials/{targetOutline}/Swordsman"),
            "Tank_0" => Resources.Load<Material>($"Materials/{targetOutline}/Tank"),
            "Clergyman_0" => Resources.Load<Material>($"Materials/{targetOutline}/Clergyman"),
            "Jockey_0" => Resources.Load<Material>($"Materials/{targetOutline}/Jockey"),
            _ => null
        };
    }

    public Sprite JobSprite
    {
        get
        {
            return jobType switch
            {
                Types.JobType.SOLDIER => Resources.Load<Sprite>("Sprites/SoldierIcon"),
                Types.JobType.HAMMER => Resources.Load<Sprite>("Sprites/HammerIcon"),
                Types.JobType.TANK => Resources.Load<Sprite>("Sprites/TankIcon"),
                Types.JobType.ARCHER => Resources.Load<Sprite>("Sprites/ArcherIcon"),
                Types.JobType.MAGE => Resources.Load<Sprite>("Sprites/MageIcon"),
                Types.JobType.CAVALRY => Resources.Load<Sprite>("Sprites/CavalryIcon"),
                _ => Resources.Load<Sprite>("Sprites/DefaultIcon")
            };
        }
    }

    public AttackBase AttackBase
    {
        get
        {
            return attackType switch
            {
                Types.AttackType.SINGLE => new AttackOfSingle(),
                Types.AttackType.AREA_MELEE => new AttackOfAreaMelee(),
                Types.AttackType.AREA_RANGE => new AttackOfAreaRange(),
                _ => null
            };
        }
    }

    public MovementBase MovementBase
    {
        get
        {
            return moveType switch
            {
                Types.MoveType.RUN => new RunMovement(),
                _ => null
            };
        }
    }

    // 現在のレベルの体力の補正値
    public float GetCurrentLevelMaxHp()
    {
        return maxHp * statusScaler[LvIdx].maxHpScaler;
    }

    // 現在のレベルの攻撃力の補正値
    public float GetCurrentLevelAtk()
    {
        return atk * statusScaler[LvIdx].atkScaler;
    }

    // 渡されたレベルの体力の補正値
    public float GetLevelofMaxHp(int lv)
    {
        return maxHp * statusScaler[level.ClampLevelIndex(lv)].maxHpScaler;
    }

    // 渡されたレベルの攻撃力の補正値
    public float GetLevelofAtk(int lv)
    {
        return atk * statusScaler[level.ClampLevelIndex(lv)].atkScaler;
    }
}

[CreateAssetMenu(fileName = "UnitStats", menuName = "Scriptable Objects/UnitStats")]
public class UnitStatsData : ScriptableObject
{
    public UnitStats unitStats;

    private void OnEnable()
    {
        if(unitStats.statusScaler != null)
            unitStats.SetMaxLevel(unitStats.MaxLevel);
    }
}
