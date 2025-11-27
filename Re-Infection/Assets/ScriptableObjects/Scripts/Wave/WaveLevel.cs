using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelStats
{
    public UnitStatsData statsData; // ユニットのScriptableObject
    public int spawnCnt;        // 出現する数
}

[CreateAssetMenu(fileName = "WaveLevel_", menuName = "Scriptable Objects/WaveLevel")]
public class WaveLevel : ScriptableObject
{
    public LevelStats[] levelStats;
    public float spawnInterbal;       // スポーンする時間

    // レベル生成コルーチン
    public IEnumerator SpawnLevel()
    {
        // レベル内のユニットを全て生成
        foreach (LevelStats Lstats in levelStats)
        {
            for (int i = 0; i < Lstats.spawnCnt; i++)
            {
                if (Lstats.statsData == null)
                {
                    Debug.LogAssertion("ユニットが設定されていない為、コルーチンを終了します");
                    yield break;
                }

                WaveSpawner.SpawnUnit(Lstats.statsData.unitStats);
                yield return null;
            }
        }

        yield break;
    }
}