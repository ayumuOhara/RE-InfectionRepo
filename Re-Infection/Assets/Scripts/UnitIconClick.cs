using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnitIconClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] UnitStats unitStats;
    [SerializeField] GameObject unitObj;

    CostManager costManager;

    Vector3 spawnPos = new Vector3(0, -1.5f, 0);  // �v���C���[���j�b�g�̐������W

    void Start()
    {
        costManager = GameObject.Find("CostManager").GetComponent<CostManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (costManager.EnoughCost(unitStats.summonCost))
                GenerateUnit();
            else
                Debug.Log("�R�X�g������܂���");
        }
    }

    // ���j�b�g����
    void GenerateUnit()
    {
        costManager.RemoveCost(unitStats.summonCost);

        spawnPos.x = Random.Range(-1.7f, 1.7f);

        // ���j�b�g�I�u�W�F�N�g�𐶐�
        GameObject unit = Instantiate(unitObj, spawnPos, Quaternion.identity);

        // �Ή�����C���f�b�N�X�̃��j�b�g�̃X�e�[�^�X��n��
        UnitController uc = unit.GetComponent<UnitController>();
        uc.SetUnitStats(unitStats, UnitGroup.Player);

        // ���j�b�g���Ǘ�����UnitManager�ɐ����������j�b�g�̐w�c�ɑΉ����郊�X�g�Ɋi�[
        UnitManager um = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        um.AddUnitList(unit, UnitGroup.Player);
    }
}
