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
    [SerializeField] Canvas resultUI;
    [SerializeField] Canvas clearUI;
    [SerializeField] Canvas failedUI;
    [SerializeField] Canvas retireUI;

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

    AudioSource SeAudio;
    [SerializeField] AudioClip lordSe;
    [SerializeField] AudioClip decideSe;
    [SerializeField] AudioClip cancelSe;
    [SerializeField] AudioClip stageClearSe;
    [SerializeField] AudioClip stageFailedSe;
    [SerializeField] AudioClip resultBgm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SeAudio = GetComponent<AudioSource>();

        resultUI.enabled = false;
        clearUI.enabled = false;
        failedUI.enabled = false;
        retireUI.enabled = false;
    }

    // 全UI表示
    public void VisibleAllUI()
    {
        masterUI.enabled = true;
    }

    // 全UI非表示
    public void InvisibleAllUI()
    {
        masterUI.enabled = false;
    }

    // 戦闘UIを非表示
    public void InvisibleCombatUI()
    {
        combatUI.enabled = false;
    }

    // ステージクリア処理
    public IEnumerator SessionClear()
    {
        clearTimeText.text = gameManager.timeManager.Minutes.ToString("D2") + ":" + gameManager.timeManager.Seconds.ToString("D2");

        GetComponent<AudioSource>().Pause();

        resultUI.enabled = true;
        clearUI.enabled = true;
        GetComponent<AudioSource>().PlayOneShot(stageClearSe);

        yield return new WaitForSeconds(2.5f);

        GetComponent<AudioSource>().clip = resultBgm;
        GetComponent<AudioSource>().Play();
    }

    // ステージ失敗処理
    public IEnumerator SessionFailed()
    {
        combatUI.enabled = false;
        GetComponent<AudioSource>().Pause();
        FindAnyObjectByType<GameManager>().GetComponent<AudioSource>().Pause();

        resultUI.enabled = true;
        failedUI.enabled = true;
        GetComponent<AudioSource>().PlayOneShot(stageFailedSe);

        yield return new WaitForSeconds(2.5f);

        GetComponent<AudioSource>().clip = resultBgm;
        GetComponent<AudioSource>().Play();
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

    // リタイアボタン(確認)
    public void OnRetireVerified()
    {
        SeAudio.PlayOneShot(decideSe);
        retireUI.enabled = true;

        if (!gameManager.timeManager.isPause)
            gameManager.timeManager.GamePause();
    }

    // リタイアキャンセル
    public void OnRetireCanceled()
    {
        SeAudio.PlayOneShot(cancelSe);
        retireUI.enabled = false;

        if (gameManager.timeManager.isPause)
            gameManager.timeManager.GamePause();
    }

    // シーンロード
    public void OnLoadScene(string name)
    {
        if (gameManager.timeManager.isPause)
            gameManager.timeManager.GamePause();

        SeAudio.PlayOneShot(lordSe);
        SceneTransitionner transitonner = Instantiate(transitionUIprefab).GetComponent<SceneTransitionner>();
        transitonner.OnLoadScene(name);
    }
}
