using Unity.VisualScripting;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    [SerializeField] UnitData unitData;

    [SerializeField] UnitManager unitManager;

    float generateInterbal; // ������̌o�ߎ���
    float generateTime = 2.0f;     // ��������Ԋu

    UnitGroup generateGroup = UnitGroup.Enemy;  // �������郆�j�b�g�̐w�c

    Vector3 playerUnitGenaretePos = new Vector3(0, -3, 0);  // �v���C���[���j�b�g�̐������W
    Vector3 enemyUnitGeneratePos = new Vector3(0, 6, 0);    // �G�l�~�[���j�b�g�̐������W

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generateInterbal = 0;

        for(int i = 0; i < 2; i++)
            GenerateUnit();
    }

    // Update is called once per frame
    void Update()
    {
        //generateInterbal += Time.deltaTime;

        //if(generateInterbal > generateTime)
        //{
        //    generateInterbal = 0;

        //    GenerateUnit();
        //}
    }

    // �G�l�~�[����
    void GenerateUnit()
    {
        var rnd_Idx = Random.Range(0, unitData.stats.Count);    // �����_���ȃ��j�b�g�̃C���f�b�N�X�𐶐�

        // �Ή�����C���f�b�N�X�̃��j�b�g�I�u�W�F�N�g�𐶐�
        GameObject unit = null;
        if (generateGroup == UnitGroup.Player)
            unit = Instantiate(unitData.stats[rnd_Idx].unitObj, playerUnitGenaretePos, Quaternion.identity);
        else
            unit = Instantiate(unitData.stats[rnd_Idx].unitObj, enemyUnitGeneratePos, Quaternion.identity);

        // ���������I�u�W�F�N�g��UnitController�R���|�[�l���g��t�^&���
        UnitController uc = unit.AddComponent<UnitController>();
        // �����������j�b�g�ɐ��������C���f�b�N�X�̃��j�b�g�̃X�^�b�c����
        uc.SetUnitStats(unitData.stats[rnd_Idx], generateGroup);

        // ���j�b�g���Ǘ�����UnitManager�ɐ����������j�b�g�̐w�c�ɑΉ����郊�X�g�Ɋi�[
        unitManager.AddUnitList(unit, generateGroup);

        // ���񐶐����郆�j�b�g�̐w�c��؂�ւ�
        generateGroup = (generateGroup == UnitGroup.Enemy ? UnitGroup.Player : UnitGroup.Enemy);
    }
}
