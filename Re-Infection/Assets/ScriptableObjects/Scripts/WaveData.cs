using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class WaveData : ScriptableObject
{
    public WaveLevel[] waveLevels;   // �E�F�[�u�ŃX�|�[�������郌�x���̃��X�g
    public float spawnInterbal;  // �X�|�[�����鎞��
    public int rewardCost;       // �E�F�[�u�N���A��Ɋl���ł���R�X�g
}