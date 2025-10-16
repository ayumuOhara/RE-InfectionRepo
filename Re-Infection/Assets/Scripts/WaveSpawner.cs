using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] WaveData[] waveData;       // ステージのウェーブデータ
    [SerializeField] UnitManager unitManager;
    [SerializeField] GameObject unitObj;
    [SerializeField] Vector3 spawnPos;          // スポーン座標

    int currentWaveIdx = 0;         // 現在のウェーブ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnLevel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // レベル生成コルーチン
    IEnumerator SpawnLevel()
    {
        // 全てのウェーブを行うまでループ
        while (currentWaveIdx < waveData.Length)
        {
            var currentWave = waveData[currentWaveIdx]; // 現在のウェーブのデータ取得

            // ウェーブ内の全てのレベルを生成するまでループ
            for(int level = 0; level < currentWave.waveLevels.Length; level++)
            {
                var currentLevel = currentWave.waveLevels[level];  // 現在のレベルのデータ取得

                // レベル内のユニットを全て生成
                foreach (LevelStats Lstats in currentLevel.levelStats)
                {
                    for (int i = 0; i < Lstats.spawnCnt; i++)
                    {
                        SpawnUnit(Lstats.unitStats);
                    }
                }

                yield return new WaitForSeconds(waveData[currentWaveIdx].spawnInterbal);
            }

            // 敵全滅待機
            Debug.Log("ウェーブ内の敵が全滅するまで待機");
            yield return new WaitUntil(() => unitManager.IsAllEnemyDefeated);

            // 全滅後、ウェーブを進行し、ウェーブのレベルをリセット
            currentWaveIdx++;

            // 最終ウェーブの場合、即終了する
            if (currentWaveIdx < waveData.Length)
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
