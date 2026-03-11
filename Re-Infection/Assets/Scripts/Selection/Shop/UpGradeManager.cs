using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Threading.Tasks;
using System;
public class UpGradeManager : MonoBehaviour
{
    private enum UnitUpGradeType
    {
        Soldier,
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
    public Button SoldierButton;
    public Button TankButton;
    public Button ArcherButton;
    public Button WizardButton;
    public Button JockeyButton;
    public Button HammerButton;
    public Button R_ArcherButton;
    public Button R_WizardButton;

    [Header("剣士のレベルとコスト")]
    public UnitStatsData SoldierStats;
    public TextMeshProUGUI Soldier_text;
    public TextMeshProUGUI SoldierUpGradeMoney_text;
    public TextMeshProUGUI SoldierName_text;
    public Image Soldier_Image;

    [Header("盾兵のレベルとコスト")]
    public UnitStatsData TankStats;
    public TextMeshProUGUI Tank_text;
    public TextMeshProUGUI TankUpGradeMoney_text;
    public TextMeshProUGUI TankName_text;
    public Image Tank_Image;

    [Header("弓使いのレベルとコスト")]
    public UnitStatsData ArcherStats;
    public TextMeshProUGUI Archer_text;
    public TextMeshProUGUI ArcherUpGradeMoney_text;
    public TextMeshProUGUI ArcherName_text;
    public Image Archer_Image;

    [Header("魔法使いのレベルとコスト")]
    public UnitStatsData WizardStats;
    public TextMeshProUGUI Wizard_text;
    public TextMeshProUGUI WizardUpGradeMoney_text;
    public TextMeshProUGUI WizardName_text;
    public Image Wizard_Image;

    [Header("騎馬兵のレベルとコスト")]
    public UnitStatsData JockeyStats;
    public TextMeshProUGUI Jockey_text;
    public TextMeshProUGUI JockeyUpGradeMoney_text;
    public TextMeshProUGUI JockeyName_text;
    public Image Jockey_Image;

    [Header("鈍器使いのレベルとコスト")]
    public UnitStatsData HammerStats;
    public TextMeshProUGUI Hammer_text;
    public TextMeshProUGUI HammerUpGradeMoney_text;
    public TextMeshProUGUI HammerName_text;
    public Image Hammer_Image;

    [Header("大弓使いのレベルとコスト")]
    public UnitStatsData R_ArcherStats;
    public TextMeshProUGUI R_Archer_text;
    public TextMeshProUGUI R_ArcherUpGradeMoney_text;
    public TextMeshProUGUI R_ArcherName_text;
    public Image R_Archer_Image;

    [Header("上級魔法使いのレベルとコスト")]
    public UnitStatsData R_WizardStats;
    public TextMeshProUGUI R_Wizard_text;
    public TextMeshProUGUI R_WizardUpGradeMoney_text;
    public TextMeshProUGUI R_WizardName_text;
    public Image R_Wizard_Image;

    [Header("ダイアログ表示用")]
    public TextMeshProUGUI DialogMassege;
    public TextMeshProUGUI DialogLevel_text1;
    public TextMeshProUGUI DialogLevel_text2;
    public TextMeshProUGUI DialogMoney_text;
    public TextMeshProUGUI Warning_text;
    public TextMeshProUGUI Detalise_text;
    public GameObject WarningObj;

    PlayerStatusData playerStatusData;

    public static event Action OnUnitLevelChanged;

    private void Awake()
    {
        playerStatusData = Resources.Load<PlayerStatusData>("PlayerStatusData");
    }

    private void Start()
    {
        DialogObj.SetActive(false);
        money_text.text = ($"{playerStatusData.wallet.CurrentMoney}");
        LayCastObj.SetActive(false);
        SetRaycastTargets(LayCastObj, false);

        UpdateUnitUI(SoldierStats.unitStats, Soldier_text, SoldierUpGradeMoney_text, SoldierButton,SoldierName_text,Soldier_Image);
        UpdateUnitUI(TankStats.unitStats, Tank_text, TankUpGradeMoney_text, TankButton,TankName_text,Tank_Image);
        UpdateUnitUI(ArcherStats.unitStats, Archer_text, ArcherUpGradeMoney_text, ArcherButton,ArcherName_text,Archer_Image);
        UpdateUnitUI(JockeyStats.unitStats, Jockey_text, JockeyUpGradeMoney_text, JockeyButton,JockeyName_text,Jockey_Image);
        UpdateUnitUI(HammerStats.unitStats, Hammer_text, HammerUpGradeMoney_text, HammerButton,HammerName_text,Hammer_Image);
        UpdateUnitUI(WizardStats.unitStats, Wizard_text, WizardUpGradeMoney_text, WizardButton,WizardName_text,Wizard_Image);
        UpdateUnitUI(R_ArcherStats.unitStats, R_Archer_text, R_ArcherUpGradeMoney_text, R_ArcherButton,R_ArcherName_text,R_Archer_Image);
        UpdateUnitUI(R_WizardStats.unitStats, R_Wizard_text, R_WizardUpGradeMoney_text, R_WizardButton,R_WizardName_text,R_Wizard_Image);

        Warning_text.text = "";
        WarningObj.SetActive(false);
    }

