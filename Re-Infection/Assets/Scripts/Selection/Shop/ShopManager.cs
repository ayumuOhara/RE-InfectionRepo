using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEditor.U2D.Aseprite;
using Unity.VisualScripting;

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
    public Button CostButton; //コスト回復ボタン
    public Button InfectionButton; //感染ボタン

    [Header("城のレベルとコスト")]
    public TextMeshProUGUI Castle_text;
    public TextMeshProUGUI CastleMoney_text;

    [Header("砲撃の攻撃力のレベルとコスト")]
    public TextMeshProUGUI Canon_text;
    public TextMeshProUGUI CanonMoney_text;

    [Header("コストの回復速度のレベルとコスト")]
    public TextMeshProUGUI Cost_text;
    public TextMeshProUGUI CostMoney_text;

    [Header("感染のレベルとコスト")]
    public TextMeshProUGUI Infection_text;
    public TextMeshProUGUI InfectionMoney_text;

    [Header("ダイアログ表示用")]
    public TextMeshProUGUI DialogMessege;
    public TextMeshProUGUI DialogLevel_text1;
    public TextMeshProUGUI DialogLevel_text2;
    public TextMeshProUGUI DialogMoney_text;
    public GameObject WarningObj;
    public TextMeshProUGUI Warning_text;
    public TextMeshProUGUI Detalise_text;

    PlayerStatusData playerStatusData;

    //城
    private int CastleMoney = 300;

    //砲撃
    private int CanonMoney = 300;

    //コスト
    private int CostMoney = 300;

    //感染
    private int InfectionMoney = 300;

    private UpgradeType currentUpgrade;

    private void Awake()
    {
        playerStatusData = Resources.Load<PlayerStatusData>("PlayerStatusData");
        playerStatusData.castleAbility.SetAbilityLevel(playerStatusData.castleAbility.lv);
        playerStatusData.cannonAbility.SetAbilityLevel(playerStatusData.cannonAbility.lv);
        playerStatusData.costAbility.SetAbilityLevel(playerStatusData.costAbility.lv);
        playerStatusData.virusAbility.SetAbilityLevel(playerStatusData.virusAbility.lv);
    }

    private void Start()
    {
        DialogObj.SetActive(false);
        money_text.text = ($"{playerStatusData.wallet.CurrentMoney}");
        LayCastObj.SetActive(false);

        Castle_text.text = ($"{playerStatusData.castleAbility.lv}");
        CastleMoney_text.text = ($"{CastleMoney}");

        Canon_text.text = ($"{playerStatusData.cannonAbility.lv}");
        CanonMoney_text.text = ($"{CanonMoney}");

        Infection_text.text = ($"{playerStatusData.virusAbility.lv}");
        InfectionMoney_text.text = ($"{InfectionMoney}");

        Cost_text.text = ($"{playerStatusData.costAbility.lv}");
        CostMoney_text.text = ($"{CostMoney}");

        Warning_text.text = "";
        WarningObj.SetActive(false);
    }

    //城の強化ボタン
    public void CastleSkillEnhancement()
    {
        if (playerStatusData.castleAbility.lv ==0)
        {
            Detalise_text.text = "HP：100　　→　　300";
        }
        else if (playerStatusData.castleAbility.lv == 1)
        {
            Detalise_text.text = "HP：300　　→　　500";
        }
        else if(playerStatusData.castleAbility.lv==2)
        {
            Detalise_text.text = "HP：500　　→　　1000";
        }
        else
        {
            StartCoroutine(WarningLevelText());
            return;
        }
        currentUpgrade = UpgradeType.Castle;
        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        DialogLevel_text1.text = ($"{playerStatusData.castleAbility.lv}");
        DialogLevel_text2.text = ($"{playerStatusData.castleAbility.lv + 1}");
        DialogMoney_text.text = ($"{CastleMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "城のHPを強化しますか？";
    }

    public void CanonSkillEnhancement()
    {
        if (playerStatusData.cannonAbility.lv == 0)
        {
            Detalise_text.text = "威力：30　　→　　50";
        }
        else if (playerStatusData.cannonAbility.lv == 1)
        {
            Detalise_text.text = "威力：50　　→　　80";
        }
        else if (playerStatusData.cannonAbility.lv == 2)
        {
            Detalise_text.text = "威力：80　　→　　100";
        }
        else
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        currentUpgrade = UpgradeType.Canon;

        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        DialogLevel_text1.text = ($"{playerStatusData.cannonAbility.lv}");
        DialogLevel_text2.text = ($"{playerStatusData.cannonAbility.lv + 1}");
        DialogMoney_text.text = ($"{CanonMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "砲撃の攻撃力を強化しますか？";
    }

    public void CostSkillEnhacement()
    {
        if (playerStatusData.costAbility.lv == 0)
        {
            Detalise_text.text = "回復速度：1　　→　　1.3";
        }
        else if (playerStatusData.costAbility.lv == 1)
        {
            Detalise_text.text = "回復速度：1.3　　→　　1.5";
        }
        else if (playerStatusData.costAbility.lv == 2)
        {
            Detalise_text.text = "回復速度：1.5　　→　　1.7";
        }
        else
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        currentUpgrade = UpgradeType.Cost;

        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        DialogLevel_text1.text = ($"{playerStatusData.costAbility.lv}");
        DialogLevel_text2.text = ($"{playerStatusData.costAbility.lv + 1}");
        DialogMoney_text.text = ($"{CostMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "コストの回復力を強化しますか？";
    }


    public void InfectionSkillEnhacement()
    {
        if (playerStatusData.virusAbility.lv == 0)
        {
            Detalise_text.text = "感染者HP：0.5　　→　　0.6\n"+
                "感染速度：10　　→　　8";
        }
        else if (playerStatusData.virusAbility.lv == 1)
        {
            Detalise_text.text = "感染者HP：0.6　　→　　0.7\n" +
                "感染速度：8　　→　　6.5";
        }
        else if (playerStatusData.virusAbility.lv == 2)
        {
            Detalise_text.text = "感染者HP：0.7　　→　　0.8\n" +
                "感染速度：6.5　　→　　5";
        }
        else
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        currentUpgrade = UpgradeType.Infection;

        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        DialogLevel_text1.text = ($"{playerStatusData.virusAbility.lv}");
        DialogLevel_text2.text = ($"{playerStatusData.virusAbility.lv + 1}");
        DialogMoney_text.text = ($"{InfectionMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "感染を強化しますか？";
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
        if (playerStatusData.castleAbility.lv >= 3)
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        if (!playerStatusData.wallet.CanBuy(CastleMoney))
        {
            Debug.Log("所持金が足りません");
            StartCoroutine(WarningMoneyText());
            return;
        }
      
        playerStatusData.wallet.RemoveMoney(CastleMoney);
        money_text.text = $"{playerStatusData.wallet.CurrentMoney}";

        playerStatusData.castleAbility.SetAbilityLevel(playerStatusData.castleAbility.lv + 1);
        Castle_text.text = $"{playerStatusData.castleAbility.lv}";

        //レベル３になった時文字を赤くする
        if (playerStatusData.castleAbility.lv >= 3)
        {
            Castle_text.color = new Color(1f, 0.337f, 0.337f);
           
            //ボタンを押せなくする
            CastleButton.interactable = false;
        }

        //レベルアップに比例して値上げ
        if (playerStatusData.castleAbility.lv == 0)
        {
            CastleMoney = 300;
            CastleMoney_text.text = $"{CastleMoney}";
        }
        else if (playerStatusData.castleAbility.lv == 1)
        {
            CastleMoney = 800;
            CastleMoney_text.text = $"{CastleMoney}";
        }
        else if (playerStatusData.castleAbility.lv == 2)
        {
            CastleMoney = 1500;
            CastleMoney_text.text = $"{CastleMoney}";
        }
        //レベルマックスでテキストをMAXにする
        else
        {
            CastleMoney_text.text = "MAX";
        }
      
        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
    }

    // 砲撃の強化処理
     private void TryUpgradeCanon()
    {
        if (playerStatusData.cannonAbility.lv >= 3)
        {
            StartCoroutine(WarningLevelText());
            return;
        }
        if (!playerStatusData.wallet.CanBuy(CanonMoney))
        {
            Debug.Log("所持金が足りません");
            StartCoroutine(WarningMoneyText());
            return;
        }

        playerStatusData.wallet.RemoveMoney(CanonMoney);
        money_text.text = $"{playerStatusData.wallet.CurrentMoney}";

        playerStatusData.cannonAbility.SetAbilityLevel(playerStatusData.cannonAbility.lv + 1);
        Canon_text.text = $"{playerStatusData.cannonAbility.lv}";

        //レベル３になった時文字を赤くする
        if (playerStatusData.cannonAbility.lv >= 3)
        {
            Canon_text.color = new Color(1f, 0.337f, 0.337f);
            //ボタンを押せなくする
            CanonButton.interactable = false;
        }

        //レベルアップに比例して値上げ
        if (playerStatusData.cannonAbility.lv == 0)
        {
            CanonMoney = 300;
            CanonMoney_text.text = $"{CanonMoney}";
        }
        else if (playerStatusData.cannonAbility.lv == 1)
        {
            CanonMoney = 800;
            CanonMoney_text.text = $"{CanonMoney}";
        }
        else if (playerStatusData.cannonAbility.lv == 2)
        {
            CanonMoney = 1500;
            CanonMoney_text.text = $"{CanonMoney}";
        }
        //レベルマックスでテキストをMAXにする
        else
        {
            CanonMoney_text.text = "MAX";
        }
      

        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
    }

    private void TryUpgradeCost()
    {
        if (playerStatusData.costAbility.lv >= 3)
        {
            StartCoroutine(WarningLevelText());
            return;
        }
        if (!playerStatusData.wallet.CanBuy(CostMoney))
        {
            Debug.Log("所持金が足りません");
            StartCoroutine(WarningMoneyText());
            return;
        }

        playerStatusData.wallet.RemoveMoney(CostMoney);
        money_text.text = $"{playerStatusData.wallet.CurrentMoney}";

        playerStatusData.costAbility.SetAbilityLevel(playerStatusData.costAbility.lv + 1);
        Cost_text.text = $"{playerStatusData.costAbility.lv}";

        //レベル３になった時文字を赤くする
        if (playerStatusData.costAbility.lv >= 3)
        {
            Cost_text.color = new Color(1f, 0.337f, 0.337f);
            //ボタンを押せなくする
            CostButton.interactable = false;
        }

        //レベルアップに比例して値上げ
        if (playerStatusData.costAbility.lv == 0)
        {
            CostMoney = 300;
            CostMoney_text.text = $"{CostMoney}";
        }
        else if (playerStatusData.costAbility.lv == 1)
        {
            CostMoney = 800;
            CostMoney_text.text = $"{CostMoney}";
        }
        else if (playerStatusData.costAbility.lv == 2)
        {
            CostMoney = 1500;
            CostMoney_text.text = $"{CostMoney}";
        }
        //レベルマックスでテキストをMAXにする
        else
        {
            CostMoney_text.text = "MAX";
        }
      
        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
    }

    private void TryUpgradeInfection()
    {
        if (playerStatusData.virusAbility.lv >= 3)
        {
            StartCoroutine(WarningLevelText());
            return;
        }
        if (!playerStatusData.wallet.CanBuy(InfectionMoney))
        {
            Debug.Log("所持金が足りません");
            StartCoroutine(WarningMoneyText());
            return;
        }

        playerStatusData.wallet.RemoveMoney(InfectionMoney);
        money_text.text = $"{playerStatusData.wallet.CurrentMoney}";

        playerStatusData.virusAbility.SetAbilityLevel(playerStatusData.virusAbility.lv + 1);
        Infection_text.text = $"{playerStatusData.virusAbility.lv}";

        //レベル３になった時文字を赤くする
        if (playerStatusData.virusAbility.lv >= 3)
        {
            Infection_text.color = new Color(1f, 0.337f, 0.337f);
            //ボタンを押せなくする
            InfectionButton.interactable = false;
        }

        //レベルアップに比例して値上げ
        if (playerStatusData.virusAbility.lv == 0)
        {
            InfectionMoney = 300;
            InfectionMoney_text.text = $"{InfectionMoney}";
        }
        else if (playerStatusData.virusAbility.lv == 1)
        {
            InfectionMoney = 800;
            InfectionMoney_text.text = $"{InfectionMoney}";
        }
        else if (playerStatusData.virusAbility.lv == 2)
        {
            InfectionMoney = 1500;
            InfectionMoney_text.text = $"{InfectionMoney}";
        }
        //レベルマックスでテキストをMAXにする
        else
        {
            InfectionMoney_text.text = "MAX";
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
