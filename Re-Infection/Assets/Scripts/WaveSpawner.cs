using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class Stage
{
    public WaveData[] waveData;       // ステージのウェーブデータ
}

public class WaveSpawner : MonoBehaviour
{
    InGameUIManager gameUIManager; // UI管理マネージャ
    CostManager costManager;
    UnitManager unitManager;

    [SerializeField] Stage[] stages;            // ステージのデータ
    [SerializeField] GameObject unitObj;
    [SerializeField] Vector3 spawnPos;          // スポーン座標

    int currentWaveIdx = 0;      // 現在のウェーブ
    int currentWaveEnemySum = 0; // 現在のウェーブの敵の残りの合計数

    // ウェーブ内の敵を全て倒したか
    public bool isAllEnemyDefeatedInWave => currentWaveEnemySum <= 0;
    // ステージクリアフラグ
    public bool isStageCompleted => currentWaveIdx >= stages[0].waveData.Length;

    void Awake()
    {
        gameUIManager = GameObject.Find("InGameUIManager").GetComponent<InGameUIManager>();
        costManager = GameObject.Find("CostManager").GetComponent<CostManager>();
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnLevels());
    }

    // レベル生成コルーチン
    IEnumerator SpawnLevels()
    {
        while (true)
        {
            var currentWave = stages[0].waveData[currentWaveIdx]; // 現在のウェーブのデータ取得

            currentWaveEnemySum = currentWave.waveEnemySum;

            gameUIManager.WaveEnemyCntText(currentWaveEnemySum, currentWave.waveEnemySum);
            gameUIManager.CurrentWaveText(currentWaveIdx);
            gameUIManager.WaveRewardText(currentWave.rewardCost);

            // ウェーブ内の全てのレベルを生成するまでループ
            for (int level = 0; level < currentWave.waveLevels.Length; level++)
            {
                if(level != 0)
                    yield return new WaitForSeconds(stages[0].waveData[currentWaveIdx].spawnInterbal);

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

            // 敵全滅待機
            Debug.Log("ウェーブ内の敵が全滅するまで待機");
            yield return new WaitUntil(() => isAllEnemyDefeatedInWave);

            // 全滅後、ウェーブを進行し、ウェーブのレベルをリセット
            currentWaveIdx++;

            // 最終ウェーブの場合、即終了する
            if (currentWave.bossWave)
            {
                yield break;

            }
            else
            {
                Debug.Log("全ての敵が全滅したので次のウェーブへ移行");
                Reward(currentWave);
                unitManager.WaveEnd();
                yield return new WaitForSeconds(3.0f);
            }
        }
    }

    // ユニット生成
    void SpawnUnit(UnitStats unitStats)
    {
        spawnPos.x = Random.Range(-2f, 2f);

        GameObject obj = Instantiate(unitObj, spawnPos, Quaternion.identity);
        UnitController uc = obj.GetComponent<UnitController>();
        uc.SetUnitStats(unitStats, UnitGroup.Enemy);    // 生成したユニットにステータスを代入
    }

    // ウェーブクリア報酬
    void Reward(WaveData currentWave)
    {
        costManager.AddCost(currentWave.rewardCost);
        gameUIManager.WaveRewardText(currentWave.rewardCost);
    }

    // ウェーブの敵の残りの合計数を減らす
    public void DecreaseEnemySum()
    {
        currentWaveEnemySum--;
        gameUIManager.WaveEnemyCntText(currentWaveEnemySum, stages[0].waveData[currentWaveIdx].waveEnemySum);
    }
}