    private void Update()
    {
        UpdateUnitButton(SoldierStats.unitStats, Soldier_text, SoldierUpGradeMoney_text, SoldierButton,SoldierName_text,Soldier_Image);
        UpdateUnitButton(TankStats.unitStats, Tank_text, TankUpGradeMoney_text, TankButton,TankName_text,Tank_Image);
        UpdateUnitButton(ArcherStats.unitStats, Archer_text, ArcherUpGradeMoney_text, ArcherButton,ArcherName_text,Archer_Image);
        UpdateUnitButton(WizardStats.unitStats, Wizard_text, WizardUpGradeMoney_text, WizardButton,WizardName_text,Wizard_Image);
        UpdateUnitButton(JockeyStats.unitStats, Jockey_text, JockeyUpGradeMoney_text, JockeyButton,JockeyName_text,Jockey_Image);
        UpdateUnitButton(HammerStats.unitStats, Hammer_text, HammerUpGradeMoney_text, HammerButton,HammerName_text,Hammer_Image);
        UpdateUnitButton(R_ArcherStats.unitStats, R_Archer_text, R_ArcherUpGradeMoney_text, R_ArcherButton,R_ArcherName_text,R_Archer_Image);
        UpdateUnitButton(R_WizardStats.unitStats, R_Wizard_text, R_WizardUpGradeMoney_text, R_WizardButton,R_WizardName_text,R_Wizard_Image);
    }

    private void UpdateUnitUI(UnitStats stats, TextMeshProUGUI lvText, TextMeshProUGUI costText, Button button,TextMeshProUGUI nameText=null,Image IconImage=null)
    {
        //ロック中の表示
        if (!stats.IsUnitUnlocked())
        {
            lvText.color = new Color(1f, 0.337f, 0.337f);
            lvText.text = "?";
            costText.text = "???";
            // ボタン無効
            button.interactable = false;

            if (IconImage != null)
                IconImage.color = Color.black;

            if (nameText != null)
                nameText.text = "？？？";

            return;
        }

        // MAX のとき
        if (stats.lv >= stats.MaxLevel)
        {
            lvText.text = "MAX";
            costText.text = "MAX";
            lvText.color = new Color(1f, 0.337f, 0.337f);
            costText.color = new Color(1f, 0.337f, 0.337f);
            button.interactable = false;
            return;
        }

        lvText.text = stats.lv.ToString();
        int cost = stats.GetNextLevelCost();
        costText.text = $"<sprite=0>{cost.ToString()}";

        bool canBuy = playerStatusData.wallet.CanBuy(cost);

        costText.color = canBuy ? Color.white : new Color(1f, 0.337f, 0.337f);
        button.interactable = canBuy;

        if (IconImage != null)
        {
            IconImage.color = Color.white;
        }

        if (nameText != null)
        {
            nameText.text = stats.unitName;
        }
    }

    // アップグレードを承認するボタンの関数
    public void UndoUpgrade()
    {
        switch (unitUpGradeType)
        {
            case UnitUpGradeType.Soldier:
                TryUpgradeUnit(
                    SoldierStats.unitStats,
                    Soldier_text,
                    SoldierUpGradeMoney_text,
                    SoldierButton
                    );
                break;

            case UnitUpGradeType.Tank:
                TryUpgradeUnit(
                    TankStats.unitStats,
                    Tank_text,
                    TankUpGradeMoney_text,
                    TankButton
                    );
                break;

            case UnitUpGradeType.Archer:
                TryUpgradeUnit(
                    ArcherStats.unitStats,
                    Archer_text,
                    ArcherUpGradeMoney_text,
                    ArcherButton
                    );
                break;

            case UnitUpGradeType.Wizard:
                TryUpgradeUnit(
                    WizardStats.unitStats,
                    Wizard_text,
                    WizardUpGradeMoney_text,
                    WizardButton
                    );
                break;

            case UnitUpGradeType.Jockey:
                TryUpgradeUnit(
                    JockeyStats.unitStats,
                    Jockey_text,
                    JockeyUpGradeMoney_text,
                    JockeyButton
                    );
                break;

            case UnitUpGradeType.Hammer:
                TryUpgradeUnit(
                    HammerStats.unitStats,
                    Hammer_text,
                    HammerUpGradeMoney_text,
                    HammerButton
                    );
                break;

            case UnitUpGradeType.R_Archer:
                TryUpgradeUnit(
                    R_ArcherStats.unitStats,
                    R_Archer_text,
                    R_ArcherUpGradeMoney_text,
                    R_ArcherButton
                    );
                break;

            case UnitUpGradeType.R_Wizard:
                TryUpgradeUnit(
                    R_WizardStats.unitStats,
                    R_Wizard_text,
                    R_WizardUpGradeMoney_text,
                    R_WizardButton
                    );
                break;
        }
    }

