using UnityEngine;

[System.Serializable]
public class Wallet
{
    public int CurrentMoney => PlayerPrefs.GetInt("Money", 0);

    // ŠŽ‹àÅ‘å’l
    public static readonly int MAX_HOLD_MONEY = 99999;

    public void AddMoney(int amount)
    {
        PlayerPrefs.SetInt("Money", Mathf.Clamp(CurrentMoney + amount, 0, MAX_HOLD_MONEY));
        PlayerPrefs.Save();
    }

    public void RemoveMoney(int amount)
    {
        PlayerPrefs.SetInt("Money", Mathf.Clamp(CurrentMoney - amount, 0, MAX_HOLD_MONEY));
        PlayerPrefs.Save();
    }

    public bool CanBuy(int value)
    {
        return CurrentMoney >= value;
    }
}