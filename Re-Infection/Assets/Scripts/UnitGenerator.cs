using Unity.VisualScripting;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    [SerializeField] UnitData unitData;

    [SerializeField] GameObject unitObj;
    [SerializeField] UnitManager unitManager;

    float generateInterbal; // ������̌o�ߎ���
    float generateTime = 5.0f;     // ��������Ԋu

    Vector3 enemyUnitGeneratePos = new Vector3(0, 5.5f, 0);    // �G�l�~�[���j�b�g�̐������W

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generateInterbal = 0;
        GenerateUnit();
    }

    // Update is called once per frame
    void Update()
    {
        generateInterbal += Time.deltaTime;

        if (generateInterbal > generateTime)
        {
            generateInterbal = 0;

            GenerateUnit();
        }
    }

    // �G�l�~�[����
    void GenerateUnit()
    {
        var rnd_Idx = Random.Range(3, unitData.stats.Count - 1);
        var rndXpos = Random.Range(-1.7f, 1.7f);

        // �Ή�����C���f�b�N�X�̃��j�b�g�I�u�W�F�N�g�𐶐�
        GameObject unit = null;
        enemyUnitGeneratePos.x = rndXpos;
        unit = Instantiate(unitObj, enemyUnitGeneratePos, Quaternion.identity);

        UnitController uc = unit.GetComponent<UnitController>();
        uc.SetUnitStats(unitData.stats[rnd_Idx], UnitGroup.Enemy);

        // ���j�b�g���Ǘ�����UnitManager�ɐ����������j�b�g�̐w�c�ɑΉ����郊�X�g�Ɋi�[
        UnitManager um = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        um.AddUnitList(unit, UnitGroup.Enemy);
    }
}
