using UnityEngine;

[System.Serializable]
public class Wallet
{
    [SerializeField]
    // ŠŽ‹à
    private int currentMoney;
    public int CurrentMoney => PlayerPrefs.GetInt("Money", 0);

    // ŠŽ‹àÅ‘å’l
    public static readonly int MAX_HOLD_MONEY = 99999;

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        if (currentMoney > MAX_HOLD_MONEY)
        {
            currentMoney = MAX_HOLD_MONEY;
        }

        PlayerPrefs.SetInt("Money", currentMoney);
    }

    public void RemoveMoney(int amount)
    {
        currentMoney -= amount;
        if (currentMoney < 0)
        {
            currentMoney = 0;
        }

        PlayerPrefs.SetInt("Money", currentMoney);
    }

    public bool CanBuy(int value)
    {
        return CurrentMoney >= value;
    }
}