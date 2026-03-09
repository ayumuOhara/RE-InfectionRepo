using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Threading.Tasks;
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

    [Header("盾兵のレベルとコスト")]
    public UnitStatsData TankStats;
    public TextMeshProUGUI Tank_text;
    public TextMeshProUGUI TankUpGradeMoney_text;

    [Header("弓使いのレベルとコスト")]
    public UnitStatsData ArcherStats;
    public TextMeshProUGUI Archer_text;
    public TextMeshProUGUI ArcherUpGradeMoney_text;

    [Header("魔法使いのレベルとコスト")]
    public UnitStatsData WizardStats;
    public TextMeshProUGUI Wizard_text;
    public TextMeshProUGUI WizardUpGradeMoney_text;

    [Header("騎馬兵のレベルとコスト")]
    public UnitStatsData JockeyStats;
    public TextMeshProUGUI Jockey_text;
    public TextMeshProUGUI JockeyUpGradeMoney_text;

    [Header("鈍器使いのレベルとコスト")]
    public UnitStatsData HammerStats;
    public TextMeshProUGUI Hammer_text;
    public TextMeshProUGUI HammerUpGradeMoney_text;

    [Header("大弓使いのレベルとコスト")]
    public UnitStatsData R_ArcherStats;
    public TextMeshProUGUI R_Archer_text;
    public TextMeshProUGUI R_ArcherUpGradeMoney_text;

    [Header("上級魔法使いのレベルとコスト")]
    public UnitStatsData R_WizardStats;
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

    [SerializeField]
    public UnitDetailUII unitDetailUII;
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

        UpdateUnitUI(SoldierStats.unitStats, Soldier_text, SoldierUpGradeMoney_text, SoldierButton);
        UpdateUnitUI(TankStats.unitStats, Tank_text, TankUpGradeMoney_text, TankButton);
        UpdateUnitUI(ArcherStats.unitStats, Archer_text, ArcherUpGradeMoney_text, ArcherButton);
        UpdateUnitUI(JockeyStats.unitStats, Jockey_text, JockeyUpGradeMoney_text, JockeyButton);
        UpdateUnitUI(HammerStats.unitStats, Hammer_text, HammerUpGradeMoney_text, HammerButton);
        UpdateUnitUI(WizardStats.unitStats, Wizard_text, WizardUpGradeMoney_text, WizardButton);
        UpdateUnitUI(R_ArcherStats.unitStats, R_Archer_text, R_ArcherUpGradeMoney_text, R_ArcherButton);
        UpdateUnitUI(R_WizardStats.unitStats, R_Wizard_text, R_WizardUpGradeMoney_text, R_WizardButton);

        Warning_text.text = "";
        WarningObj.SetActive(false);

        SoldierStats.unitStats.SetLevel(1);
        TankStats.unitStats.SetLevel(1);
        ArcherStats.unitStats.SetLevel(1);
        WizardStats.unitStats.SetLevel(1);
        JockeyStats.unitStats.SetLevel(1);
        HammerStats.unitStats.SetLevel(1);
        R_ArcherStats.unitStats.SetLevel(1);
        R_WizardStats.unitStats.SetLevel(1);

        
    }
    private void UpdateUnitUI(UnitStats stats, TextMeshProUGUI lvText, TextMeshProUGUI costText, Button button)
    {
        // レベルテキストは常に赤
        lvText.color = new Color(1f, 0.337f, 0.337f);

        // MAX のとき
        if (stats.lv >= stats.MaxLevel)
        {
            lvText.text = "MAX";
            costText.text = "MAX";
            costText.color = new Color(1f, 0.337f, 0.337f);
            button.interactable = false;
            return;
        }

        lvText.text = stats.lv.ToString();

        int nextCost = GetTrueNextCost(stats);
        costText.text = nextCost.ToString();

        bool canBuy = playerStatusData.wallet.CurrentMoney >= nextCost;

        costText.color = canBuy ? Color.white : new Color(1f, 0.337f, 0.337f);
        button.interactable = canBuy;
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

        int cost = GetTrueNextCost(stats);

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

        playerStatusData.wallet.RemoveMoney(cost);
     
        money_text.text = playerStatusData.wallet.CurrentMoney.ToString();
        stats.LevelUP();

        UpdateUnitUI(stats, lvText, costText, button);

        unitDetailUII.SetUnit(stats);

        DialogObj.SetActive(false);
        LayCastObj.SetActive(false);
        SetRaycastTargets(LayCastObj, false);
    }

    // TはBaseUpgradeを継承している必要がある
    private void SetDialogTextUnit(UnitStats stats, string message)
    {
        DialogObj.SetActive(true);
        LayCastObj.SetActive(true);
        SetRaycastTargets(LayCastObj, true);


        DialogLevel_text1.text = stats.lv.ToString();
        DialogLevel_text2.text = (stats.lv + 1).ToString();
        int nextCost = GetTrueNextCost(stats);
        DialogMoney_text.text = nextCost.ToString();
        DialogMassege.text = message;

        Detalise_text.text = $"HP：{stats.GetCurrentLevelMaxHp()}　→　HP：{stats.GetLevelofMaxHp(stats.lv + 1)}\n" +
                             $"ATK：{stats.GetCurrentLevelAtk()}　→　ATK：{stats.GetLevelofAtk(stats.lv + 1)}";
    }

    public void SoliderEnhancement()
    {
        unitUpGradeType = UnitUpGradeType.Soldier;
        SetDialogTextUnit(SoldierStats.unitStats, "剣士を強化しますか？");
        int cost = GetTrueNextCost(SoldierStats.unitStats);
       

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
    private int GetTrueNextCost(UnitStats stats)
    {
        int nextIndex = stats.LvIdx + 1;

        if (nextIndex >= stats.statusScaler.Length)
            return 0;

        return (int)stats.statusScaler[nextIndex].LevelUpCost;
    }

    private void SetRaycastTargets(GameObject obj, bool enabled)
    {
        var graphics = obj.GetComponentsInChildren<UnityEngine.UI.Graphic>(true);

        foreach (var g in graphics)
        {
            g.raycastTarget = enabled;
        }
    }
}
