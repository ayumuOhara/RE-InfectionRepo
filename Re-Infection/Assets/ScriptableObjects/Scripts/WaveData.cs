using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelStats
{
    public UnitStats unitStats; // ユニットのScriptableObject
    public int spawnCnt;        // 出現する数
}

[CreateAssetMenu(fileName = "WaveLevel", menuName = "Scriptable Objects/WaveLevel")]
public class Level : ScriptableObject
{
    public LevelStats[] levelStats;
}

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class WaveData : ScriptableObject
{
    public Level[] waveLevels;   // ウェーブでスポーンさせるレベルのリスト
    public float spawnInterbal;  // スポーンする時間
    public int rewardCost;       // ウェーブクリア後に獲得できるコスト
}