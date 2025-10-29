using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] Canvas resultUI;
    [SerializeField] Canvas clearUI;
    [SerializeField] Canvas failedUI;
    [SerializeField] Canvas retireUI;

    [SerializeField] Canvas nextWaveUI;

    [SerializeField] TextMeshProUGUI currentWaveText;
    [SerializeField] TextMeshProUGUI rewardCostText;
    [SerializeField] TextMeshProUGUI currentEnemyCntText;

    [SerializeField] TextMeshProUGUI cntDownText;
    [SerializeField] Image cntDownGauge;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        resultUI.enabled = false;
        clearUI.enabled = false;
        failedUI.enabled = false;
        nextWaveUI.enabled = false;
        retireUI.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.waveSpawner.isStageCompleted)
        {
            Debug.Log("Stage Completed !!");
            resultUI.enabled = true;
            clearUI.enabled = true;
        }

        if (gameManager.castleWallManager.isBreak)
        {
            Debug.Log("Stage Failed ...");
            resultUI.enabled = true;
            failedUI.enabled = true;
        }
    }

    // 敵の合計数テキスト
    public void WaveEnemyCntText(int currentCnt, int maxCnt)
    {
        currentEnemyCntText.text = $"{currentCnt} / {maxCnt}";
    }

    // 現在のウェーブテキスト
    public void CurrentWaveText(int value)
    {
        currentWaveText.text = "Wave " + (value + 1);
    }

    // 次ウェーブのUIを表示
    public void OnDisplayNextWaveUI()
    {
        nextWaveUI.enabled = true;
    }

    // 次ウェーブのUIを非表示
    public void OffDisplayNextWaveUI()
    {
        nextWaveUI.enabled = false;
    }

    // カウントダウンテキスト
    public void CountDownText(int value)
    {
        cntDownText.text = value.ToString();
    }

    // ウェーブ開始テキスト
    public void WaveStartText()
    {
        cntDownText.text = "スタート！";
    }

    // ウェーブまでの時間ゲージ
    public void NextWaveTimerGauge(float value)
    {
        cntDownGauge.fillAmount = value;
    }

    // ウェーブクリア時の報酬コスト
    public void WaveRewardText(int value)
    {
        rewardCostText.text = "+" + value;
    }

    // リタイアボタン(確認)
    public void OnRetireVerified()
    {
        retireUI.enabled = true;

        if(!gameManager.timeManager.isPause)
            gameManager.timeManager.GamePause();
    }

    // リタイアキャンセル
    public void OnRetireCanceled()
    {
        retireUI.enabled = false;

        if (gameManager.timeManager.isPause)
            gameManager.timeManager.GamePause();
    }

    // リタイアボタン(決定)
    public void OnRetireDecision()
    {
        SceneManager.LoadSceneAsync("MainScene");
    }

    // シーンロード
    public void OnLoadScene(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }
}
