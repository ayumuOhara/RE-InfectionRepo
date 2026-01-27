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
    Infection,
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
    public Button InfectionButton; //砲撃範囲ボタン
    public Button CostButton; //コスト回復ボタン

    [Header("城のレベルとコスト")]
    public TextMeshProUGUI Castle_text;
    public TextMeshProUGUI CastleMoney_text;

    [Header("爆弾の攻撃力のレベルとコスト")]
    public TextMeshProUGUI Canon_text;
    public TextMeshProUGUI CanonMoney_text;

    [Header("感染レベルとコスト")]
    public TextMeshProUGUI Infection_text;
    public TextMeshProUGUI InfectionMoney_text;

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
    public TextMeshProUGUI Details_text; //レベルアップ詳細

<<<<<<< HEAD
    [Header("レベルアップ詳細")]
    public TextMeshProUGUI CastleDetails_text; //城
    private int CastleLevelUp=100; //城の最大HP
    public TextMeshProUGUI CanonDetails_text; //砲撃
    private int CanonLevelUp=30; //砲撃威力
=======
   
>>>>>>> parent of a41bf10 (Revert "邱ｨ謌蝉ｿｮ豁｣")

    [Header("所持金")]
    public int money = 1000;

    //城
    private int Castle_level = 0;
    private int CastleMoney = 300;

    //砲撃
    private int Canon_level = 0;
    private int CanonMoney = 300;

    //砲撃範囲
    private int Infection_level = 0;
    private int InfectionMoney = 300;

    //コスト
    private int Cost_level = 0;
    private int CostMoney = 300;

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

        Infection_text.text = ($"{Infection_level}");
        InfectionMoney_text.text = ($"{InfectionMoney}");

        Cost_text.text = ($"{Cost_level}");
        CostMoney_text.text = ($"{CostMoney}");

        Warning_text.text = "";
        WarningObj.SetActive(false);

<<<<<<< HEAD
        CastleDetails_text.text = $"城のHPがアップ\n最大HPは{CastleLevelUp}です";

        CanonDetails_text.text = $"爆弾の威力アップ\n威力値は{CanonLevelUp}です";
    }
    private void Update()
    {
        CastleDetails_text.text = $"城の最大HPがアップ\n最大HPは{CastleLevelUp}です";

        CanonDetails_text.text = $"爆弾の威力アップ\n威力値は{CanonLevelUp}です";
    }
=======
     
    }
   
   
