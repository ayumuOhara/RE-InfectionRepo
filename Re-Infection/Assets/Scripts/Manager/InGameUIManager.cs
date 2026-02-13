using System.Collections;
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

    [SerializeField] TextMeshProUGUI clearTimeText;

    [SerializeField] TextMeshProUGUI waveCoinText;
    [SerializeField] TextMeshProUGUI stageCoinText;
    [SerializeField] GameObject stageClearReward;
    [SerializeField] TextMeshProUGUI firstCoinText;
    [SerializeField] GameObject firstClearReward;
    [SerializeField] TextMeshProUGUI totalCoinText;
    [SerializeField] TextMeshProUGUI currentCoinText;

    private SEManager seManager;

    string coinIconText = "<sprite=0>";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        seManager = FindObjectOfType<SEManager>();

        resultUI.enabled = false;
        clearUI.enabled = false;
        failedUI.enabled = false;
        retireUI.enabled = false;
    }

    // 全UI表示
    public void VisibleAllUI()
    {
        masterUI.gameObject.SetActive(true);
    }

    // 全UI非表示
    public void InvisibleAllUI()
    {
        masterUI.gameObject.SetActive(false);
    }

    // ステージクリア処理
    public IEnumerator SessionClear()
    {
        tutorialUI.enabled = false;
        timeUI.enabled = false;
        combatUI.enabled = false;

        clearTimeText.text = gameManager.timeManager.Minutes.ToString("D2") + ":" + gameManager.timeManager.Seconds.ToString("D2");

        var audio = FindObjectOfType<BGMManager>();
        audio.StopBGM();

        resultUI.enabled = true;
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

        resultUI.enabled = true;
        clearUI.enabled = false;
        failedUI.enabled = true;

        seManager.PlaySE(SEManager.SEType.StageFailed);

        yield return new WaitForSeconds(2.5f);

        audio.PlayBGM(BGMManager.BGMType.Result);
    }

    // 報酬処理
    public void SessionReward()
    {
        rewardUI.enabled = true;

        var totalCoin = 0;
        var waveCoin = gameManager.waveSpawner.CurrentStage.waveClearCoin * gameManager.waveSpawner.currentWaveIdx;
        var stageCoin = gameManager.waveSpawner.CurrentStage.stageClearCoin;

        totalCoin = gameManager.waveSpawner.IsSessionClear ? waveCoin + stageCoin : waveCoin;

        if (gameManager.waveSpawner.IsSessionClear)
        {
            stageClearReward.SetActive(true);

            if (!gameManager.waveSpawner.CurrentStage.isClear)
            {
                totalCoin += gameManager.waveSpawner.CurrentStage.firstClearCoin;

                gameManager.waveSpawner.CurrentStage.isClear = true;
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

        GetCoinText(waveCoinText, waveCoin);
        GetCoinText(stageCoinText, stageCoin);
        GetCoinText(firstCoinText, waveCoin + stageCoin);

        GetCoinText(totalCoinText, totalCoin);

        Wallet wallet = Resources.Load<PlayerStatusData>("PlayerStatusData").wallet;
        wallet.AddMoney(totalCoin);

        currentCoinText.text = $"{wallet.CurrentMoney}";

        rewardUI.transform.Find("Rewards").GetComponent<Animator>().SetTrigger("Reward");
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
        Debug.Log("Coin" + value.ToString());
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
