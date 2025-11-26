using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UnitManager : MonoBehaviour
{
    public List<UnitBase> playerUnitList { get; private set; } = new List<UnitBase>();    // プレイヤーユニット格納リスト
    public List<UnitBase> enemyUnitList { get; private set; } = new List<UnitBase>();     // エネミーユニット格納リスト
    public List<UnitBase> corpseUnitList { get; private set; } = new List<UnitBase>();    // 死体ユニット格納リスト

    // プレイヤーユニットの数を返す
    public int PlayerCnt => playerUnitList.Count;

    // 敵の数を返す
    public int EnemyCnt => enemyUnitList.Count;

    // プレイヤーユニットがいないか返す
    public bool IsAllUnitDefeated => playerUnitList.Count <= 0;

    // 敵がいないか返す
    public bool IsAllEnemyDefeated => enemyUnitList.Count <= 0;

    // ユニットをリストに追加
    public void AddUnitList(UnitBase unit)
    {
        switch (unit)
        {
            case PlayerUnit:
                playerUnitList.Add(unit); break;
            case EnemyUnit: 
                enemyUnitList.Add(unit);  break;
        }
    }

    // ユニットをリストから削除
    public void RemoveUnitList(UnitBase unit)
    {
        switch (unit)
        {
            case PlayerUnit:
                playerUnitList.Remove(unit); break;
            case EnemyUnit:
                enemyUnitList.Remove(unit);  break;
        }
    }

    // 指定された味方ユニットの数を返す
    public int GetUnitCnt(UnitStats stats)
    {
        return playerUnitList.Count(unit => unit.Stats.unitName == stats.unitName);
    }

    // 死体リストに追加
    public void AddCorpseList(EnemyUnit unit)
    {
        corpseUnitList.Add(unit);
    }

    // 死体リストから削除
    public void RemoveCorpseList(EnemyUnit unit)
    {
        corpseUnitList.Remove(unit);
    }

    // 死体のリストを返す
    public List<UnitBase> GetCorpseList()
    {
        return corpseUnitList;
    }

    // ユニットを全て削除
    public void AllUnitDestroy(string tag)
    {
        switch (tag)
        {
            case "Player":
                foreach (var unit in playerUnitList)
                    Destroy(unit.gameObject);

                playerUnitList.Clear();
                break;
            case "Enemy":
                foreach (var unit in enemyUnitList)
                    Destroy(unit.gameObject);

                enemyUnitList.Clear();
                break;
        }
    }
}
