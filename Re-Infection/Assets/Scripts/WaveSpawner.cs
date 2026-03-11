using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class WaveSpawner : MonoBehaviour
{
    InGameUIManager gameUIManager; // UI管理マネージャ
    CostManager costManager;
    UnitManager unitManager;
    TimeManager timeManager;
    public StageData stageData;

    [SerializeField] Image castlePoint;

    [SerializeField] Stage stage;            // ステージのデータ

    [SerializeField] SpriteRenderer backgroundRenderer;     // 背景表示用のSpriteRenderer
    public Stage CurrentStage => stage;

    public int currentWaveIdx { get; private set; } = 0;      // 現在のウェーブ
    int currentWaveEnemySum = 0; // 現在のウェーブの敵の残りの合計数

    const float WAVE_START_CNT = 0.5f;

    EnemyUnit bossUnit;

    // ウェーブ内の敵を全て倒したか
    public bool isAllEnemyDefeatedInWave => currentWaveEnemySum <= 0;

    // ボスユニットを倒したか
    bool isBossDefeated => bossUnit.IsDead;

    // ウェーブが始まったか
    bool isStartWave = false;
    public bool IsStartWave => isStartWave;

    // 周回をクリアしたか
    bool isSessionClear = false;
    public bool IsSessionClear => isSessionClear;

    void Awake()
    {
        gameUIManager = GameObject.Find("InGameUI").GetComponent<InGameUIManager>();
        costManager = GameObject.Find("CostManager").GetComponent<CostManager>();
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        stage = stageData.Stage[stageData.SelectStageNumber];

        backgroundRenderer.sprite = stage.background;       // 選択されたステージの背景画像をSpriteRendererに反映
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var currentWave = stage.waveData[currentWaveIdx]; // 現在のウェーブのデータ取得
        SetWaveUI(currentWave);

        StartCoroutine(Wave());
    }

    // ウェーブ進行コルーチン
    IEnumerator Wave()
    {
        StartCoroutine(FindObjectOfType<TimeManager>().SessionTimer());
        isSessionClear = false;

        while (true)
        {
            isStartWave = false;

            var currentWave = stage.waveData[currentWaveIdx]; // 現在のウェーブのデータ取得
            SetWaveUI(currentWave);

            yield return WaveStart();
            yield return currentWave.SpawnLevels();

            if(currentWave.bossWave)
            {
                StartCoroutine(BossUI());
                yield return new WaitUntil(() => isBossDefeated);
            }
            else
            {
                yield return new WaitUntil(() => isAllEnemyDefeatedInWave);
            }

            // 最終ウェーブの場合、即終了する
            if (currentWave.bossWave)
            {
                timeManager.SetSessionClearTime();
                CurrentStage.SetStageBestClearTime(timeManager.sessionClearTime);

                StartCoroutine(StageClear());
                yield break;
            }
            else
            {
                // 全滅後、ウェーブを進行
                currentWaveIdx++;
                unitManager.AllUnitDestroy("Player");
            }
        }
    }

    // ボスUIとウェーブUIの表示切替
    void ChangeUIEnabled(WaveData currentWave)
    {
        if (currentWave.bossWave)
        {
            gameUIManager.VisibleBossNameText();
            gameUIManager.VisibleBossHealth();
            gameUIManager.InvisibleWaveEnemyCntText();
            gameUIManager.InvisibleCurrentWaveText();
        }
        else
        {
            gameUIManager.InvisibleBossNameText();
            gameUIManager.InvisibleBossHealth();
            gameUIManager.VisibleWaveEnemyCntText();
            gameUIManager.VisibleCurrentWaveText();
        }
    }

    // ウェーブ開始コルーチン
    IEnumerator WaveStart()
    {
        gameUIManager.OpenHoldLabel();
        castlePoint.enabled = true;

        float holdTime = 0;
        while (holdTime < WAVE_START_CNT)
        {
            if (Input.GetMouseButton(0))
            {
                gameUIManager.VisibleHoldIcon();

                holdTime += Time.deltaTime;
                float amount = (float)holdTime / WAVE_START_CNT;
                gameUIManager.HoldProgressGauge(amount);

                yield return null;
            }
            else
            {
                holdTime = 0;
                gameUIManager.InvisibleHoldIcon();

                yield return null;
            }
        }

        gameUIManager.CloseHoldLabel();
        gameUIManager.InvisibleHoldIcon();
        castlePoint.enabled = false;

        isStartWave = true;

        yield break;
    }

    // ステージクリアコルーチン
    IEnumerator StageClear()
    {
        gameUIManager.InvisibleAllUI();
        gameUIManager.BossHealthProgress(0);
        FindObjectOfType<BGMManager>().StopBGM();

        Time.timeScale = 0.4f;

        FindObjectOfType<SEManager>().PlaySE(SEManager.SEType.BossDefeat);

        yield return new WaitForSeconds(1.0f);

        Time.timeScale = 1.0f;
        gameUIManager.VisibleAllUI();
        isSessionClear = true;

        yield break;
    }

    // ボスUI更新コルーチン
    IEnumerator BossUI()
    {
        gameUIManager.BossNameText(bossUnit.Stats.unitName);

        while (!isBossDefeated)
        {
            gameUIManager.BossHealthText((int)bossUnit.CurrentHealth);
            gameUIManager.BossHealthProgress(bossUnit.HealthRate);
            yield return null;
        }

        gameUIManager.BossHealthText((int)bossUnit.CurrentHealth);

        yield break;
    }

    // ボスユニット設定
    public void SetBoss(EnemyUnit boss)
    {
        bossUnit = boss;
    }

    // ウェーブの敵の残りの合計数を減らす
    public void DecreaseEnemySum()
    {
        if (stage.waveData[currentWaveIdx].bossWave) return;

        currentWaveEnemySum--;
        gameUIManager.WaveEnemyCntText(currentWaveEnemySum);
        gameUIManager.CurrentWaveProgress(currentWaveEnemySum, stage.waveData[currentWaveIdx].waveEnemySum);
    }

    // ウェーブUI初期化
    void SetWaveUI(WaveData currentWave)
    {
        ChangeUIEnabled(currentWave);

        if (currentWave.bossWave)
        {
            gameUIManager.BossNameText(currentWave.waveLevels[0].levelStats[0].statsData.unitStats.unitName);
            gameUIManager.BossHealthText((int)currentWave.waveLevels[0].levelStats[0].statsData.unitStats.maxHp);
            gameUIManager.BossHealthProgress(currentWave.waveLevels[0].levelStats[0].statsData.unitStats.maxHp / currentWave.waveLevels[0].levelStats[0].statsData.unitStats.maxHp);
        }
        else
        {
            currentWaveEnemySum = currentWave.waveEnemySum;

            // UI変更
            gameUIManager.WaveEnemyCntText(currentWaveEnemySum);
            gameUIManager.CurrentWaveText(currentWaveIdx);
            gameUIManager.CurrentWaveProgress(currentWaveEnemySum, currentWaveEnemySum);
        }
    }
}
