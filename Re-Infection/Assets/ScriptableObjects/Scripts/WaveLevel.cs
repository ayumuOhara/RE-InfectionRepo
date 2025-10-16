using UnityEngine;

[System.Serializable]
public class LevelStats
{
    public UnitStats unitStats; // ���j�b�g��ScriptableObject
    public int spawnCnt;        // �o�����鐔
}

[CreateAssetMenu(fileName = "WaveLevel", menuName = "Scriptable Objects/WaveLevel")]
public class WaveLevel : ScriptableObject
{
    public LevelStats[] levelStats;
}