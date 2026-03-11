using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] Canvas transitionUIprefab;

    [SerializeField] Canvas masterUI;
    [SerializeField] Canvas combatUI;
    [SerializeField] Canvas timeUI;
    [SerializeField] Canvas resultUI;
    [SerializeField] Canvas unlockUI;
    [SerializeField] Canvas clearUI;
    [SerializeField] Canvas failedUI;
    [SerializeField] Canvas rewardUI;
    [SerializeField] Canvas retireUI;
    [SerializeField] Canvas returnHomeUI;
    [SerializeField] Canvas tutorialUI;

    [SerializeField] TextMeshProUGUI currentWaveText;
    [SerializeField] TextMeshProUGUI currentEnemyCntText;
    [SerializeField] Slider currentWaveProgress;

    [SerializeField] TextMeshProUGUI bossNameText;
    [SerializeField] TextMeshProUGUI bossHealthText;

    [SerializeField] Image costIcon;

    [SerializeField] Image holdTextLabel;
    [SerializeField] Image holdProgressIcon;
    [SerializeField] Image holdProgressGauge;

    [SerializeField] Animator unlockBackgroundAnimator;
    [SerializeField] Animator unlockUnitAnimator;
    [SerializeField] TextMeshProUGUI unlockTitleText;
    [SerializeField] TextMeshProUGUI tapCloseText;

    [SerializeField] TextMeshProUGUI clearTimeText;

    [SerializeField] TextMeshProUGUI waveCoinText;
    [SerializeField] TextMeshProUGUI stageCoinText;
    [SerializeField] GameObject stageClearReward;
    [SerializeField] TextMeshProUGUI firstCoinText;
    [SerializeField] GameObject firstClearReward;
    [SerializeField] TextMeshProUGUI currentCoinText;
    [SerializeField] GameObject currentCoinLabel;
    [SerializeField] float coinTextElapsedTime;

    [SerializeField] Button returnToHomeButton;
    [SerializeField] Button replayButton;

    private SEManager seManager;

    string coinIconText = "<sprite=0>";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        seManager = FindObjectOfType<SEManager>();

        resultUI.enabled = false;
        tutorialUI.enabled = false;
        clearUI.enabled = false;
        failedUI.enabled = false;
        retireUI.enabled = false;
        unlockUI.enabled = false;
    }

    // 全UI表示
    public void VisibleAllUI()
    {
        masterUI.gameObject.SetActive(true);
    }

    // 全UI非表示
    public void InvisibleAllUI()
    {
        combatUI.enabled = false;
        timeUI.enabled = false;
        tutorialUI.enabled = false;
        masterUI.gameObject.SetActive(false);
    }

    // ステージクリア処理
    public IEnumerator SessionClear()
    {
        tutorialUI.enabled = false;
        timeUI.enabled = false;
        combatUI.enabled = false;

        clearTimeText.text = gameManager.timeManager.GetSessionClearTimeText();

        var audio = FindObjectOfType<BGMManager>();
        audio.StopBGM();

        clearUI.enabled = true;
        failedUI.enabled = false;

        seManager.PlaySE(SEManager.SEType.StageClear);

        yield return new WaitForSeconds(2.5f);

        audio.PlayBGM(BGMManager.BGMType.Result);
    }

    // ステージ失敗処理
    public IEnumerator SessionFailed()
    {
        tutorialUI.enabled = false;
        timeUI.enabled = false;
        combatUI.enabled = false;

        var audio = FindObjectOfType<BGMManager>();
        audio.StopBGM();

        clearUI.enabled = false;
        failedUI.enabled = true;

        seManager.PlaySE(SEManager.SEType.StageFailed);

        yield return new WaitForSeconds(2.5f);

        audio.PlayBGM(BGMManager.BGMType.Result);
    }

    // 報酬処理
    public IEnumerator SessionReward()
    {
        resultUI.enabled = true;
        rewardUI.enabled = true;

        currentCoinLabel.SetActive(false);

        var totalCoin = 0;
        var waveCoin = gameManager.waveSpawner.CurrentStage.waveClearCoin * gameManager.waveSpawner.currentWaveIdx;
        var stageCoin = gameManager.waveSpawner.CurrentStage.stageClearCoin;

        totalCoin = gameManager.waveSpawner.IsSessionClear ? waveCoin + stageCoin : waveCoin;

        bool unitUnlock = false;    // ユニットのアンロック処理を行うか

        if (gameManager.waveSpawner.IsSessionClear)
        {
            stageClearReward.SetActive(true);

            if (!gameManager.waveSpawner.CurrentStage.isClear)
            {
                // 初回クリアの場合、アンロック処理のフラグを真
                unitUnlock = true;

                // アンロック処理を行う場合、ホームへ戻るボタンともう一度プレイするボタンを非表示にする
                if (unitUnlock)
                {
                    returnToHomeButton.gameObject.SetActive(false);
                    replayButton.gameObject.SetActive(false);
                }

                // クリア済みフラグを真 & ステージの進行度を進める
                gameManager.waveSpawner.CurrentStage.SetIsClear(true);
                gameManager.waveSpawner.stageData.SetStageProgress(gameManager.waveSpawner.CurrentStage.stageNum + 1);

                totalCoin += gameManager.waveSpawner.CurrentStage.firstClearCoin;

                firstClearReward.SetActive(true);
            }
            else
            {
                firstClearReward.SetActive(false);
            }
        }
        else
        {
            stageClearReward.SetActive(false);
            firstClearReward.SetActive(false);
        }

        // ウェーブ、ステージクリア、初回クリアで取得したコインをテキストに表示
        GetCoinText(waveCoinText, waveCoin);
        GetCoinText(stageCoinText, stageCoin);
        GetCoinText(firstCoinText, gameManager.waveSpawner.CurrentStage.firstClearCoin);

        // 合計コインを所持金に追加
        Wallet wallet = Resources.Load<PlayerStatusData>("PlayerStatusData").wallet;
        float currentCoin = wallet.CurrentMoney;
        wallet.AddMoney(totalCoin);

        // 報酬アニメーション再生
        rewardUI.transform.Find("Rewards").GetComponent<Animator>().SetTrigger("Reward");

        yield return new WaitForSeconds(3);

        // 所持金のカウントアップ表示
        currentCoinLabel.SetActive(true);
        float time = 0;
        while (time < coinTextElapsedTime)
        {
            time += Time.deltaTime;
            float t = time / coinTextElapsedTime;

            float coin = Mathf.Lerp(currentCoin, wallet.CurrentMoney, t);

            currentCoinText.text = $"{(int)coin}";

            yield return null;
        }

        // ユニットがアンロックされる場合、それを表示するアニメーションを再生
        if(unitUnlock)
            yield return VisibleUnlockUnits(gameManager.waveSpawner.CurrentStage);

        // アンロック処理終了後、ボタンを再び表示
        returnToHomeButton.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(true);
    }

    // アンロックしたユニット表示
    public IEnumerator VisibleUnlockUnits(Stage stage)
    {
        // アンロックするユニットがいない場合、コルーチンを停止
        if(stage.unlockUnits.Length <= 0 || stage.unlockUnits == null)
            yield break;

        unlockUI.enabled = true;

        // アンロックされるユニット全てのアンロック処理
        foreach (var unit in stage.unlockUnits.ToArray())
        {
            unlockTitleText.enabled = false;
            tapCloseText.enabled = false;

            unit.unitStats.UnitUnLock();

            string key = GetAnimationKey(unit.unitStats);
            Debug.Log(key);

            if (key != string.Empty)
                unlockUnitAnimator.SetBool(key, true);
            else
                continue;

            unlockBackgroundAnimator.SetTrigger("Unlock");

            yield return new WaitForSeconds(0.5f);

            seManager.PlaySE(SEManager.SEType.UnlockUnit);

            yield return new WaitForSeconds(1f);

            unlockTitleText.enabled = true;
            tapCloseText.enabled = true;

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            unlockUnitAnimator.SetBool(key, false);
        }

        unlockUI.enabled = false;
    }

    // アンロック時に再生するユニットのアニメーションのキーを取得
    private string GetAnimationKey(UnitStats stats)
    {
        return stats.unitName switch
        {
            "弓使い" => "Archer",
            "盾兵" => "Tank",
            "騎馬兵" => "Jockey",
            "鈍器使い" => "Warrior",
            "魔法使い" => "Clergyman",
            "上魔法使い" => "Witch",
            "大弓使い" => "Bow",
            _ => string.Empty,
        };
    }

    // 敵の合計数テキスト
    public void WaveEnemyCntText(int value)
    {
        currentEnemyCntText.text = $"残り {value} 体";
    }

    // 敵の合計数テキスト表示
    public void VisibleWaveEnemyCntText()
    {
        currentEnemyCntText.enabled = true;
    }

    // 敵の合計数テキスト非表示
    public void InvisibleWaveEnemyCntText()
    {
        currentEnemyCntText.enabled = false;
    }

    // 現在のウェーブテキスト
    public void CurrentWaveText(int value)
    {
        currentWaveText.text = "WAVE " + (value + 1);
    }

    // 現在のウェーブテキスト表示
    public void VisibleCurrentWaveText()
    {
        currentWaveText.enabled = true;
    }

    // 現在のウェーブテキスト非表示
    public void InvisibleCurrentWaveText()
    {
        currentWaveText.enabled = false;
    }

    // 現在のウェーブの進行度
    public void CurrentWaveProgress(int value, int max)
    {
        currentWaveProgress.value = (float)value / max;
    }

    // ホールドアイコンをタップ位置に表示
    public void VisibleHoldIcon()
    {
        if (holdProgressIcon.gameObject.activeSelf != false) return;

        holdProgressIcon.gameObject.SetActive(true);
        holdProgressIcon.rectTransform.position = Input.mousePosition;
    }

    // ホールドアイコンを非表示
    public void InvisibleHoldIcon()
    {
        if (holdProgressIcon.gameObject.activeSelf != true) return;

        holdProgressIcon.gameObject.SetActive(false);
    }

    // ホールドUIを開くアニメーション再生
    public void OpenHoldLabel()
    {
        holdTextLabel.GetComponent<Animator>().SetTrigger("Open");
    }

    // ホールドUIを閉じるアニメーション再生
    public void CloseHoldLabel()
    {
        holdTextLabel.GetComponent<Animator>().SetTrigger("Close");
    }

    // ホールドの進行度
    public void HoldProgressGauge(float value)
    {
        holdProgressGauge.fillAmount = value;
    }

    // コスト生成の進行度
    public void CostGenerateGauge(float value)
    {
        costIcon.fillAmount = value;
    }

    // ボスの名前表示
    public void VisibleBossNameText()
    {
        bossNameText.enabled = true;
    }

    // ボスの名前非表示
    public void InvisibleBossNameText()
    {
        bossNameText.enabled = false;
    }

    // ボスの名前表記
    public void BossNameText(string name)
    {
        bossNameText.text = name;
    }

    // ボスHP表示
    public void VisibleBossHealth()
    {
        bossHealthText.enabled = true;
    }

    // ボスHP非表示
    public void InvisibleBossHealth()
    {
        bossHealthText.enabled = false;
    }

    // ボスHP変動表記
    public void BossHealthText(int value)
    {
        bossHealthText.text = "HP " + value.ToString();
    }

    // ボスHPバー表記
    public void BossHealthProgress(float progress)
    {
        currentWaveProgress.value = progress;
    }

    // シーン遷移確認ボタン(確認)
    public void OnVerified(Canvas ui)
    {
        seManager.PlaySE(SEManager.SEType.Button_Click);
        ui.enabled = true;

        if (!gameManager.timeManager.isPause)
            gameManager.timeManager.GamePause();
    }

    // シーン遷移キャンセル
    public void OnCanceled(Canvas ui)
    {
        seManager.PlaySE(SEManager.SEType.Button_Click);
        ui.enabled = false;

        if (gameManager.timeManager.isPause)
            gameManager.timeManager.GamePause();
    }

    // 取得コイン表示
    public void GetCoinText(TextMeshProUGUI text,int value)
    {
        text.text = $"{coinIconText}{value}";
    }

    // シーンロード
    public void OnLoadScene(string name)
    {
        if (gameManager.timeManager.isPause)
            gameManager.timeManager.GamePause();

        seManager.PlaySE(SEManager.SEType.Lord);
        SceneTransitionner transitonner = Instantiate(transitionUIprefab).GetComponent<SceneTransitionner>();
        transitonner.OnLoadScene(name);
    }
}
