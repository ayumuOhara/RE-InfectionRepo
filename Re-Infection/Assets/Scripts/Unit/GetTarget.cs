using UnityEngine;
using System.Collections.Generic;

public static class GetTarget
{
    // 最も近い敵対ユニット取得
    public static GameObject GetNearestTargetUnit(UnitBase unit)
    {
        UnitManager unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();

        switch (unit.TargetLayerStr)
        {
            case "PlayerUnit":
                return NearestUnit(unitManager.playerUnitList, unit.MyPos);
            case "EnemyUnit":
                return NearestUnit(unitManager.enemyUnitList, unit.MyPos);
            default:
                return null;
        }
    }

    // 最も遠い敵対ユニット取得
    public static GameObject GetFarthestTargetUnit(UnitBase unit)
    {
        UnitManager unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();

        switch (unit.TargetLayerStr)
        {
            case "PlayerUnit":
                return FarthestUnit(unitManager.playerUnitList, unit.MyPos);
            case "EnemyUnit":
                return FarthestUnit(unitManager.enemyUnitList, unit.MyPos);
            default:
                return null;
        }
    }

    // 渡されたユニットリストから一番近い要素を返す
    static GameObject NearestUnit(List<UnitBase> unitBases, Vector3 myPos)
    {
        if(unitBases.Count <= 0 || unitBases == null) return null;

        GameObject nearestObj = null;

        foreach (UnitBase targetUnit in unitBases)
        {
            if (targetUnit.IsDead) continue;

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

    // 渡されたユニットリストから一番遠い要素を返す
    static GameObject FarthestUnit(List<UnitBase> unitBases, Vector3 myPos)
    {
        if (unitBases.Count <= 0 || unitBases == null) return null;

        GameObject farthestObj = null;

        foreach (UnitBase targetUnit in unitBases)
        {
            if (targetUnit.IsDead) continue;

            if (farthestObj == null)
            {
                farthestObj = targetUnit.gameObject;
            }
            else
            {
                // 現在のfarthestObjがtargetUnitより距離が遠かったらそのままにし、targetUnitの方が遠い場合、targetUnitを代入
                farthestObj = Vector3.Distance(farthestObj.transform.position, myPos) > Vector3.Distance(targetUnit.gameObject.transform.position, myPos) ? farthestObj : targetUnit.gameObject;
            }
        }

        return farthestObj;
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
