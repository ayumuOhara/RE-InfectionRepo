using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.UI;

[Serializable]
public class Level
{
    private int maxlv;

    public void SetLevel(string key, int level)
    {
        ClampLevel(level);
        PlayerPrefs.SetInt(key, level);
        PlayerPrefs.Save();
    }

    public int GetLevel(string key)
    {
        return PlayerPrefs.GetInt(key, 1);
    }

    public void SetMaxLevel(int maxLevel) => maxlv = maxLevel;

    public int ClampLevel(int level)
    {
        return Mathf.Clamp(level, 1, maxlv);
    }

    public int ClampLevelIndex(int level)
    {
        return Mathf.Clamp(level - 1, 0, maxlv - 1);
    }
}

[System.Serializable]
public abstract class BaseUpgrade
{
    private Level level = new Level();

    [SerializeField] private string levelKey;

    public int lv => level.GetLevel(levelKey);
    public int LvIdx => level.ClampLevelIndex(lv);
    public int MaxLevel => upgradeMoney.Length + 1;
    public void SetUpgradeLevel(int lv) => level.SetLevel(levelKey, lv);
    public void SetMaxlevel() => level.SetMaxLevel(MaxLevel);
    public int ClampLevel(int lv) => level.ClampLevel(lv);
    public int ClampLevelIndex(int lv) => level.ClampLevelIndex(lv);
    public int UpgradeMoney => (int)upgradeMoney[lv == MaxLevel ? LvIdx - 1 : LvIdx];   // レベルが最大の時は添え字を-1する
                                                                                        // ※アップグレードのレベルの配列のサイズと費用の配列のサイズが1ずれている為
    public bool canUpgrade => lv < MaxLevel;

    [Header("各Lv(0~)のアップグレードのコスト(※最大レベルを除く)")]
    [Tooltip("各Lvからアップグレードする際に必要になるコインの数\nまた、配列のサイズがそのままレベルの最大値になる")]
    [SerializeField]
    private uint[] upgradeMoney;

    // アップグレードの性能を取得
    public virtual object GetLevelofUpgrade(int level)
    {
        return null;
    }
}

[System.Serializable]
public class CastleUpgrade : BaseUpgrade
{
    [Header("各Lv(0~)のアップグレード内容")]
    [SerializeField]
    // Lvごとの体力設定
    private int[] healths;

    public override object GetLevelofUpgrade(int level)
    {
        return healths[ClampLevelIndex(level)];
    }

    // 現在のレベルに応じた値を返す
    public int Health => healths[LvIdx];
}

[System.Serializable]
public class CannonDamageUpgrade : BaseUpgrade
{
    [Header("各Lv(0~)のアップグレード内容")]
    [SerializeField]
    // Lvごとの体力設定
    private int[] damages;

    public override object GetLevelofUpgrade(int level)
    {
        return damages[ClampLevelIndex(level)];
    }

    // 現在のレベルに応じた値を返す
    public int Damage => damages[LvIdx];
}

[System.Serializable]
public class CannonCoolTimeUpgrade : BaseUpgrade
{
    [Header("各Lv(0~)のアップグレード内容")]
    [SerializeField]
    private float[] coolTime;

    public override object GetLevelofUpgrade(int level)
    {
        return (int)coolTime[ClampLevelIndex(level)];
    }

    public int CoolTime => (int)coolTime[LvIdx];
}

[System.Serializable]
public class CostLimitUpgrade : BaseUpgrade
{
    [Header("各Lv(0~)のアップグレード内容")]
    [SerializeField]
    // Lvごとの体力設定
    private int[] maxCostCnt;

    public override object GetLevelofUpgrade(int level)
    {
        return maxCostCnt[ClampLevelIndex(level)];
    }

    // 現在のレベルに応じた値を返す
    public int MaxCost => maxCostCnt[LvIdx];
}

[System.Serializable]
public class CostGenerationSpeedUpgrade : BaseUpgrade
{
    [Header("各Lv(0~)のアップグレード内容")]
    [SerializeField]
    // Lvごとの体力設定
    private float[] generateSpeed;

    public override object GetLevelofUpgrade(int level)
    {
        return generateSpeed[ClampLevelIndex(level)];
    }

    // 現在のレベルに応じた値を返す
    public float GenerateSpeed => generateSpeed[LvIdx];
}

[System.Serializable]
public class VirusUpgrade : BaseUpgrade
{
    [Header("各Lv(0~)のアップグレード内容")]
    [Tooltip("感染ウイルスのスタッツ")]
    [Range(0, 1)]
    [SerializeField]
    // Lvごとの体力設定
    private float[] reviveHealthRate;

    public override object GetLevelofUpgrade(int level)
    {
        return reviveHealthRate[ClampLevelIndex(level)];
    }

    // 現在のレベルに応じた値を返す
    public float ReviveHealthRate => reviveHealthRate[LvIdx];
}

[CreateAssetMenu(fileName = "PlayerStatusData", menuName = "Scriptable Objects/PlayerStatusData")]
public class PlayerStatusData : ScriptableObject
{
    public Wallet wallet;
    public CastleUpgrade castleUpgrade;
    public CannonDamageUpgrade cannonDamageUpgrade;
    public CannonCoolTimeUpgrade cannonCoolTimeUpgrade;
    public CostLimitUpgrade costLimitUpgrade;
    public CostGenerationSpeedUpgrade costGenerationSpeedUpgrade;
    public VirusUpgrade virusUpgrade;

    private void OnEnable()
    {
        castleUpgrade.SetMaxlevel();
        cannonDamageUpgrade.SetMaxlevel();
        cannonCoolTimeUpgrade.SetMaxlevel();
        costLimitUpgrade.SetMaxlevel();
        costGenerationSpeedUpgrade.SetMaxlevel();
        virusUpgrade.SetMaxlevel();
    }

    [ContextMenu("全アビリティのLvをリセット")]
    public void ResetAllLevels()
    {
        castleUpgrade.SetUpgradeLevel(1);
        cannonDamageUpgrade.SetUpgradeLevel(1);
        cannonCoolTimeUpgrade.SetUpgradeLevel(1);
        costLimitUpgrade.SetUpgradeLevel(1);
        costGenerationSpeedUpgrade.SetUpgradeLevel(1);
        virusUpgrade.SetUpgradeLevel(1);
    }
}
