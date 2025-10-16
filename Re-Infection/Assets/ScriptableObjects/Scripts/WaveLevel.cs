using UnityEngine;

[System.Serializable]
public class LevelStats
{
    public UnitStats unitStats; // ユニットのScriptableObject
    public int spawnCnt;        // 出現する数
}

[CreateAssetMenu(fileName = "WaveLevel", menuName = "Scriptable Objects/WaveLevel")]
public class WaveLevel : ScriptableObject
{
    public LevelStats[] levelStats;
}