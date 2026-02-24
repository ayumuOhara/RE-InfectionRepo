using UnityEngine;
using System.Collections;
using System;

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

    public static event Action<UnitStats, LayerMask, Vector3> OnSpawnUnit;

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

                var spawnPos = new Vector3(0, 4.5f, 0);
                spawnPos.x = UnityEngine.Random.Range(-2f, 2f);

                // LayerMask
                // 7 == EnemyUnit
                OnSpawnUnit?.Invoke(Lstats.statsData.unitStats, 7, spawnPos);
                yield return null;
            }
        }

        yield break;
    }
}