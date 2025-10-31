using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class GetTarget
{
    public static GameObject GetTargetObj(UnitGroup targetGroup, Vector3 myPos)
    {
        UnitManager unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();

        // 取得したい陣営のリスト格納用変数
        List<UnitController> targetUnitList = new List<UnitController>();

        // 目的の陣営のユニットリストを代入
        if(targetGroup == UnitGroup.Player)
            targetUnitList = unitManager.playerUnitList;
        if (targetGroup == UnitGroup.Enemy)
            targetUnitList = unitManager.enemyUnitList;

        if(targetUnitList == null || targetUnitList.Count == 0) return null; // 対象が取得できない場合、nullを返す

        GameObject nearestObj = null;

        foreach(UnitController targetUnit in targetUnitList)
        {
            if (nearestObj == null)
            {
                nearestObj = targetUnit.gameObject;
            }
            else
            {
                // 現在のnearestObjがtargetUnitより距離が近かったらそのままにし、targetUnitの方が近い場合、targetUnitを代入
                nearestObj = Vector3.Distance(nearestObj.transform.position, myPos) < Vector3.Distance(targetUnit.gameObject.transform.position, myPos) ? nearestObj : targetUnit.gameObject;
            }
        }

        return nearestObj;
    }

    // ターゲットが範囲内かどうか
    public static bool TargetInRange(Vector3 targetPos, Vector3 pos, float range)
    {
        return Vector3.Distance(targetPos, pos) < range;
    }
}
