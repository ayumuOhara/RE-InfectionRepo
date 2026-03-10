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
public class StatusScaler
{
    [Header("最大HPの上昇値")]
    [SerializeField]
    private float maxHpScaler;
    public float MaxHPScaler => maxHpScaler;

    [Header("攻撃力の上昇値")]
    [SerializeField]
    private float atkScaler;
    public float AtkScaler => atkScaler;

    [Header("次のレベルアップに必要なお金(※最大レベルでは無効)")]
    [SerializeField]
    private uint levelUpCost;
    public uint LevelUpCost => levelUpCost;
}

//-----------------------------------------------------------------------------------------------------------
// ユニットのレベルにある各関数の使い方に当たっては、以下のURLに飛んでください。
// https://teams.microsoft.com/l/message/19:26575505d35b4ce6893309db020c6b78@thread.tacv2/1772156039529?tenantId=06d7146c-4c1a-47ba-bd34-84ba7e0a07f3&groupId=6350995f-6ead-4631-9c94-1e7df7a2198e&parentMessageId=1772156039529&teamName=%E3%82%B2%E3%83%BC%E3%83%A0%E3%83%BB%E3%82%AF%E3%83%AA%E3%82%A8%E3%82%A4%E3%82%BF%E3%83%BC%E7%A7%91%EF%BC%882024%E5%B9%B4%E5%BA%A6%EF%BC%89&channelName=%E3%82%B2%E3%83%BC%E3%83%A0%E9%96%8B%E7%99%BA_%E3%81%BF%E3%82%93%E3%81%AA%E3%81%A7%E6%84%9F%E6%9F%93%E3%83%81%E3%83%BC%E3%83%A0&createdTime=1772156039529
// また、それでも各関数の使い方や処理に関して不明な点がある場合は、担当者に質問するなどしてください。
//-----------------------------------------------------------------------------------------------------------

[System.Serializable]
public class UnitStats
{
    [Header("ユニットが解放されたか")]
    [SerializeField] private bool isUnlocked;
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

    [Header("Lv毎の上昇値")]
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
    public int lv => level.GetLevel(unitName + "Level");  // ユニットのレベル
    public int LvIdx => level.LvIdx;    // 配列の添え字に使うレベル
    public int MaxLevel => statusScaler.Length;     // レベルの最大値 
    public void SetLevel(int lv) => level.SetLevel(unitName + "Level", lv);     // 引数をレベルに設定
    public void SetMaxLevel(int lv) => level.SetMaxLevel(lv);   // 最大レベルを設定
    public void LevelUP() => level.SetLevel(unitName + "Level", lv + 1);    // 次のレベルへアップ
    public int GetNextLevelCost() => (int)statusScaler[LvIdx].LevelUpCost; // レベルアップに必要なコストを取得

    public static event Action OnUnlockUnit;

    public void UnitUnLock()
    {
        PlayerPrefs.SetInt(unitName + "Unlock", 1);

        OnUnlockUnit?.Invoke();
    }

    public bool IsUnitUnlocked()
    {
        return PlayerPrefs.GetInt(unitName + "Unlock", 0) == 1 || isUnlocked ? true : false;
    }

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

    // 現在のレベルの体力
    public float GetCurrentLevelMaxHp()
    {
        if (statusScaler == null) return maxHp;
        return maxHp +  statusScaler[LvIdx].MaxHPScaler;
    }

    // 現在のレベルの攻撃力
    public float GetCurrentLevelAtk()
    {
        if (statusScaler == null) return atk;
        return atk + statusScaler[LvIdx].AtkScaler;
    }

    // 渡されたレベルの体力
    public float GetLevelofMaxHp(int lv)
    {
        if (statusScaler == null) return maxHp;
        return maxHp + statusScaler[level.ClampLevelIndex(lv)].MaxHPScaler;
    }

    // 渡されたレベルの攻撃力
    public float GetLevelofAtk(int lv)
    {
        if (statusScaler == null) return atk;
        return atk + statusScaler[level.ClampLevelIndex(lv)].AtkScaler;
    }
}

[CreateAssetMenu(fileName = "UnitStats", menuName = "Scriptable Objects/UnitStats")]
public class UnitStatsData : ScriptableObject
{
    public UnitStats unitStats;

    private void OnEnable()
    {
        if (unitStats.statusScaler != null)
        {
            unitStats.SetMaxLevel(unitStats.MaxLevel);
        }
    }
}
