using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class WaveData : ScriptableObject
{
    public WaveLevel[] waveLevels;   // ウェーブでスポーンさせるレベルのリスト
    public float spawnInterbal;      // スポーンする時間
    public int rewardCost;           // ウェーブクリア後に獲得できるコスト
    public bool bossWave;            // ボスウェーブか

    public int waveEnemySum => SpawnEnemySum(); // ウェーブ内の敵の合計数

    // ウェーブ内の敵の合計数を返す
    int SpawnEnemySum()
    {
        int sum = 0;
        foreach (WaveLevel waveLevel in waveLevels)
        {
            foreach(LevelStats levelStats in waveLevel.levelStats)
            {
                sum += levelStats.spawnCnt;
            }
        }
        return sum;
    }
}