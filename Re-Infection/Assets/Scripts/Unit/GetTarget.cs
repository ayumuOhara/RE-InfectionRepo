using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.RuleTile.TilingRuleOutput;
using static UnityEditor.PlayerSettings;

public static class GetTarget
{
    // プレイヤーユニット取得
    public static GameObject GetPlayerUnit(Vector3 myPos)
    {
        UnitManager unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();

        if(unitManager.playerUnitList == null || unitManager.playerUnitList.Count == 0) return null; // 対象が取得できない場合、nullを返す
        
        return NearestUnit(unitManager.playerUnitList, myPos);
    }

    // エネミーユニット取得
    public static GameObject GetEnemyUnit(Vector3 myPos)
    {
        UnitManager unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();

        if (unitManager.enemyUnitList == null || unitManager.enemyUnitList.Count == 0) return null; // 対象が取得できない場合、nullを返す

        return NearestUnit(unitManager.enemyUnitList, myPos);
    }

    // 渡されたユニットリストから一番近い要素を返す
    public static GameObject NearestUnit(List<UnitBase> unitBases, Vector3 myPos)
    {
        GameObject nearestObj = null;

        foreach (UnitBase targetUnit in unitBases)
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

    // ターゲットが攻撃範囲内かどうか
    public static bool TargetInRange(Vector3 targetPos, Vector3 pos, float range)
    {
        return Vector3.Distance(targetPos, pos) < range;
    }

    // 近いほうのターゲットを返す
    public static GameObject NearestTarget(GameObject unit, GameObject castle, Vector3 myPos)
    {
        if(unit == null) return castle;

        return Vector3.Distance(unit.transform.position, myPos) < Vector3.Distance(castle.transform.position, myPos)
               ? unit : castle;
    }
}
