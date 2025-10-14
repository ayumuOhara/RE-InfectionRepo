using UnityEngine;
using System.Collections.Generic;

public static class GetTarget
{
    public static GameObject GetTargetObj(UnitGroup targetGroup, Vector3 myPos)
    {
        UnitManager unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();

        // �擾�������w�c�̃��X�g�i�[�p�ϐ�
        List<GameObject> targetUnitList = new List<GameObject>();

        // �ړI�̐w�c�̃��j�b�g���X�g����
        if(targetGroup == UnitGroup.Player)
            targetUnitList = unitManager.playerUnitList;
        if (targetGroup == UnitGroup.Enemy)
            targetUnitList = unitManager.enemyUnitList;

        if(targetUnitList == null || targetUnitList.Count == 0) return null; // �Ώۂ��擾�ł��Ȃ��ꍇ�Anull��Ԃ�

        GameObject nearestObj = null;

        foreach(GameObject targetUnit in targetUnitList)
        {
            if (nearestObj == null)
            {
                nearestObj = targetUnit;
            }
            else
            {
                // ���݂�nearestObj��targetUnit��苗�����߂������炻�̂܂܂ɂ��AtargetUnit�̕����߂��ꍇ�AtargetUnit����
                nearestObj = Vector3.Distance(nearestObj.transform.position, myPos) < Vector3.Distance(targetUnit.transform.position, myPos) ? nearestObj : targetUnit;
            }
        }

        return nearestObj;
    }
}
