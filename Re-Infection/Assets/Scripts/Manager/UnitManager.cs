using UnityEngine;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;

public class UnitManager : MonoBehaviour
{
    public List<UnitController> playerUnitList { get; private set; } = new List<UnitController>();    // プレイヤーユニット格納リスト
    public List<UnitController> enemyUnitList { get; private set; } = new List<UnitController>();     // エネミーユニット格納リスト
    public List<UnitController> corpseUnitList { get; private set; } = new List<UnitController>();    // 死体ユニット格納リスト

    // プレイヤーユニットの数を返す
    public int PlayerCnt => playerUnitList.Count;

    // 敵の数を返す
    public int EnemyCnt => enemyUnitList.Count;

    // プレイヤーユニットがいないか返す
    public bool IsAllUnitDefeated => playerUnitList.Count <= 0;

    // 敵がいないか返す
    public bool IsAllEnemyDefeated => enemyUnitList.Count <= 0;

    // ユニットをリストに追加
    public void AddUnitList(UnitController unit, UnitGroup group)
    {
        if(group == UnitGroup.Player)
            playerUnitList.Add(unit);
        if(group == UnitGroup.Enemy)
            enemyUnitList.Add(unit);
    }

    // ユニットをリストから削除
    public void RemoveUnitList(UnitController unit, UnitGroup group)
    {
        if (group == UnitGroup.Player)
            playerUnitList.Remove(unit);
        if (group == UnitGroup.Enemy)
            enemyUnitList.Remove(unit);
    }

    // ユニットのリストを返す
    public List<UnitController> GetUnitList(UnitGroup group)
    {
        return group == UnitGroup.Player ? playerUnitList : enemyUnitList;
    }

    // 指定された味方ユニットの数を返す
    public int GetUnitCnt(UnitStats stats)
    {
        var cnt = 0;

        foreach (var unit in playerUnitList)
        {
            if (unit.unitName == stats.unitName)
            {
                cnt++;
            }
        }

        return cnt;
    }

    // 死体リストに追加
    public void AddCorpseList(UnitController unit)
    {
        corpseUnitList.Add(unit);
    }

    // 死体リストから削除
    public void RemoveCorpseList(UnitController unit)
    {
        corpseUnitList.Remove(unit);
    }

    // 死体のリストを返す
    public List<UnitController> GetCorpseList()
    {
        return corpseUnitList;
    }

    // プレイヤーのユニットを全て除外
    public void AllPlayerUnitDestroy()
    {
        foreach (UnitController unit in playerUnitList)
            Destroy(unit.gameObject);

        playerUnitList.Clear();
    }

    // エネミーのユニットを全て除外
    public void AllEnemyUnitDestroy()
    {
        foreach (UnitController unit in enemyUnitList)
            Destroy(unit.gameObject);

        enemyUnitList.Clear();
    }
}
