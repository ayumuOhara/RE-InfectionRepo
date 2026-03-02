using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class UpGradeManager : MonoBehaviour
{
    private enum UnitUpGradeType
    {
        Solider,
        Tank,
        Archer,
        Wizard,
        Jockey,
        Hammer,
        R_Archer,
        R_Wizard,
    }

    private UnitUpGradeType unitUpGradeType;

    [Header("UI")]
    public GameObject DialogObj;
    public TextMeshProUGUI money_text;
    public GameObject LayCastObj;

    //Unit強化ボタン
    [Header("Buttons")]
    public Button SoliderButton;
    public Button TankButton;
    public Button ArcherButton;
    public Button WizardButton;
    public Button JockeyButton;
    public Button HammerButton;
    public Button R_ArcherButton;
    public Button R_WizardButton;

    [Header("剣士のレベルとコスト")]
    public TextMeshProUGUI Solider_text;
    public TextMeshProUGUI SoliderUpGradeMoney_text;

    [Header("盾兵のレベルとコスト")]
    public TextMeshProUGUI Tank_text;
    public TextMeshProUGUI TankUpGradeMoney_text;

    [Header("弓使いのレベルとコスト")]
    public TextMeshProUGUI Archer_text;
    public TextMeshProUGUI ArcherUpGradeMoney_text;

    [Header("魔法使いのレベルとコスト")]
    public TextMeshProUGUI Wizard_text;
    public TextMeshProUGUI WizardUpGradeMoney_text;

    [Header("騎馬兵のレベルとコスト")]
    public TextMeshProUGUI Jockey_text;
    public TextMeshProUGUI JokeyUpGradeMoney_text;

    [Header("鈍器使いのレベルとコスト")]
    public TextMeshProUGUI Hammer_text;
    public TextMeshProUGUI HammerUpGradeMoney_text;

    [Header("大弓使いのレベルとコスト")]
    public TextMeshProUGUI R_Archer_text;
    public TextMeshProUGUI R_ArcherUpGradeMoney_text;

    [Header("上級魔法使いのレベルとコスト")]
    public TextMeshProUGUI R_Wizard_text;
    public TextMeshProUGUI R_WizardUpGradeMoney_text;

    [Header("ダイアログ表示用")]
    public TextMeshProUGUI DialogMassege;
    public TextMeshProUGUI DialogLevel_text1;
    public TextMeshProUGUI DialogLevel_text2;
    public TextMeshProUGUI DialogMoney_text;
    public TextMeshProUGUI Warning_text;
    public TextMeshProUGUI Detalise_text;
    public GameObject WarningObj;

    PlayerStatusData playerStatusData;

    private void Awake()
    {
        playerStatusData = Resources.Load<PlayerStatusData>("PlayerStatusData");

        //playerStatusData.castleUpgrade.SetUpgradeLevel(playerStatusData.castleUpgrade.lv);
        //playerStatusData.cannonDamageUpgrade.SetUpgradeLevel(playerStatusData.cannonDamageUpgrade.lv);
        //playerStatusData.cannonCoolTimeUpgrade.SetUpgradeLevel(playerStatusData.cannonCoolTimeUpgrade.lv);
        //playerStatusData.costLimitUpgrade.SetUpgradeLevel(playerStatusData.costLimitUpgrade.lv);
        //playerStatusData.costGenerationSpeedUpgrade.SetUpgradeLevel(playerStatusData.costGenerationSpeedUpgrade.lv);
        //playerStatusData.virusUpgrade.SetUpgradeLevel(playerStatusData.virusUpgrade.lv);
    }

    private void Start()
    {
        DialogObj.SetActive(false);
        money_text.text = ($"{playerStatusData.wallet.CurrentMoney}");
        LayCastObj.SetActive(false);

        //SetUpgradeTextAndButton(playerStatusData.castleUpgrade);
        //SetUpgradeTextAndButton(playerStatusData.cannonDamageUpgrade);
        //SetUpgradeTextAndButton(playerStatusData.cannonCoolTimeUpgrade);
        //SetUpgradeTextAndButton(playerStatusData.costLimitUpgrade);
        //SetUpgradeTextAndButton(playerStatusData.costGenerationSpeedUpgrade);
        //SetUpgradeTextAndButton(playerStatusData.virusUpgrade);

        Warning_text.text = "";
        WarningObj.SetActive(false);
    }

    // 渡された強化内容によってUI表示を操作
    private void SetUpgradeTextAndButton<T>(T upgrade) where T : BaseUpgrade
    {
        switch (upgrade.GetType().ToString())
        {
            //case "CastleUpgrade":
            //    SetTextAndButton(playerStatusData.castleUpgrade, CastleLv_text, CastleUpgradeMoney_text, CastleButton);
            //    break;
            //case "CannonDamageUpgrade":
            //    SetTextAndButton(playerStatusData.cannonDamageUpgrade, CannonDamageLv_text, CannonDamageUpgradeMoney_text, CannonDamageButton);
            //    break;
            //case "CannonCoolTimeUpgrade":
            //    SetTextAndButton(playerStatusData.cannonCoolTimeUpgrade, CannonCoolTimeLv_text, CannonCoolTimeUpgradeMoney_text, CannonCoolTimeButton);
            //    break;
            //case "CostLimitUpgrade":
            //    SetTextAndButton(playerStatusData.costLimitUpgrade, CostLimitLv_text, CostLimitUpgradeMoney_text, CostLimitButton);
            //    break;
            //case "CostGenerationSpeedUpgrade":
            //    SetTextAndButton(playerStatusData.costGenerationSpeedUpgrade, CostGenerationSpeedLv_text, CostGenerationSpeedUpgradeMoney_text, CostGenerationSpeedButton);
            //    break;
            //case "VirusUpgrade":
            //    SetTextAndButton(playerStatusData.virusUpgrade, VirusLv_text, VirusUpgradeMoney_text, VirusButton);
            //    break;
            //default:
            //    break;
        }
    }

    // アップグレードを承認するボタンの関数
    public void UndoUpgrade()
    {
        //switch (upgradeType)
        //{
        //    case UpgradeType.Castle:
        //        TryUpgrade(playerStatusData.castleUpgrade, playerStatusData.castleUpgrade.UpgradeMoney);
        //        break;
        //    case UpgradeType.CannonDamage:
        //        TryUpgrade(playerStatusData.cannonDamageUpgrade, playerStatusData.cannonDamageUpgrade.UpgradeMoney);
        //        break;
        //    case UpgradeType.CannonCoolTime:
        //        TryUpgrade(playerStatusData.cannonCoolTimeUpgrade, playerStatusData.cannonCoolTimeUpgrade.UpgradeMoney);
        //        break;
        //    case UpgradeType.CostLimit:
        //        TryUpgrade(playerStatusData.costLimitUpgrade, playerStatusData.costLimitUpgrade.UpgradeMoney);
        //        break;
        //    case UpgradeType.CostGenerationSpeed:
        //        TryUpgrade(playerStatusData.costGenerationSpeedUpgrade, playerStatusData.costGenerationSpeedUpgrade.UpgradeMoney);
        //        break;
        //    case UpgradeType.Virus:
        //        TryUpgrade(playerStatusData.virusUpgrade, playerStatusData.virusUpgrade.UpgradeMoney);
        //        break;
        //    default:
        //        break;
        //}

        //SetUpgradeTextAndButton(playerStatusData.castleUpgrade);
        //SetUpgradeTextAndButton(playerStatusData.cannonDamageUpgrade);
        //SetUpgradeTextAndButton(playerStatusData.cannonCoolTimeUpgrade);
        //SetUpgradeTextAndButton(playerStatusData.costLimitUpgrade);
        //SetUpgradeTextAndButton(playerStatusData.costGenerationSpeedUpgrade);
        //SetUpgradeTextAndButton(playerStatusData.virusUpgrade);
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
        DialogMassege.text = message;

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

    public void SoliderEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.Solider;
        //SetDialogText(playerStatusData.castleUpgrade, "剣士を強化しますか？", "HP");
    }

    public void TankEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.Tank;
        //SetDialogText(playerStatusData.castleUpgrade, "盾兵を強化しますか？", "HP");
    }
    public void ArcherEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.Archer;
        //SetDialogText(playerStatusData.castleUpgrade, "弓使いを強化しますか？", "HP");
    }
    public void WizardEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.Wizard;
        //SetDialogText(playerStatusData.castleUpgrade, "魔法使いを強化しますか？", "HP");
    }
    public void JockeyEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.Jockey;
        //SetDialogText(playerStatusData.castleUpgrade, "騎馬兵を強化しますか？", "HP");
    }
    public void HammerEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.Hammer;
        //SetDialogText(playerStatusData.castleUpgrade, "鈍器使いを強化しますか？", "HP");
    }
    public void R_ArcherEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.R_Archer;
        //SetDialogText(playerStatusData.castleUpgrade, "大弓使いを強化しますか？", "HP");
    }
    public void R_WizardEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.R_Wizard;
        //SetDialogText(playerStatusData.castleUpgrade, "上級魔法使いを強化しますか？", "HP");
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
