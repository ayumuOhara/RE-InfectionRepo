using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class WaveData : ScriptableObject
{
    public WaveLevel[] waveLevels;   // ウェーブでスポーンさせるレベルのリスト
    public int rewardCost;           // ウェーブクリア後に獲得できるコスト
    public bool bossWave;            // ボスウェーブか

    private List<UnitStats> spawnUnits;
    // スポーンするユニットの種類
    public List<UnitStats> SpawnUnits
    {
        get
        {
            if (spawnUnits != null)
            {
                return spawnUnits;
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

                spawnUnits = units.ToList();
                return spawnUnits;
            }
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        spawnUnits = null;
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