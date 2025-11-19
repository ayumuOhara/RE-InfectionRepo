using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    InGameUIManager gameUIManager; // UI管理マネージャ
    CostManager costManager;
    UnitManager unitManager;

    [SerializeField] AudioClip[] clearSe;

    [SerializeField] Stage stage;            // ステージのデータ
    [SerializeField] GameObject unitObj;
    [SerializeField] Vector3 spawnPos;       // スポーン座標

    int currentWaveIdx = 0;      // 現在のウェーブ
    int currentWaveEnemySum = 0; // 現在のウェーブの敵の残りの合計数

    const float WAVE_START_CNT = 0.5f;

    UnitController bossUnit;

    // ウェーブ内の敵を全て倒したか
    public bool isAllEnemyDefeatedInWave => currentWaveEnemySum <= 0;

    // ボスユニットを倒したか
    bool isBossDefeated => bossUnit.isDead;

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

            yield return StartCoroutine(WaveStart());
            yield return StartCoroutine(SpawnLevels(currentWave));

            if(currentWave.bossWave)
            {
                Debug.Log("ボスが撃破されるまで待機");
                StartCoroutine(BossUI());
                yield return new WaitUntil(() => isBossDefeated);
            }
            else
            {
                Debug.Log("ウェーブ内の敵が全滅するまで待機");
                yield return new WaitUntil(() => isAllEnemyDefeatedInWave);
            }

            // 最終ウェーブの場合、即終了する
            if (currentWave.bossWave)
            {
                StartCoroutine(StageClear());
                yield break;
            }
            else
            {
                // 全滅後、ウェーブを進行
                currentWaveIdx++;
                Debug.Log("全ての敵が全滅したので次のウェーブへ移行");
                Reward(currentWave);
                unitManager.AllPlayerUnitDestroy();
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

        isStartWave = true;

        yield break;
    }

    // ステージクリアコルーチン
    IEnumerator StageClear()
    {
        gameUIManager.InvisibleAllUI();
        gameUIManager.BossHealthProgress(0);
        FindAnyObjectByType<GameManager>().GetComponent<AudioSource>().Pause();

        Time.timeScale = 0.4f;

        AudioSource audio =  GetComponent<AudioSource>();
        foreach(var se in clearSe)
            audio.PlayOneShot(se);

        yield return new WaitForSeconds(1.0f);

        Time.timeScale = 1.0f;
        gameUIManager.VisibleAllUI();
        isSessionClear = true;

        yield break;
    }

    // レベル生成コルーチン
    IEnumerator SpawnLevels(WaveData currentWave)
    {
        // ウェーブ内の全てのレベルを生成するまでループ
        for (int level = 0; level < currentWave.waveLevels.Length; level++)
        {
            if (level != 0)
                yield return new WaitForSeconds(stage.waveData[currentWaveIdx].spawnInterbal);

            var currentLevel = currentWave.waveLevels[level];  // 現在のレベルのデータ取得

            // レベル内のユニットを全て生成
            foreach (LevelStats Lstats in currentLevel.levelStats)
            {
                for (int i = 0; i < Lstats.spawnCnt; i++)
                {
                    SpawnUnit(Lstats.unitStats);
                    yield return null;
                }
            }
        }


        yield break;
    }

    // ボスUI更新コルーチン
    IEnumerator BossUI()
    {
        Debug.Log("ボスUI表示コルーチン呼び出し");

        gameUIManager.BossNameText(bossUnit.unitName);

        while (!isBossDefeated)
        {
            gameUIManager.BossHealthText((int)bossUnit.currentHp);
            gameUIManager.BossHealthProgress(bossUnit.HealthRate);
            yield return null;
        }

        gameUIManager.BossHealthText((int)bossUnit.currentHp);


        yield break;
    }

    // ユニット生成
    void SpawnUnit(UnitStats unitStats)
    {
        spawnPos.x = Random.Range(-2f, 2f);

        UnitController uc = Instantiate(unitObj, spawnPos, Quaternion.identity).GetComponent<UnitController>();
        uc.transform.position = spawnPos;
        uc.SetUnitStats(unitStats, UnitGroup.Enemy);    // 生成したユニットにステータスを代入

        // ボスユニット代入
        if (unitStats.bossUnit)
            bossUnit = uc;
    }

    // ウェーブクリア報酬
    void Reward(WaveData currentWave)
    {
        var units = unitManager.GetUnitList(UnitGroup.Player);

        var unitCnt = 0;
        foreach (var unit in units)
        {
            if (!unit.isInfection)
                unitCnt++;
        }

        costManager.AddCost(currentWave.rewardCost + unitCnt);
        gameUIManager.WaveRewardText(currentWave.rewardCost);
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
            gameUIManager.BossNameText(currentWave.waveLevels[0].levelStats[0].unitStats.unitName);
            gameUIManager.BossHealthText((int)currentWave.waveLevels[0].levelStats[0].unitStats.maxHp);
            gameUIManager.BossHealthProgress(currentWave.waveLevels[0].levelStats[0].unitStats.maxHp / currentWave.waveLevels[0].levelStats[0].unitStats.maxHp);
            gameUIManager.CloseRewardLabel();
        }
        else
        {
            currentWaveEnemySum = currentWave.waveEnemySum;

            // UI変更
            gameUIManager.WaveEnemyCntText(currentWaveEnemySum);
            gameUIManager.CurrentWaveText(currentWaveIdx);
            gameUIManager.CurrentWaveProgress(currentWaveEnemySum, currentWaveEnemySum);
            gameUIManager.WaveRewardText(currentWave.rewardCost);
        }
    }
}
