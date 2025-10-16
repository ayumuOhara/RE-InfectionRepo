using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelStats
{
    public UnitStats unitStats; // ���j�b�g��ScriptableObject
    public int spawnCnt;        // �o�����鐔
}

[CreateAssetMenu(fileName = "WaveLevel", menuName = "Scriptable Objects/WaveLevel")]
public class Level : ScriptableObject
{
    public LevelStats[] levelStats;
}

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class WaveData : ScriptableObject
{
    public Level[] waveLevels;   // �E�F�[�u�ŃX�|�[�������郌�x���̃��X�g
    public float spawnInterbal;  // �X�|�[�����鎞��
    public int rewardCost;       // �E�F�[�u�N���A��Ɋl���ł���R�X�g
}