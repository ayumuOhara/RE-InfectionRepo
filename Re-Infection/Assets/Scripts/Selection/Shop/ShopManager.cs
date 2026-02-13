using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    private enum UpgradeType
    {
        Castle,
        CannonDamage,
        CannonCoolTime,
        CostLimit,
        CostGenerationSpeed,
        Virus,
    }

    private UpgradeType upgradeType;

    [Header("UI")]
    public GameObject DialogObj;
    public TextMeshProUGUI money_text;
    public GameObject LayCastObj;

    [Header("Buttons")]
    public Button CastleButton; //城の強化ボタン
    public Button CannonDamageButton; //砲撃強化ボタン
    public Button CannonCoolTimeButton; //砲撃強化ボタン
    public Button CostLimitButton; //コスト回復ボタン
    public Button CostGenerationSpeedButton; //コスト回復ボタン
    public Button VirusButton; //感染ボタン

    [Header("城のレベルとコスト")]
    public TextMeshProUGUI CastleLv_text;
    public TextMeshProUGUI CastleUpgradeMoney_text;

    [Header("砲撃の攻撃力のレベルとコスト")]
    public TextMeshProUGUI CannonDamageLv_text;
    public TextMeshProUGUI CannonDamageUpgradeMoney_text;

    [Header("砲撃のクールタイムのレベルとコスト")]
    public TextMeshProUGUI CannonCoolTimeLv_text;
    public TextMeshProUGUI CannonCoolTimeUpgradeMoney_text;

    [Header("コストの最大値のレベルとコスト")]
    public TextMeshProUGUI CostLimitLv_text;
    public TextMeshProUGUI CostLimitUpgradeMoney_text;

    [Header("コストの回復速度のレベルとコスト")]
    public TextMeshProUGUI CostGenerationSpeedLv_text;
    public TextMeshProUGUI CostGenerationSpeedUpgradeMoney_text;

    [Header("感染のレベルとコスト")]
    public TextMeshProUGUI VirusLv_text;
    public TextMeshProUGUI VirusUpgradeMoney_text;

    [Header("ダイアログ表示用")]
    public TextMeshProUGUI DialogMessege;
    public TextMeshProUGUI DialogLevel_text1;
    public TextMeshProUGUI DialogLevel_text2;
    public TextMeshProUGUI DialogMoney_text;
    public GameObject WarningObj;
    public TextMeshProUGUI Warning_text;
    public TextMeshProUGUI Detalise_text;

    PlayerStatusData playerStatusData;

    private void Awake()
    {
        playerStatusData = Resources.Load<PlayerStatusData>("PlayerStatusData");

        playerStatusData.castleUpgrade.SetUpgradeLevel(playerStatusData.castleUpgrade.lv);
        playerStatusData.cannonDamageUpgrade.SetUpgradeLevel(playerStatusData.cannonDamageUpgrade.lv);
        playerStatusData.cannonCoolTimeUpgrade.SetUpgradeLevel(playerStatusData.cannonCoolTimeUpgrade.lv);
        playerStatusData.costLimitUpgrade.SetUpgradeLevel(playerStatusData.costLimitUpgrade.lv);
        playerStatusData.costGenerationSpeedUpgrade.SetUpgradeLevel(playerStatusData.costGenerationSpeedUpgrade.lv);
        playerStatusData.virusUpgrade.SetUpgradeLevel(playerStatusData.virusUpgrade.lv);
    }

    private void Start()
    {
        DialogObj.SetActive(false);
        money_text.text = ($"{playerStatusData.wallet.CurrentMoney}");
        LayCastObj.SetActive(false);

        SetUpgradeTextAndButton(playerStatusData.castleUpgrade);
        SetUpgradeTextAndButton(playerStatusData.cannonDamageUpgrade);
        SetUpgradeTextAndButton(playerStatusData.cannonCoolTimeUpgrade);
        SetUpgradeTextAndButton(playerStatusData.costLimitUpgrade);
        SetUpgradeTextAndButton(playerStatusData.costGenerationSpeedUpgrade);
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
                SetTextAndButton(playerStatusData.castleUpgrade, CastleLv_text, CastleUpgradeMoney_text, CastleButton);
                break;
            case "CannonDamageUpgrade":
                SetTextAndButton(playerStatusData.cannonDamageUpgrade, CannonDamageLv_text, CannonDamageUpgradeMoney_text, CannonDamageButton);
                break;
            case "CannonCoolTimeUpgrade":
                SetTextAndButton(playerStatusData.cannonCoolTimeUpgrade, CannonCoolTimeLv_text, CannonCoolTimeUpgradeMoney_text, CannonCoolTimeButton);
                break;
            case "CostLimitUpgrade":
                SetTextAndButton(playerStatusData.costLimitUpgrade, CostLimitLv_text, CostLimitUpgradeMoney_text, CostLimitButton);
                break;
            case "CostGenerationSpeedUpgrade":
                SetTextAndButton(playerStatusData.costGenerationSpeedUpgrade, CostGenerationSpeedLv_text, CostGenerationSpeedUpgradeMoney_text, CostGenerationSpeedButton);
                break;
            case "VirusUpgrade":
                SetTextAndButton(playerStatusData.virusUpgrade, VirusLv_text, VirusUpgradeMoney_text, VirusButton);
                break;
            default:
                break;
        }
    }

    // アップグレードを承認するボタンの関数
    public void UndoUpgrade()
    {
        switch (upgradeType)
        {
            case UpgradeType.Castle:
                TryUpgrade(playerStatusData.castleUpgrade, playerStatusData.castleUpgrade.UpgradeMoney);
                break;
            case UpgradeType.CannonDamage:
                TryUpgrade(playerStatusData.cannonDamageUpgrade, playerStatusData.cannonDamageUpgrade.UpgradeMoney);
                break;
            case UpgradeType.CannonCoolTime:
                TryUpgrade(playerStatusData.cannonCoolTimeUpgrade, playerStatusData.cannonCoolTimeUpgrade.UpgradeMoney);
                break;
            case UpgradeType.CostLimit:
                TryUpgrade(playerStatusData.costLimitUpgrade, playerStatusData.costLimitUpgrade.UpgradeMoney);
                break;
            case UpgradeType.CostGenerationSpeed:
                TryUpgrade(playerStatusData.costGenerationSpeedUpgrade, playerStatusData.costGenerationSpeedUpgrade.UpgradeMoney);
                break;
            case UpgradeType.Virus:
                TryUpgrade(playerStatusData.virusUpgrade, playerStatusData.virusUpgrade.UpgradeMoney);
                break;
            default:
                break;
        }
    }

    // 指定の強化内容のLvと必要なお金の表示切替
    // レベルが最大の時、ボタンが触れられなくなる
    private void SetTextAndButton(BaseUpgrade upgrade, TextMeshProUGUI lvText, TextMeshProUGUI moneyText, Button button)
    {
        lvText.text = upgrade.lv.ToString();
        moneyText.text = upgrade.lv >= 3 ? "MAX" : $"<size=40><sprite=0><size=45>{upgrade.UpgradeMoney}";

        if (playerStatusData.wallet.CurrentMoney < upgrade.UpgradeMoney || upgrade.lv >= upgrade.MaxLevel)
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

        SEManager.Instance.PlaySE(SEManager.SEType.Upgrade);

        playerStatusData.wallet.RemoveMoney(money);
        money_text.text = $"{playerStatusData.wallet.CurrentMoney}";

        Upgrade.SetUpgradeLevel(Upgrade.lv + 1);
        SetUpgradeTextAndButton(Upgrade);

        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
    }

    public void CastleSkillEnhancement()
    {
        upgradeType = UpgradeType.Castle;

        if (playerStatusData.castleUpgrade.lv == 0)
        {
            Detalise_text.text = "HP：100　　→　　300";
        }
        else if (playerStatusData.castleUpgrade.lv == 1)
        {
            Detalise_text.text = "HP：300　　→　　500";
        }
        else if (playerStatusData.castleUpgrade.lv == 2)
        {
            Detalise_text.text = "HP：500　　→　　1000";
        }
        else
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        DialogLevel_text1.text = ($"{playerStatusData.castleUpgrade.lv}");
        DialogLevel_text2.text = ($"{playerStatusData.castleUpgrade.lv + 1}");
        DialogMoney_text.text = ($"{playerStatusData.castleUpgrade.UpgradeMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "城のHPを強化しますか？";
    }

    public void CannonDamageSkillEnhancement()
    {
        upgradeType = UpgradeType.CannonDamage;

        if (playerStatusData.cannonDamageUpgrade.lv == 0)
        {
            Detalise_text.text = "威力：40　　→　　70";
        }
        else if (playerStatusData.cannonDamageUpgrade.lv == 1)
        {
            Detalise_text.text = "威力：70　　→　　90";
        }
        else if (playerStatusData.cannonDamageUpgrade.lv == 2)
        {
            Detalise_text.text = "威力：90　　→　　100";
        }
        else
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        DialogLevel_text1.text = ($"{playerStatusData.cannonDamageUpgrade.lv}");
        DialogLevel_text2.text = ($"{playerStatusData.cannonDamageUpgrade.lv + 1}");
        DialogMoney_text.text = ($"{playerStatusData.cannonDamageUpgrade.UpgradeMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "砲撃の攻撃力を強化しますか？";
    }

    public void CannonCoolTimeSkillEnhancement()
    {
        upgradeType = UpgradeType.CannonCoolTime;

        if (playerStatusData.cannonCoolTimeUpgrade.lv == 0)
        {
            Detalise_text.text = "クールタイム：220　　→　　180";
        }
        else if (playerStatusData.cannonCoolTimeUpgrade.lv == 1)
        {
            Detalise_text.text = "クールタイム：180　　→　　150";
        }
        else if (playerStatusData.cannonCoolTimeUpgrade.lv == 2)
        {
            Detalise_text.text = "クールタイム：150　　→　　120";
        }
        else
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        DialogLevel_text1.text = ($"{playerStatusData.cannonCoolTimeUpgrade.lv}");
        DialogLevel_text2.text = ($"{playerStatusData.cannonCoolTimeUpgrade.lv + 1}");
        DialogMoney_text.text = ($"{playerStatusData.cannonCoolTimeUpgrade.UpgradeMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "クールタイムを強化しますか？";
    }

    public void CostLimitSkillEnhacement()
    {
        upgradeType = UpgradeType.CostLimit;

        if (playerStatusData.costLimitUpgrade.lv == 0)
        {
            Detalise_text.text = "コスト最大値：30　　→　　40";
        }
        else if (playerStatusData.costLimitUpgrade.lv == 1)
        {
            Detalise_text.text = "コスト最大値：40　　→　　50";
        }
        else if (playerStatusData.costLimitUpgrade.lv == 2)
        {
            Detalise_text.text = "コスト最大値：50　　→　　60";
        }
        else
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        DialogLevel_text1.text = ($"{playerStatusData.costLimitUpgrade.lv}");
        DialogLevel_text2.text = ($"{playerStatusData.costLimitUpgrade.lv + 1}");
        DialogMoney_text.text = ($"{playerStatusData.costLimitUpgrade.UpgradeMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "コストの最大値を強化しますか？";
    }

    public void CostGenerationSpeedSkillEnhacement()
    {
        upgradeType = UpgradeType.CostGenerationSpeed;

        if (playerStatusData.costGenerationSpeedUpgrade.lv == 0)
        {
            Detalise_text.text = "生成速度：1.6　　→　　1.5";
        }
        else if (playerStatusData.costGenerationSpeedUpgrade.lv == 1)
        {
            Detalise_text.text = "生成速度：1.5　　→　　1.4";
        }
        else if (playerStatusData.costGenerationSpeedUpgrade.lv == 2)
        {
            Detalise_text.text = "生成速度：1.4　　→　　1.2";
        }
        else
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        DialogLevel_text1.text = ($"{playerStatusData.costGenerationSpeedUpgrade.lv}");
        DialogLevel_text2.text = ($"{playerStatusData.costGenerationSpeedUpgrade.lv + 1}");
        DialogMoney_text.text = ($"{playerStatusData.costGenerationSpeedUpgrade.UpgradeMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "コストの生成速度を強化しますか？";
    }

    public void VirusSkillEnhacement()
    {
        upgradeType = UpgradeType.Virus;

        if (playerStatusData.virusUpgrade.lv == 0)
        {
            Detalise_text.text = "感染者HP：0.3　　→　　0.5\n" +
                "感染速度：15　　→　　10";
        }
        else if (playerStatusData.virusUpgrade.lv == 1)
        {
            Detalise_text.text = "感染者HP：0.5　　→　　0.6\n" +
                "感染速度：10　　→　　7";
        }
        else if (playerStatusData.virusUpgrade.lv == 2)
        {
            Detalise_text.text = "感染者HP：0.6　　→　　0.8\n" +
                "感染速度：7　　→　　5";
        }
        else
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        DialogLevel_text1.text = ($"{playerStatusData.virusUpgrade.lv}");
        DialogLevel_text2.text = ($"{playerStatusData.virusUpgrade.lv + 1}");
        DialogMoney_text.text = ($"{playerStatusData.virusUpgrade.UpgradeMoney}");
        DialogMessege.text = "";
        DialogMessege.text = "感染を強化しますか？";
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
