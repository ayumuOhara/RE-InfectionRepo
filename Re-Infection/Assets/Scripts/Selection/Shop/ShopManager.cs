using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public enum UpgradeType
{
    Castle,
    Canon,
    Virus,
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
    public Button VirusButton; //感染ボタン

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
    public TextMeshProUGUI Virus_text;
    public TextMeshProUGUI VirusMoney_text;

    [Header("ダイアログ表示用")]
    public TextMeshProUGUI DialogMessege;
    public TextMeshProUGUI DialogLevel_text1;
    public TextMeshProUGUI DialogLevel_text2;
    public TextMeshProUGUI DialogMoney_text;
    public GameObject WarningObj;
    public TextMeshProUGUI Warning_text;
    public TextMeshProUGUI Detalise_text;

    PlayerStatusData playerStatusData;

    private UpgradeType currentUpgrade;

    private void Awake()
    {
        playerStatusData = Resources.Load<PlayerStatusData>("PlayerStatusData");

        playerStatusData.castleUpgrade.SetUpgradeLevel(playerStatusData.castleUpgrade.lv);
        playerStatusData.cannonUpgrade.SetUpgradeLevel(playerStatusData.cannonUpgrade.lv);
        playerStatusData.costUpgrade.SetUpgradeLevel(playerStatusData.costUpgrade.lv);
        playerStatusData.virusUpgrade.SetUpgradeLevel(playerStatusData.virusUpgrade.lv);
    }

    private void Start()
    {
        DialogObj.SetActive(false);
        money_text.text = ($"{playerStatusData.wallet.CurrentMoney}");
        LayCastObj.SetActive(false);

        SetUpgradeTextAndButton(playerStatusData.castleUpgrade);
        SetUpgradeTextAndButton(playerStatusData.cannonUpgrade);
        SetUpgradeTextAndButton(playerStatusData.costUpgrade);
        SetUpgradeTextAndButton(playerStatusData.virusUpgrade);

        Warning_text.text = "";
        WarningObj.SetActive(false);
    }

    // 渡された強化内容によってUI表示を操作
    private void SetUpgradeTextAndButton(BaseUpgrade Upgrade)
    {
        switch (Upgrade.GetType().ToString())
        {
            case "CastleUpgrade":
                SetTextAndButton(playerStatusData.castleUpgrade, Castle_text, CastleMoney_text, CastleButton);
                break;
            case "CannonUpgrade":
                SetTextAndButton(playerStatusData.cannonUpgrade, Canon_text, CanonMoney_text, CanonButton);
                break;
            case "CostUpgrade":
                SetTextAndButton(playerStatusData.costUpgrade, Cost_text, CostMoney_text, CostButton);
                break;
            case "VirusUpgrade":
                SetTextAndButton(playerStatusData.virusUpgrade, Virus_text, VirusMoney_text, VirusButton);
                break;
            default:
                break;
        }
    }

    // 指定の強化内容のLvと必要なお金の表示切替
    // レベルが最大の時、ボタンが触れられなくなる
    private void SetTextAndButton(BaseUpgrade Upgrade, TextMeshProUGUI lvText, TextMeshProUGUI moneyText, Button button)
    {
        lvText.text = Upgrade.lv.ToString();
        moneyText.text = Upgrade.lv >= 3 ? "MAX" : $"<size=40><sprite=0><size=45>{Upgrade.UpgradeMoney}";

        if (playerStatusData.wallet.CurrentMoney < Upgrade.UpgradeMoney || Upgrade.lv >= Upgrade.MaxLevel)
        {
            lvText.color = new Color(1f, 0.337f, 0.337f);
            moneyText.color = new Color(1f, 0.337f, 0.337f);

            //ボタンを押せなくする
            if (button != null)
            {
                button.interactable = false;
            }
        }
    }

    //城の強化ボタン
    public void CastleSkillEnhancement()
    {
        if (playerStatusData.castleUpgrade.lv ==0)
        {
            Detalise_text.text = "HP：100　　→　　300";
        }
        else if (playerStatusData.castleUpgrade.lv == 1)
        {
            Detalise_text.text = "HP：300　　→　　500";
        }
        else if(playerStatusData.castleUpgrade.lv==2)
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

        DialogLevel_text1.text = ($"{playerStatusData.castleUpgrade.lv}");
        DialogLevel_text2.text = ($"{playerStatusData.castleUpgrade.lv + 1}");
        DialogMoney_text.text = ($"{playerStatusData.castleUpgrade.UpgradeMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "城のHPを強化しますか？";
    }

    public void CanonSkillEnhancement()
    {
        if (playerStatusData.cannonUpgrade.lv == 0)
        {
            Detalise_text.text = "威力：30　　→　　50";
        }
        else if (playerStatusData.cannonUpgrade.lv == 1)
        {
            Detalise_text.text = "威力：50　　→　　80";
        }
        else if (playerStatusData.cannonUpgrade.lv == 2)
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

        DialogLevel_text1.text = ($"{playerStatusData.cannonUpgrade.lv}");
        DialogLevel_text2.text = ($"{playerStatusData.cannonUpgrade.lv + 1}");
        DialogMoney_text.text = ($"{playerStatusData.cannonUpgrade.UpgradeMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "砲撃の攻撃力を強化しますか？";
    }

    public void CostSkillEnhacement()
    {
        if (playerStatusData.costUpgrade.lv == 0)
        {
            Detalise_text.text = "回復速度：1　　→　　1.3";
        }
        else if (playerStatusData.costUpgrade.lv == 1)
        {
            Detalise_text.text = "回復速度：1.3　　→　　1.5";
        }
        else if (playerStatusData.costUpgrade.lv == 2)
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

        DialogLevel_text1.text = ($"{playerStatusData.costUpgrade.lv}");
        DialogLevel_text2.text = ($"{playerStatusData.costUpgrade.lv + 1}");
        DialogMoney_text.text = ($"{playerStatusData.costUpgrade.UpgradeMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "コストの回復力を強化しますか？";
    }


    public void VirusSkillEnhacement()
    {
        if (playerStatusData.virusUpgrade.lv == 0)
        {
            Detalise_text.text = "感染者HP：0.5　　→　　0.6\n"+
                "感染速度：10　　→　　8";
        }
        else if (playerStatusData.virusUpgrade.lv == 1)
        {
            Detalise_text.text = "感染者HP：0.6　　→　　0.7\n" +
                "感染速度：8　　→　　6.5";
        }
        else if (playerStatusData.virusUpgrade.lv == 2)
        {
            Detalise_text.text = "感染者HP：0.7　　→　　0.8\n" +
                "感染速度：6.5　　→　　5";
        }
        else
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        currentUpgrade = UpgradeType.Virus;

        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        DialogLevel_text1.text = ($"{playerStatusData.virusUpgrade.lv}");
        DialogLevel_text2.text = ($"{playerStatusData.virusUpgrade.lv + 1}");
        DialogMoney_text.text = ($"{playerStatusData.virusUpgrade.UpgradeMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "感染を強化しますか？";
    }

    public void YesButton()
    {
        switch (currentUpgrade)
        {
            case UpgradeType.Castle:
                TryUpgrade(playerStatusData.castleUpgrade, playerStatusData.castleUpgrade.UpgradeMoney);
                break;

            case UpgradeType.Canon:
                TryUpgrade(playerStatusData.cannonUpgrade, playerStatusData.cannonUpgrade.UpgradeMoney);
                break;

            case UpgradeType.Virus:
                TryUpgrade(playerStatusData.virusUpgrade, playerStatusData.virusUpgrade.UpgradeMoney);
                break;

            case UpgradeType.Cost:
                TryUpgrade(playerStatusData.costUpgrade, playerStatusData.costUpgrade.UpgradeMoney);
                break;

        }

    }

    public void NoButton()
    {
        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
    }

    // 渡された強化要素のアップグレードを行う
    private void TryUpgrade(BaseUpgrade Upgrade, int money)
    {
        if (Upgrade.lv >= Upgrade.MaxLevel)
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

        Upgrade.SetUpgradeLevel(Upgrade.lv + 1);
        SetUpgradeTextAndButton(Upgrade);

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
