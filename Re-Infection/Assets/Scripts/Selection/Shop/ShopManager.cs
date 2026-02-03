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
    private int CastleMoney
    {
        get
        {
            switch (playerStatusData.castleAbility.lv)
            {
                case 0:
                    return 300;
                case 1:
                    return 800;
                case 2:
                    return 1500;
                default:
                    return 0;
            }
        }
    }

    //砲撃
    private int CanonMoney
    {
        get
        {
            switch (playerStatusData.cannonAbility.lv)
            {
                case 0:
                    return 300;
                case 1:
                    return 800;
                case 2:
                    return 1500;
                default:
                    return 0;
            }
        }
    }

    //コスト
    private int CostMoney
    {
        get
        {
            switch (playerStatusData.costAbility.lv)
            {
                case 0:
                    return 300;
                case 1:
                    return 800;
                case 2:
                    return 1500;
                default:
                    return 0;
            }
        }
    }

    //感染
    private int InfectionMoney
    {
        get
        {
            switch (playerStatusData.virusAbility.lv)
            {
                case 0:
                    return 300;
                case 1:
                    return 800;
                case 2:
                    return 1500;
                default:
                    return 0;
            }
        }
    }

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

        SetAbilityTextAndButton(playerStatusData.castleAbility);
        SetAbilityTextAndButton(playerStatusData.cannonAbility);
        SetAbilityTextAndButton(playerStatusData.costAbility);
        SetAbilityTextAndButton(playerStatusData.virusAbility);

        Warning_text.text = "";
        WarningObj.SetActive(false);
    }

    // 渡された強化内容によってUI表示を操作
    private void SetAbilityTextAndButton(BaseAbility ability)
    {
        switch (ability.GetType().ToString())
        {
            case "CastleAbility":
                SetTextAndButton(Castle_text, CastleMoney_text, playerStatusData.castleAbility.lv, CastleMoney, CastleButton);
                break;
            case "CannonAbility":
                SetTextAndButton(Canon_text, CanonMoney_text, playerStatusData.cannonAbility.lv, CanonMoney, CanonButton);
                break;
            case "CostAbility":
                SetTextAndButton(Cost_text, CostMoney_text, playerStatusData.costAbility.lv, CostMoney, CostButton);
                break;
            case "VirusAbility":
                SetTextAndButton(Infection_text, InfectionMoney_text, playerStatusData.virusAbility.lv, InfectionMoney, InfectionButton);
                break;
            default:
                break;
        }
    }

    // 指定の強化内容のLvと必要なお金の表示切替
    // レベルが最大の時、ボタンが触れられなくなる
    private void SetTextAndButton(TextMeshProUGUI lvText, TextMeshProUGUI moneyText, int lv, int money, Button button)
    {
        lvText.text = lv.ToString();
        moneyText.text = lv >= 3 ? "MAX" : $"{money}";

        //レベル３になった時文字を赤くする
        if (lv >= 3)
        {
            lvText.color = new Color(1f, 0.337f, 0.337f);
            moneyText.color = new Color(1f, 0.337f, 0.337f);
            //ボタンを押せなくする
            if(button != null) button.interactable = false;
        }
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
                TryUpgrade(playerStatusData.castleAbility, CastleMoney);
                break;

            case UpgradeType.Canon:
                TryUpgrade(playerStatusData.cannonAbility, CanonMoney);
                break;

            case UpgradeType.Infection:
                TryUpgrade(playerStatusData.virusAbility, InfectionMoney);
                break;

            case UpgradeType.Cost:
                TryUpgrade(playerStatusData.costAbility, CostMoney);
                break;

        }

    }

    public void NoButton()
    {
        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
    }

    // 渡された強化要素のアップグレードを行う
    private void TryUpgrade(BaseAbility ability, int money)
    {
        if (ability.lv >= 3)
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        if (!playerStatusData.wallet.CanBuy(money))
        {
            Debug.Log("所持金が足りません");
            StartCoroutine(WarningMoneyText());
            return;
        }

        playerStatusData.wallet.RemoveMoney(money);
        money_text.text = $"{playerStatusData.wallet.CurrentMoney}";

        ability.SetAbilityLevel(ability.lv + 1);
        SetAbilityTextAndButton(ability);

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
