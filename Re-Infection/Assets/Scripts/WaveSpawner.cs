using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;

[System.Serializable]
public class Stage
{
    public WaveData[] waveData;       // �X�e�[�W�̃E�F�[�u�f�[�^
}

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] Stage[] stages;            // �X�e�[�W�̃f�[�^
    [SerializeField] UnitManager unitManager;
    [SerializeField] GameObject unitObj;
    [SerializeField] Vector3 spawnPos;          // �X�|�[�����W

    int currentWaveIdx = 0; // ���݂̃E�F�[�u

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnLevels());
    }

    // ���x�������R���[�`��
    IEnumerator SpawnLevels()
    {
        // �S�ẴE�F�[�u���s���܂Ń��[�v
        while (currentWaveIdx < stages[0].waveData.Length)
        {
            var currentWave = stages[0].waveData[currentWaveIdx]; // ���݂̃E�F�[�u�̃f�[�^�擾

            // �E�F�[�u���̑S�Ẵ��x���𐶐�����܂Ń��[�v
            for(int level = 0; level < currentWave.waveLevels.Length; level++)
            {
                if(level != 0)
                    yield return new WaitForSeconds(stages[0].waveData[currentWaveIdx].spawnInterbal);

                var currentLevel = currentWave.waveLevels[level];  // ���݂̃��x���̃f�[�^�擾

                // ���x�����̃��j�b�g��S�Đ���
                foreach (LevelStats Lstats in currentLevel.levelStats)
                {
                    for (int i = 0; i < Lstats.spawnCnt; i++)
                    {
                        SpawnUnit(Lstats.unitStats);
                        yield return null;
                    }
                }
            }

            // �G�S�őҋ@
            Debug.Log("�E�F�[�u���̓G���S�ł���܂őҋ@");
            yield return new WaitUntil(() => unitManager.IsAllEnemyDefeated);

            // �S�Ō�A�E�F�[�u��i�s���A�E�F�[�u�̃��x�������Z�b�g
            currentWaveIdx++;

            // �ŏI�E�F�[�u�̏ꍇ�A���I������
            if (currentWaveIdx < stages[0].waveData.Length)
            {
                Debug.Log("�S�Ă̓G���S�ł����̂Ŏ��̃E�F�[�u�ֈڍs");
                yield return new WaitForSeconds(3.0f);
            }
            else
            {
                Debug.Log("�E�F�[�u��S�Ċ������܂���");
                Debug.Log("�X�e�[�W�N���A");
                yield break;
            }
        }
    }

    // ���j�b�g����
    void SpawnUnit(UnitStats unitStats)
    {
        spawnPos.x = Random.Range(-1.7f, 1.7f);

        GameObject obj = Instantiate(unitObj, spawnPos, Quaternion.identity);
        UnitController uc = obj.GetComponent<UnitController>();
        uc.SetUnitStats(unitStats, UnitGroup.Enemy);    // �����������j�b�g�ɃX�e�[�^�X����

        unitManager.AddUnitList(obj, UnitGroup.Enemy);  // UnitManager�̃��X�g�ɒǉ�
    }
}
