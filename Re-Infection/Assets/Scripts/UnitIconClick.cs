using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnitIconClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] UnitData unitData;
    [SerializeField] int unitIdx;
    [SerializeField] GameObject unitObj;

    Vector3 playerUnitGenaretePos = new Vector3(0, -1.5f, 0);  // �v���C���[���j�b�g�̐������W

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GenerateUnit();
        }
    }

    // ���j�b�g����
    void GenerateUnit()
    {
        var rndXpos = Random.Range(-1.7f, 1.7f);

        // ���j�b�g�I�u�W�F�N�g�𐶐�
        GameObject unit = null;
        playerUnitGenaretePos.x = rndXpos;
        unit = Instantiate(unitObj, playerUnitGenaretePos, Quaternion.identity);

        // �Ή�����C���f�b�N�X�̃��j�b�g�̃X�e�[�^�X��n��
        UnitController uc = unit.GetComponent<UnitController>();
        uc.SetUnitStats(unitData.stats[unitIdx], UnitGroup.Player);

        // ���j�b�g���Ǘ�����UnitManager�ɐ����������j�b�g�̐w�c�ɑΉ����郊�X�g�Ɋi�[
        UnitManager um = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        um.AddUnitList(unit, UnitGroup.Player);
    }
}
