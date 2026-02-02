using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public Wallet wallet; // 所持金データ
    public TextMeshProUGUI moneyText; // 表示するテキスト

    private void Awake()
    {
        wallet = Resources.Load<PlayerStatusData>("PlayerStatusData").wallet;
    }

    void Start()
    {
        UpdateMoneyText();
    }

    public void UpdateMoneyText()
    {
        moneyText.text = $"{wallet.CurrentMoney}";
    }
}