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

    [SerializeField] Canvas resultUI;
    [SerializeField] Canvas clearUI;
    [SerializeField] Canvas failedUI;
    [SerializeField] Canvas retireUI;
    [SerializeField] Canvas nextWaveUI;

    [SerializeField] TextMeshProUGUI currentWaveText;
    [SerializeField] GameObject rewardIcon;
    [SerializeField] TextMeshProUGUI rewardCostText;
    [SerializeField] TextMeshProUGUI currentEnemyCntText;
    [SerializeField] Image currentWaveProgress;

    [SerializeField] TextMeshProUGUI bossNameText;
    [SerializeField] TextMeshProUGUI bossHealthText;

    [SerializeField] TextMeshProUGUI cntDownText;
    [SerializeField] Image cntDownGauge;

    AudioSource SeAudio;
    [SerializeField] AudioClip lordSe;
    [SerializeField] AudioClip decideSe;
    [SerializeField] AudioClip cancelSe;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SeAudio = GetComponent<AudioSource>();

        resultUI.enabled = false;
        clearUI.enabled = false;
        failedUI.enabled = false;
        nextWaveUI.enabled = false;
        retireUI.enabled = false;
    }

    // ステージクリア処理
    public void StageClear()
    {
        Debug.Log("Stage Completed !!");
        resultUI.enabled = true;
        clearUI.enabled = true;
    }

    // ステージ失敗処理
    public void StageFailed()
    {
        Debug.Log("Stage Failed ...");
        resultUI.enabled = true;
        failedUI.enabled = true;
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
        currentWaveText.text = "ウェーブ " + (value + 1);
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
        currentWaveProgress.fillAmount = (float)value / max;
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

    // ウェーブクリア時の報酬コスト非表示
    public void InvisibleRewardIcon()
    {
        rewardIcon.SetActive(false);
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
        bossHealthText.text = value.ToString();
    }

    // ボスHPバー表記
    public void BossHealthProgress(float progress)
    {
        currentWaveProgress.fillAmount = progress;
    }

    // リタイアボタン(確認)
    public void OnRetireVerified()
    {
        SeAudio.PlayOneShot(decideSe);
        retireUI.enabled = true;
    }

    // リタイアキャンセル
    public void OnRetireCanceled()
    {
        SeAudio.PlayOneShot(cancelSe);
        retireUI.enabled = false;
    }

    // シーンロード
    public void OnLoadScene(string name)
    {
        SeAudio.PlayOneShot(lordSe);
        SceneTransitionner transitonner = Instantiate(transitionUIprefab).GetComponent<SceneTransitionner>();
        transitonner.OnLoadScene(name);
    }
}
