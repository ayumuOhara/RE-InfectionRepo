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
    private void SetUpgradeTextAndButton<T>(T upgrade) where T : BaseUpgrade
    {
        switch (upgrade.GetType().ToString())
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

        SetUpgradeTextAndButton(playerStatusData.castleUpgrade);
        SetUpgradeTextAndButton(playerStatusData.cannonDamageUpgrade);
        SetUpgradeTextAndButton(playerStatusData.cannonCoolTimeUpgrade);
        SetUpgradeTextAndButton(playerStatusData.costLimitUpgrade);
        SetUpgradeTextAndButton(playerStatusData.costGenerationSpeedUpgrade);
        SetUpgradeTextAndButton(playerStatusData.virusUpgrade);
    }

    public void NoButton()
    {
        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
    }

    // 渡された強化要素のアップグレードを行う
    private void TryUpgrade<T>(T upgrade, int money) where T : BaseUpgrade
    {
        if (!upgrade.canUpgrade)
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

        upgrade.SetUpgradeLevel(upgrade.lv + 1);
        SetUpgradeTextAndButton(upgrade);

        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
    }

    // 指定の強化内容のLvと必要なお金の表示切替
    // レベルが最大の時、ボタンが触れられなくなる
    private void SetTextAndButton<T>(T upgrade, TextMeshProUGUI lvText, TextMeshProUGUI moneyText, Button button) where T : BaseUpgrade
    {
        lvText.text = upgrade.canUpgrade ? upgrade.lv.ToString() : "MAX";
        moneyText.text = upgrade.canUpgrade ? $"<size=40><sprite=0><size=45>{upgrade.UpgradeMoney}" : "MAX";

        if (playerStatusData.wallet.CurrentMoney < upgrade.UpgradeMoney || !upgrade.canUpgrade)
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

    // TはBaseUpgradeを継承している必要がある
    private void SetDialogText<T>(T upgrade, string message, string unitName) where T : BaseUpgrade
    {
        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);

        // 基本情報の設定
        DialogLevel_text1.text = upgrade.lv.ToString();
        DialogLevel_text2.text = (upgrade.lv + 1).ToString();
        DialogMoney_text.text = upgrade.UpgradeMoney.ToString();
        DialogMessege.text = message;

        // 詳細テキストの構築
        object currentValue = upgrade.GetLevelofUpgrade(upgrade.lv);
        object nextValue = upgrade.GetLevelofUpgrade(upgrade.lv + 1);

        // 感染(Virus)などの割合表示への対応（必要に応じて）
        if (upgrade is VirusUpgrade)
        {
            float cur = (float)currentValue * 100;
            float nxt = (float)nextValue * 100;
            Detalise_text.text = $"{unitName} : {cur}%\n強化後の{unitName} : {nxt}%";
        }
        else
        {
            Detalise_text.text = $"{unitName} : {currentValue}\n強化後の{unitName} : {nextValue}";
        }
    }

    public void CastleSkillEnhancement()
    {
        upgradeType = UpgradeType.Castle;
        SetDialogText(playerStatusData.castleUpgrade, "城のHPを強化しますか？", "HP");
    }

    public void CannonDamageSkillEnhancement()
    {
        upgradeType = UpgradeType.CannonDamage;
        SetDialogText(playerStatusData.cannonDamageUpgrade, "砲撃の攻撃力を強化しますか？", "ダメージ");
    }

    public void CannonCoolTimeSkillEnhancement()
    {
        upgradeType = UpgradeType.CannonCoolTime;
        SetDialogText(playerStatusData.cannonCoolTimeUpgrade, "クールタイムを強化しますか？", "クールタイム");
    }

    public void CostLimitSkillEnhacement()
    {
        upgradeType = UpgradeType.CostLimit;
        SetDialogText(playerStatusData.costLimitUpgrade, "コストの最大値を強化しますか？", "最大値");
    }

    public void CostGenerationSpeedSkillEnhacement()
    {
        upgradeType = UpgradeType.CostGenerationSpeed;
        SetDialogText(playerStatusData.costGenerationSpeedUpgrade, "コストの生成速度を強化しますか？", "生成速度");
    }

    public void VirusSkillEnhacement()
    {
        upgradeType = UpgradeType.Virus;
        SetDialogText(playerStatusData.virusUpgrade, "感染を強化しますか？", "感染時のHP割合");
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
