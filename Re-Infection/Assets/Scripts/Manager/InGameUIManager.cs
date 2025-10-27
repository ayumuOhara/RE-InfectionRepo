using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    GameManager gameManager;

    Canvas resultWindow;
    Canvas clearWindow;
    Canvas failedWindow;

    TextMeshProUGUI currentWaveText;
    TextMeshProUGUI rewardCostText;
    TextMeshProUGUI currentEnemyCntText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        resultWindow = GameObject.Find("ResultWindow").GetComponent<Canvas>();
        clearWindow = GameObject.Find("ClearWindow").GetComponent<Canvas>();
        failedWindow = GameObject.Find("FailedWindow").GetComponent<Canvas>();

        currentWaveText = GameObject.Find("CurrentWaveText").GetComponent<TextMeshProUGUI>();
        rewardCostText = GameObject.Find("RewardsCostCnt").GetComponent<TextMeshProUGUI>();
        currentEnemyCntText = GameObject.Find("EnemyCnt").GetComponent<TextMeshProUGUI>();

        resultWindow.enabled = false;
        clearWindow.enabled = false;
        failedWindow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.waveSpawner.isStageCompleted)
        {
            Debug.Log("Stage Completed !!");
            resultWindow.enabled = true;
            clearWindow.enabled = true;
        }

        if (gameManager.castleWallManager.isBreak)
        {
            Debug.Log("Stage Failed ...");
            resultWindow.enabled = true;
            failedWindow.enabled = true;
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

    // ウェーブクリア時の報酬コスト
    public void WaveRewardText(int value)
    {
        rewardCostText.text = "+" + value;
    }

    // シーンロード
    public void OnLoadScene(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }
}