>>>>>>> parent of a41bf10 (Revert "邱ｨ謌蝉ｿｮ豁｣")
    //城の強化ボタン
    public void CastleSkillEnhancement()
    {
        if (Castle_level == 0)
        {
            Details_text.text = "HP：100　　→　　300";
        }
        else if (Castle_level == 1)
        {
            Details_text.text = "HP：300　　→　　500";
        }
        else if (Castle_level == 2)
        {
            Details_text.text = "HP：500　　→　　1000";
        }
       else
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
        if (Canon_level == 0)
        {
            Details_text.text = "威力：30　　→　　50";
        }
        else if (Canon_level == 1)
        {
            Details_text.text = "威力：50　　→　　80";
        }
        else if (Canon_level == 2)
        {
            Details_text.text = "威力：80　　→　　100";
        }
        else
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

    public void InfectionSkillEnhacement()
    {
        if (Infection_level == 0)
        {
            Details_text.text = "HP：0.5　　→　　0.6\n"+
                "速度：10 　→　　9";
        }
        else if (Infection_level == 1)
        {
            Details_text.text = "HP：0.6　　→　　0.7\n" +
                "速度：9 　→　　7.5";
        }
        else if (Infection_level == 2)
        {
            Details_text.text = Details_text.text = "HP：0.7　　→　　0.8\n" +
                "速度：7.5　 →　　5";
        }
        else
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        currentUpgrade = UpgradeType.Infection;

        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        DialogLevel_text1.text = ($"{Infection_level}");
        DialogLevel_text2.text = ($"{Infection_level + 1}");
        DialogMoney_text.text = ($"{InfectionMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "感染を強化しますか？";
    }

    public void CostSkillEnhacement()
    {
        if (Cost_level == 0)
        {
            Details_text.text = "MAX：30　　→　　35\n" +
                "回復量：1.6　 →　　1.4";
        }
        else if (Cost_level == 1)
        {
            Details_text.text = "MAX：35　　→　　40\n" +
                "回復量：1.4　 →　　1.2";
        }
        else if (Cost_level == 2)
        {
            Details_text.text = "MAX：40　　→　　50\n" +
                "回復量：1.2　 →　　1";
        }
        else
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

            case UpgradeType.Infection:
                TryUpgradeInfection();
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

        if (CastleMoney ==300)
        {
            CastleMoney = 800; // 次のコスト
        }
        else if (CastleMoney == 800)
        {
            CastleMoney = 1500;
        }

<<<<<<< HEAD
        if (Castle_level == 1)
=======
     
        //レベルマックスでテキストをMAXにする
         if (Castle_level >= 3)
        {
            CastleMoney_text.text = "MAX";
           
        }
        else
>>>>>>> parent of a41bf10 (Revert "邱ｨ謌蝉ｿｮ豁｣")
        {
            CastleLevelUp = 300;
            CastleMoney_text.text = $"{CastleMoney}";
        }
<<<<<<< HEAD
        else if (Castle_level == 2)
        {
            CastleLevelUp = 500;
            CastleMoney_text.text = $"{CastleMoney}";
        }
        //レベルマックスでテキストをMAXにする
        else if (Castle_level >= 3)
        {
            CastleMoney_text.text = "MAX";
            CastleLevelUp = 1000;
        }
      
=======
>>>>>>> parent of a41bf10 (Revert "邱ｨ謌蝉ｿｮ豁｣")
       
       
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

        if (CanonMoney == 300)
        {
            CanonMoney = 800; // 次のコスト
        }
        else if (CanonMoney == 800)
        {
            CanonMoney = 1500;
<<<<<<< HEAD
        }
        if (Canon_level == 1)
        {
            CanonLevelUp = 50;
        }
        else if (Canon_level == 2)
        {
            CanonLevelUp = 80;
        }
=======
        }
       
     
>>>>>>> parent of a41bf10 (Revert "邱ｨ謌蝉ｿｮ豁｣")
        //レベルマックスでテキストをMAXにする
        else if (Canon_level >= 3)
        {
<<<<<<< HEAD
            CanonLevelUp = 100;
=======
         
>>>>>>> parent of a41bf10 (Revert "邱ｨ謌蝉ｿｮ豁｣")
            CanonMoney_text.text = "MAX";
        }
        else
        {
            CanonMoney_text.text = $"{CanonMoney}";
        }

        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
    }

    private void TryUpgradeInfection()
    {
        if (Infection_level >= 3)
        {
            StartCoroutine(WarningLevelText());
            return;
        }
        if (money < InfectionMoney)
        {
            Debug.Log("所持金が足りません");
            StartCoroutine(WarningMoneyText());
            return;
        }

        money -= InfectionMoney;
        money_text.text = $"{money}";

        Infection_level++;
        Infection_text.text = $"{Infection_level}";

        //レベル３になった時文字を赤くする
        if (Infection_level >= 3)
        {
            Infection_text.color = new Color(1f, 0.337f, 0.337f);
            //ボタンを押せなくする
            InfectionButton.interactable = false;
        }

        if (InfectionMoney == 300) 
        {
            InfectionMoney = 800; // 次のコスト
            InfectionMoney_text.text = $"{InfectionMoney}";
        }
        else if(InfectionMoney == 800)
        {
            InfectionMoney = 1500;
            InfectionMoney_text.text = $"{InfectionMoney}";
        }

        //レベルマックスでテキストをMAXにする
        if (Infection_level >= 3)
        {
            InfectionMoney_text.text = "MAX";
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

        if (CostMoney == 300)
        {
            CostMoney = 800; // 次のコスト
        }
        else if (CostMoney == 800)
        {
            CostMoney = 1500;
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
