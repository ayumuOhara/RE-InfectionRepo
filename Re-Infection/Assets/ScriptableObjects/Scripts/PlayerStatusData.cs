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
        if (lv >= MaxLevel)
        {
            lv = MaxLevel;
        }
    }
}

[System.Serializable]
public class CastleUpgrade : BaseUpgrade
{
    [Header("各Lv(0~)のアップグレード内容")]
    [SerializeField]
    // Lvごとの体力設定
    private int[] healths;

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

    // 現在のレベルに応じた値を返す
    public int Damage => damages[lv];
}

[System.Serializable]
public class CannonCoolTimeUpgrade : BaseUpgrade
{
    [Header("各Lv(0~)のアップグレード内容")]
    [SerializeField]
    private float[] coolTime;

    public int CoolTime => (int)coolTime[lv];
}

[System.Serializable]
public class CostLimitUpgrade : BaseUpgrade
{
    [Header("各Lv(0~)のアップグレード内容")]
    [SerializeField]
    // Lvごとの体力設定
    private int[] maxCostCnt;

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

    // 現在のレベルに応じた値を返す
    public float GenerateSpeed => generateSpeed[lv];
}

[System.Serializable]
public class VirusUpgrade : BaseUpgrade
{
    [System.Serializable]
    public struct VirusContext
    {
        [Tooltip("ユニット感染完了時の体力回復の割合")]
        [Range(0, 1)]
        [SerializeField]
        public float reviveHealthRate;
        [Tooltip("ユニットの感染完了までの時間")]
        public float infectionTime;
    }

    [Header("各Lv(0~)のアップグレード内容")]
    [Tooltip("感染ウイルスのスタッツ")]
    [SerializeField]
    // Lvごとの体力設定
    private VirusContext[] virusStats;

    // 現在のレベルに応じた値を返す
    public VirusContext VirusStats => virusStats[lv];
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
