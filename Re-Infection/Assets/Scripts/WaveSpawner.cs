using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    InGameUIManager gameUIManager; // UI管理マネージャ
    CostManager costManager;
    UnitManager unitManager;

    [SerializeField] Stage stage;            // ステージのデータ
    [SerializeField] GameObject unitObj;
    [SerializeField] Vector3 spawnPos;       // スポーン座標

    int currentWaveIdx = 0;      // 現在のウェーブ
    int currentWaveEnemySum = 0; // 現在のウェーブの敵の残りの合計数

    const int NEXT_WAVE_START_CNT = 3;

    UnitController bossUnit;

    // ウェーブ内の敵を全て倒したか
    public bool isAllEnemyDefeatedInWave => currentWaveEnemySum <= 0;

    // ボスユニットを倒したか
    bool isBossDefeated => bossUnit.isDead;

    // 周回をクリアしたか
    bool isSessionClear = false;
    public bool IsSessionClear => isSessionClear;

    void Awake()
    {
        gameUIManager = GameObject.Find("InGameUIManager").GetComponent<InGameUIManager>();
        costManager = GameObject.Find("CostManager").GetComponent<CostManager>();
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Wave());
    }

    // ウェーブ進行コルーチン
    IEnumerator Wave()
    {
        isSessionClear = false;

        while (true)
        {
            var currentWave = stage.waveData[currentWaveIdx]; // 現在のウェーブのデータ取得
            ChangeUIEnabled(currentWave);

            if (currentWave.bossWave)
            {
                gameUIManager.BossNameText(currentWave.waveLevels[0].levelStats[0].unitStats.unitName);
                gameUIManager.BossHealthText((int)currentWave.waveLevels[0].levelStats[0].unitStats.maxHp);
                gameUIManager.BossHealthProgress(currentWave.waveLevels[0].levelStats[0].unitStats.maxHp / currentWave.waveLevels[0].levelStats[0].unitStats.maxHp);
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
                isSessionClear = true;
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
        float waveStartTimer = NEXT_WAVE_START_CNT;
        while (waveStartTimer > 0)
        {
            gameUIManager.OnDisplayNextWaveUI();
            waveStartTimer -= Time.deltaTime;
            gameUIManager.CountDownText((int)waveStartTimer + 1);
            float amount = (float)waveStartTimer / NEXT_WAVE_START_CNT;
            gameUIManager.NextWaveTimerGauge(amount);

            yield return null;
        }

        gameUIManager.WaveStartText();
        yield return new WaitForSeconds(0.75f);
        gameUIManager.OffDisplayNextWaveUI();
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
    }

    // ユニット生成
    void SpawnUnit(UnitStats unitStats)
    {
        spawnPos.x = Random.Range(-2f, 2f);

        UnitController uc = Instantiate(unitObj, spawnPos, Quaternion.identity).GetComponent<UnitController>();
        uc.transform.position = spawnPos;
        uc.SetUnitStats(unitStats, UnitGroup.Enemy);    // 生成したユニットにステータスを代入

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
        currentWaveEnemySum--;
        gameUIManager.WaveEnemyCntText(currentWaveEnemySum);
        gameUIManager.CurrentWaveProgress(currentWaveEnemySum, stage.waveData[currentWaveIdx].waveEnemySum);
    }
}
