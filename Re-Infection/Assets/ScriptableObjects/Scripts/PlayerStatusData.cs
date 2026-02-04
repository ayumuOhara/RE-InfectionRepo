using System;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

[System.Serializable]
public abstract class BaseAbility
{
    public int lv { get; private set; }

    public void SetAbilityLevel(int level)
    {
        lv = level;
    }
}

[System.Serializable]
public class CastleAbility : BaseAbility
{
    [Tooltip("最大HP")]
    [SerializeField]
    // Lvごとの体力設定
    private int[] healths;

    // 現在のレベルに応じた値を返す
    public int Health => healths[lv];
}

[System.Serializable]
public class CannonAbility : BaseAbility
{
    [Tooltip("砲撃時のダメージ")]
    [SerializeField]
    // Lvごとの体力設定
    private int[] damages;

    [Header("砲撃クールタイム")]
    [SerializeField]
    private float coolTime;

    // 現在のレベルに応じた値を返す
    public int Damage => damages[lv];

    public int CoolTime => (int)coolTime;
}

[System.Serializable]
public class CostAbility : BaseAbility
{
    [System.Serializable]
    public struct CostContext
    {
        [Tooltip("プレイヤーの持てるコストの最大値")]
        public int maxCost;
        [Tooltip("1コストが生成されるまでの時間(秒)")]
        public float interbal;
    }

    [Tooltip("コスト関連のスタッツ")]
    [SerializeField]
    // Lvごとの体力設定
    private CostContext[] costStats;

    // 現在のレベルに応じた値を返す
    public CostContext CostStats => costStats[lv];
}

[System.Serializable]
public class VirusAbility : BaseAbility
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
    public CastleAbility castleAbility;
    public CannonAbility cannonAbility;
    public CostAbility costAbility;
    public VirusAbility virusAbility;

    [ContextMenu("全アビリティのLvをリセット")]
    public void ResetAllLevels()
    {
        castleAbility.SetAbilityLevel(0);
        cannonAbility.SetAbilityLevel(0);
        costAbility.SetAbilityLevel(0);
        virusAbility.SetAbilityLevel(0);
    }
}
