using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public enum UpgradeType
{
    Castle,
    Canon,
    CanonRange,
    Cost
}
public class ShopManager:MonoBehaviour
{
    [Header("UI")]
    public GameObject DialogObj;
    public TextMeshProUGUI money_text;

    [Header("城のレベルとコスト")]
    public TextMeshProUGUI Castle_text;
    public TextMeshProUGUI CastleMoney_text;

    [Header("砲撃の攻撃力のレベルとコスト")]
    public TextMeshProUGUI Canon_text;
    public TextMeshProUGUI CanonMoney_text;

    [Header("砲撃の範囲のレベルとコスト")]
    public TextMeshProUGUI CanonRange_text;
    public TextMeshProUGUI CanonRangeMoney_text;

    [Header("コストの回復速度のレベルとコスト")]
    public TextMeshProUGUI Cost_text;
    public TextMeshProUGUI CostMoney_text;

    [Header("ダイアログ表示用")]
    public TextMeshProUGUI DialogMessege;
    public TextMeshProUGUI DialogLevel_text1;
    public TextMeshProUGUI DialogLevel_text2;
    public TextMeshProUGUI DialogMoney_text;

    
    [Header("所持金")]
    public int money = 1000;

    //城
    private int Castle_level = 1;
    private int CastleMoney = 100;

    //砲撃
    private int Canon_level = 1;
    private int CanonMoney = 100;

    //砲撃範囲
    private int CanonRange_level = 1;
    private int CanonRangeMoney = 100;

    //コスト
    private int Cost_level = 1;
    private int CostMoney = 100;

    private UpgradeType currentUpgrade;

    private void Start()
    {
        DialogObj.SetActive(false);
        money_text.text = ($"{money}");

        Castle_text.text = ($"{Castle_level}");
        CastleMoney_text.text = ($"{CastleMoney}");

        Canon_text.text = ($"{Canon_level}");
        CanonMoney_text.text = ($"{CanonMoney}");

        CanonRange_text.text = ($"{CanonRange_level}");
        CanonRangeMoney_text.text = ($"{CanonRangeMoney}");

        Cost_text.text = ($"{Cost_level}");
        CostMoney_text.text = ($"{CostMoney}");
    }

    //城の強化ボタン
    public void CastleSkillEnhancement()
    {
        currentUpgrade = UpgradeType.Castle;
        DialogObj.SetActive(true);
        
        DialogLevel_text1.text = ($"{Castle_level}");
        DialogLevel_text2.text = ($"{Castle_level + 1}");
        DialogMoney_text.text = ($"{CastleMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "城のHPを強化しますか？";
    }

    public void CanonSkillEnhancement()
    {
        currentUpgrade = UpgradeType.Canon;

        DialogObj.SetActive(true);

        DialogLevel_text1.text = ($"{Canon_level}");
        DialogLevel_text2.text = ($"{Canon_level + 1}");
        DialogMoney_text.text = ($"{CanonMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "砲撃の攻撃力を強化しますか？";
    }

    public void CanonRangeSkillEnhacement()
    {
        currentUpgrade = UpgradeType.CanonRange;

        DialogObj.SetActive(true);

        DialogLevel_text1.text = ($"{CanonRange_level}");
        DialogLevel_text2.text = ($"{CanonRange_level + 1}");
        DialogMoney_text.text = ($"{CanonRangeMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "砲撃の範囲を強化しますか？";
    }

    public void CostSkillEnhacement()
    {
        currentUpgrade = UpgradeType.Cost;

        DialogObj.SetActive(true);

        DialogLevel_text1.text = ($"{Cost_level}");
        DialogLevel_text2.text = ($"{Cost_level + 1}");
        DialogMoney_text.text = ($"{CostMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "コストの回復力を強化しますか？";
    }
    public void YesButton()
    {
        switch (currentUpgrade)
        {
            case UpgradeType.Castle:
                TryUpgradeCastle();
                break;

            case UpgradeType.Canon:
                TryUpgradeCanon();
                break;

            case UpgradeType.CanonRange:
                TryUpgradeCanonRange();
                break;

            case UpgradeType.Cost:
                TryUpgradeCost();
                break;

        }

    }

    public void NoButton()
    {
        DialogObj.SetActive(false);
    }

    //城の強化処理
    private void TryUpgradeCastle()
    {
        if (money < CastleMoney)
        {
            Debug.Log("所持金が足りません");
            return;
        }

        money -= CastleMoney;
        money_text.text = $"{money}";

        Castle_level++;
        Castle_text.text = $"{Castle_level}";

        if (CastleMoney == 100)
        {
            CastleMoney = 500; // 次のコスト
        }
        else if (CastleMoney == 500)
        {
            CastleMoney = 1000;
        }
        CastleMoney_text.text = $"{CastleMoney}";

        DialogObj.SetActive(false);
    }

    // 砲撃の強化処理
     private void TryUpgradeCanon()
    {
        if (money < CanonMoney)
        {
            Debug.Log("所持金が足りません");
            return;
        }

        money -= CanonMoney;
        money_text.text = $"{money}";

        Canon_level++;
        Canon_text.text = $"{Canon_level}";

        if (CanonMoney == 100)
        {
            CanonMoney = 500; // 次のコスト
        }
        else if (CanonMoney == 500)
        {
            CanonMoney = 1000;
        }
            CanonMoney_text.text = $"{CanonMoney}";

        DialogObj.SetActive(false);
    }

    private void TryUpgradeCanonRange()
    {
        if (money < CanonRangeMoney)
        {
            Debug.Log("所持金が足りません");
            return;
        }

        money -= CanonRangeMoney;
        money_text.text = $"{money}";

        CanonRange_level++;
        CanonRange_text.text = $"{CanonRange_level}";


        if (CanonRangeMoney == 100) 
        { 
        CanonRangeMoney = 500; // 次のコスト
        }
        else if(CanonRangeMoney == 500)
        {
            CanonRangeMoney = 1000;
        }
        CanonRangeMoney_text.text = $"{CanonRangeMoney}";

        DialogObj.SetActive(false);
    }

    private void TryUpgradeCost()
    {
        if (money < CostMoney)
        {
            Debug.Log("所持金が足りません");
            return;
        }

        money -= CostMoney;
        money_text.text = $"{money}";

        Cost_level++;
        Cost_text.text = $"{Cost_level}";


        if (CostMoney == 100)
        {
            CostMoney = 500; // 次のコスト
        }
        else if (CostMoney == 500)
        {
            CostMoney = 1000;
        }
        CostMoney_text.text = $"{CostMoney}";

        DialogObj.SetActive(false);
    }
}
