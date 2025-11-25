using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class WaveData : ScriptableObject
{
    public WaveLevel[] waveLevels;   // ウェーブでスポーンさせるレベルのリスト
    public bool bossWave;            // ボスウェーブか

    // レベル生成コルーチン
    public IEnumerator SpawnLevels()
    {
        // ウェーブ内の全てのレベルを生成するまでループ
        for (int level = 0; level < waveLevels.Length; level++)
        {
            if (level != 0)
                yield return new WaitForSeconds(waveLevels[level].spawnInterbal);

            var currentLevel = waveLevels[level];  // 現在のレベルのデータ取得

            // レベル内のユニットを全て生成
            yield return currentLevel.SpawnLevel();
        }

        yield break;
    }


    private List<UnitStats> spawnUnitsList;
    // スポーンするユニットの種類
    public List<UnitStats> SpawnUnitsList
    {
        get
        {
            if (spawnUnitsList != null)
            {
                return spawnUnitsList;
            }
            else
            {
                var units = new HashSet<UnitStats>();
                foreach (var level in waveLevels)
                {
                    foreach (var stats in level.levelStats)
                    {
                        units.Add(stats.unitStats);
                    }
                }

                spawnUnitsList = units.ToList();
                return spawnUnitsList;
            }
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        spawnUnitsList = null;
    }
#endif

    // ウェーブ内の敵の数
    public int waveEnemySum
    {
        get
        {
            int sum = 0;
            foreach (WaveLevel waveLevel in waveLevels)
            {
                foreach (LevelStats levelStats in waveLevel.levelStats)
                {
                    sum += levelStats.spawnCnt;
                }
            }
            return sum;
        }

        private set { }
    }
}