    public void NoButton()
    {
        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
        SetRaycastTargets(LayCastObj, false);
    }

    // 渡された強化要素のアップグレードを行う
    private void TryUpgradeUnit(UnitStats stats, TextMeshProUGUI lvText, TextMeshProUGUI costText, Button button)
    {
        int cost = stats.GetNextLevelCost();

         if (stats.lv >= stats.MaxLevel)
        {
            StartCoroutine(WarningLevelText());
            return;
        }

        if (!playerStatusData.wallet.CanBuy(cost))
        {
            StartCoroutine(WarningMoneyText());
            return;
        }

        SEManager.Instance.PlaySE(SEManager.SEType.Upgrade);

        playerStatusData.wallet.RemoveMoney(cost);
     
        money_text.text = playerStatusData.wallet.CurrentMoney.ToString();
        stats.LevelUP();

        UpdateUnitUI(stats, lvText, costText, button);

        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
        SetRaycastTargets(LayCastObj, false);

        OnUnitLevelChanged?.Invoke();
    }

    // TはBaseUpgradeを継承している必要がある
    private void SetDialogTextUnit(UnitStats stats, string message)
    {
        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);
        SetRaycastTargets(LayCastObj, true);

        DialogLevel_text1.text = stats.lv.ToString();
        DialogLevel_text2.text = (stats.lv + 1).ToString();
        int nextCost = stats.GetNextLevelCost();
        DialogMoney_text.text = nextCost.ToString();
        DialogMassege.text = message;

        Detalise_text.text = $"HP：{stats.GetCurrentLevelMaxHp()}　→　HP：{stats.GetLevelofMaxHp(stats.lv + 1)}\n" +
                             $"ATK：{stats.GetCurrentLevelAtk()}　→　ATK：{stats.GetLevelofAtk(stats.lv + 1)}";
    }

    public void SoliderEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.Soldier;
        SetDialogTextUnit(SoldierStats.unitStats, "剣士を強化しますか？");
    }

    public void TankEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.Tank;
        SetDialogTextUnit(TankStats.unitStats, "盾兵を強化しますか？");
    }

    public void ArcherEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.Archer;
        SetDialogTextUnit(ArcherStats.unitStats, "弓使いを強化しますか？");
    }

    public void WizardEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.Wizard;
        SetDialogTextUnit(WizardStats.unitStats, "魔法使いを強化しますか？");
    }

    public void JockeyEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.Jockey;
        SetDialogTextUnit(JockeyStats.unitStats, "騎馬兵を強化しますか？");
    }

    public void HammerEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.Hammer;
        SetDialogTextUnit(HammerStats.unitStats, "鈍器使いを強化しますか？");
    }

    public void R_ArcherEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.R_Archer;
        SetDialogTextUnit(R_ArcherStats.unitStats, "大弓使いを強化しますか？");
    }
    public void R_WizardEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.R_Wizard;
        SetDialogTextUnit(R_WizardStats.unitStats, "上級魔法使いを強化しますか？");
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

    private void SetRaycastTargets(GameObject obj, bool enabled)
    {
        var graphics = obj.GetComponentsInChildren<UnityEngine.UI.Graphic>(true);

        foreach (var g in graphics)
        {
            g.raycastTarget = enabled;
        }
    }

    private void UpdateUnitButton(UnitStats stats, TextMeshProUGUI lvText, TextMeshProUGUI costText, Button button, TextMeshProUGUI nameText = null, Image iconImage = null)
    {
        if (!stats.IsUnitUnlocked())
        {
            lvText.text = "?";
            costText.text = "???";

            button.interactable = false;

            if (iconImage != null)
            {
                iconImage.color = Color.black;
            }

            if (nameText != null)
            {
                nameText.text = "？？？";
            }

            return;

        }

        int cost = stats.GetNextLevelCost();
        bool canBuy = playerStatusData.wallet.CanBuy(cost);

        lvText.text = stats.lv.ToString();
        costText.text = $"<sprite=0>{cost.ToString()}";

        costText.color = canBuy ? Color.white : new Color(1f, 0.337f, 0.337f);

        // ボタンの状態を更新
        button.interactable = canBuy;

        costText.color = canBuy ? Color.white : new Color(1f, 0.337f, 0.337f);

        if (stats.lv >= stats.MaxLevel)
        {
            lvText.text = "MAX";
            costText.text = "MAX";
            lvText.color = new Color(1f, 0.337f, 0.337f);
            costText.color = new Color(1f, 0.337f, 0.337f);
            button.interactable = false;
            return;
        }

        if (iconImage != null)
            iconImage.color = Color.white;

        if (nameText != null)
            nameText.text = stats.unitName;

        // UI の表示も更新
        lvText.text = $"{stats.lv}";
        costText.text = $"<sprite=0>{cost.ToString()}";
    }

}
