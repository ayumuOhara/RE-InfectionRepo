using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
    public GameObject LayCastObj;

    [Header("Buttons")]
    public Button CastleButton; //城の強化ボタン
    public Button CanonButton; //砲撃強化ボタン
    public Button CanonRangeButton; //砲撃範囲ボタン
    public Button CostButton; //コスト回復ボタン

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
    public GameObject WarningObj;
    public TextMeshProUGUI Warning_text;

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
        LayCastObj.SetActive(false);

        Castle_text.text = ($"{Castle_level}");
        CastleMoney_text.text = ($"{CastleMoney}");

        Canon_text.text = ($"{Canon_level}");
        CanonMoney_text.text = ($"{CanonMoney}");

        CanonRange_text.text = ($"{CanonRange_level}");
        CanonRangeMoney_text.text = ($"{CanonRangeMoney}");

        Cost_text.text = ($"{Cost_level}");
        CostMoney_text.text = ($"{CostMoney}");

        Warning_text.text = "";
        WarningObj.SetActive(false);
    }

    //城の強化ボタン
    public void CastleSkillEnhancement()
    {
        if (Castle_level >= 3)
        {
            StartCoroutine(WarningLevelText());
            return;
        }
        currentUpgrade = UpgradeType.Castle;
        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        DialogLevel_text1.text = ($"{Castle_level}");
        DialogLevel_text2.text = ($"{Castle_level + 1}");
        DialogMoney_text.text = ($"{CastleMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "城のHPを強化しますか？";
    }

    public void CanonSkillEnhancement()
    {
        if (Canon_level >= 3)
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        currentUpgrade = UpgradeType.Canon;

        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        DialogLevel_text1.text = ($"{Canon_level}");
        DialogLevel_text2.text = ($"{Canon_level + 1}");
        DialogMoney_text.text = ($"{CanonMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "砲撃の攻撃力を強化しますか？";
    }

    public void CanonRangeSkillEnhacement()
    {
        if (CanonRange_level >= 3)
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        currentUpgrade = UpgradeType.CanonRange;

        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        DialogLevel_text1.text = ($"{CanonRange_level}");
        DialogLevel_text2.text = ($"{CanonRange_level + 1}");
        DialogMoney_text.text = ($"{CanonRangeMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "砲撃の範囲を強化しますか？";
    }

    public void CostSkillEnhacement()
    {
        if (Cost_level >= 3)
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        currentUpgrade = UpgradeType.Cost;

        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

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
        LayCastObj.SetActive(false);
    }

    //城の強化処理
    private void  TryUpgradeCastle()
    {
        if (Castle_level >= 3)
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        if (money < CastleMoney)
        {
            Debug.Log("所持金が足りません");
            StartCoroutine(WarningMoneyText());
            return;
        }
      
        money -= CastleMoney;
        money_text.text = $"{money}";

        Castle_level++;
        Castle_text.text = $"{Castle_level}";

        //レベル３になった時文字を赤くする
        if (Castle_level >= 3)
        {
            Castle_text.color = new Color(1f, 0.337f, 0.337f);
           
            //ボタンを押せなくする
            CastleButton.interactable = false;
        }

        if (CastleMoney == 100)
        {
            CastleMoney = 500; // 次のコスト
        }
        else if (CastleMoney == 500)
        {
            CastleMoney = 1000;
        }

        //レベルマックスでテキストをMAXにする
        if (Castle_level >= 3)
        {
            CastleMoney_text.text = "MAX";
        }
        else
        {
            CastleMoney_text.text = $"{CastleMoney}";
        }
        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
    }

    // 砲撃の強化処理
     private void TryUpgradeCanon()
    {
        if (Canon_level >= 3)
        {
            StartCoroutine(WarningLevelText());
            return;
        }
        if (money < CanonMoney)
        {
            Debug.Log("所持金が足りません");
            StartCoroutine(WarningMoneyText());
            return;
        }

        money -= CanonMoney;
        money_text.text = $"{money}";

        Canon_level++;
        Canon_text.text = $"{Canon_level}";

        //レベル３になった時文字を赤くする
        if (Canon_level >= 3)
        {
            Canon_text.color = new Color(1f, 0.337f, 0.337f);
            //ボタンを押せなくする
            CanonButton.interactable = false;
        }

        if (CanonMoney == 100)
        {
            CanonMoney = 500; // 次のコスト
        }
        else if (CanonMoney == 500)
        {
            CanonMoney = 1000;
        }

        //レベルマックスでテキストをMAXにする
        if (Canon_level >= 3)
        {
            CanonMoney_text.text = "MAX";
        }
        else
        {
            CanonMoney_text.text = $"{CanonMoney}";
        }

        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
    }

    private void TryUpgradeCanonRange()
    {
        if (CanonRange_level >= 3)
        {
            StartCoroutine(WarningLevelText());
            return;
        }
        if (money < CanonRangeMoney)
        {
            Debug.Log("所持金が足りません");
            StartCoroutine(WarningMoneyText());
            return;
        }

        money -= CanonRangeMoney;
        money_text.text = $"{money}";

        CanonRange_level++;
        CanonRange_text.text = $"{CanonRange_level}";

        //レベル３になった時文字を赤くする
        if (CanonRange_level >= 3)
        {
            CanonRange_text.color = new Color(1f, 0.337f, 0.337f);
            //ボタンを押せなくする
            CanonRangeButton.interactable = false;
        }

        if (CanonRangeMoney == 100) 
        { 
        CanonRangeMoney = 500; // 次のコスト
        }
        else if(CanonRangeMoney == 500)
        {
            CanonRangeMoney = 1000;
        }

        //レベルマックスでテキストをMAXにする
        if (CanonRange_level >= 3)
        {
            CanonRangeMoney_text.text = "MAX";
        }
        else
        {
            CanonRangeMoney_text.text = $"{CanonRangeMoney}";
        }

        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
    }

    private void TryUpgradeCost()
    {
        if (Cost_level >= 3)
        {
            StartCoroutine(WarningLevelText());
            return;
        }
        if (money < CostMoney)
        {
            Debug.Log("所持金が足りません");
            StartCoroutine(WarningMoneyText());
            return;
        }

        money -= CostMoney;
        money_text.text = $"{money}";

        Cost_level++;
        Cost_text.text = $"{Cost_level}";

        //レベル３になった時文字を赤くする
        if (Cost_level >= 3)
        {
            Cost_text.color = new Color(1f, 0.337f, 0.337f);
            //ボタンを押せなくする
            CostButton.interactable = false;
        }

        if (CostMoney == 100)
        {
            CostMoney = 500; // 次のコスト
        }
        else if (CostMoney == 500)
        {
            CostMoney = 1000;
        }

        //レベルマックスでテキストをMAXにする
        if (Cost_level >= 3)
        {
            CostMoney_text.text = "MAX";
        }
        else
        {
            CostMoney_text.text = $"{CostMoney}";
        }

        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
    }

   public IEnumerator WarningMoneyText()
    {
       
        WarningObj.SetActive(true);
        Warning_text.text = ("お 金 が 足 り ま せ ん ！");
        yield return new WaitForSeconds(1f);
        WarningObj.SetActive(false);
    }

    public IEnumerator WarningLevelText()
    {

        WarningObj.SetActive(true);
        Warning_text.text = ("レ ベ ル マ ッ ク ス で す ！");
        yield return new WaitForSeconds(1f);
        WarningObj.SetActive(false);
    }
}
