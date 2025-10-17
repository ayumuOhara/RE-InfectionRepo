using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;

[System.Serializable]
public class Stage
{
    public WaveData[] waveData;       // ステージのウェーブデータ
}

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] Stage[] stages;            // ステージのデータ
    [SerializeField] UnitManager unitManager;
    [SerializeField] GameObject unitObj;
    [SerializeField] Vector3 spawnPos;          // スポーン座標

    int currentWaveIdx = 0; // 現在のウェーブ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnLevels());
    }

    // レベル生成コルーチン
    IEnumerator SpawnLevels()
    {
        // 全てのウェーブを行うまでループ
        while (currentWaveIdx < stages[0].waveData.Length)
        {
            var currentWave = stages[0].waveData[currentWaveIdx]; // 現在のウェーブのデータ取得

            // ウェーブ内の全てのレベルを生成するまでループ
            for(int level = 0; level < currentWave.waveLevels.Length; level++)
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
            yield return new WaitUntil(() => unitManager.IsAllEnemyDefeated);

            // 全滅後、ウェーブを進行し、ウェーブのレベルをリセット
            currentWaveIdx++;

            // 最終ウェーブの場合、即終了する
            if (currentWaveIdx < stages[0].waveData.Length)
            {
                Debug.Log("全ての敵が全滅したので次のウェーブへ移行");
                yield return new WaitForSeconds(3.0f);
            }
            else
            {
                Debug.Log("ウェーブを全て完了しました");
                Debug.Log("ステージクリア");
                yield break;
            }
        }
    }

    // ユニット生成
    void SpawnUnit(UnitStats unitStats)
    {
        spawnPos.x = Random.Range(-1.7f, 1.7f);

        GameObject obj = Instantiate(unitObj, spawnPos, Quaternion.identity);
        UnitController uc = obj.GetComponent<UnitController>();
        uc.SetUnitStats(unitStats, UnitGroup.Enemy);    // 生成したユニットにステータスを代入

        unitManager.AddUnitList(obj, UnitGroup.Enemy);  // UnitManagerのリストに追加
    }
}
