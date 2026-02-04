using UnityEngine;

[System.Serializable]
public class Wallet
{
    [SerializeField]
    // ŠŽ‹à
    private int currentMoney;
    public int CurrentMoney => currentMoney;

    // ŠŽ‹àÅ‘å’l
    public static readonly int MAX_HOLD_MONEY = 99999;

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        if (currentMoney > MAX_HOLD_MONEY)
        {
            currentMoney = MAX_HOLD_MONEY;
        }
    }

    public void RemoveMoney(int amount)
    {
        currentMoney -= amount;
        if (currentMoney < 0)
        {
            currentMoney = 0;
        }
    }

    public bool CanBuy(int amount)
    {
        return currentMoney >= amount;
    }
}