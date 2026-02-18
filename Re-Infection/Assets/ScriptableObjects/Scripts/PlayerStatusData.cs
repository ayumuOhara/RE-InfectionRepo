using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

[System.Serializable]
public abstract class BaseUpgrade
{
    public int lv { get; private set; }
    public int MaxLevel => upgradeMoney.Length;

    [Header("各Lv(0~)のアップグレードのコスト(※最大レベルを除く)")]
    [Tooltip("各Lvからアップグレードする際に必要になるコインの数\nまた、配列のサイズがそのままレベルの最大値になる")]
    [SerializeField]
    private int[] upgradeMoney;
    public int UpgradeMoney => upgradeMoney[lv >= MaxLevel ? lv - 1 : lv];

    public void SetUpgradeLevel(int level)
    {
        lv = level;
        ClampLevel(level);
    }

    // アップグレードの性能を取得
    public virtual object GetLevelofUpgrade(int level)
    {
        return null;
    }

    protected int ClampLevel(int level)
    {
        return Mathf.Clamp(level, 0, MaxLevel - 1);
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
        ClampLevel(level);
        return healths[level];
    }

    // 現在のレベルに応じた値を返す
    public int Health => healths[lv];
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
        ClampLevel(level);
        return damages[level];
    }

    // 現在のレベルに応じた値を返す
    public int Damage => damages[lv];
}

[System.Serializable]
public class CannonCoolTimeUpgrade : BaseUpgrade
{
    [Header("各Lv(0~)のアップグレード内容")]
    [SerializeField]
    private float[] coolTime;

    public override object GetLevelofUpgrade(int level)
    {
        ClampLevel(level);
        return (int)coolTime[level];
    }

    public int CoolTime => (int)coolTime[lv];
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
        ClampLevel(level);
        return maxCostCnt[level];
    }

    // 現在のレベルに応じた値を返す
    public int MaxCost => maxCostCnt[lv];
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
        ClampLevel(level);
        return generateSpeed[level];
    }

    // 現在のレベルに応じた値を返す
    public float GenerateSpeed => generateSpeed[lv];
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
        ClampLevel(level);
        return reviveHealthRate[level];
    }

    // 現在のレベルに応じた値を返す
    public float ReviveHealthRate => reviveHealthRate[lv];
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

    [ContextMenu("全アビリティのLvをリセット")]
    public void ResetAllLevels()
    {
        castleUpgrade.SetUpgradeLevel(0);
        cannonDamageUpgrade.SetUpgradeLevel(0);
        cannonCoolTimeUpgrade.SetUpgradeLevel(0);
        costLimitUpgrade.SetUpgradeLevel(0);
        costGenerationSpeedUpgrade.SetUpgradeLevel(0);
        virusUpgrade.SetUpgradeLevel(0);
    }
}
