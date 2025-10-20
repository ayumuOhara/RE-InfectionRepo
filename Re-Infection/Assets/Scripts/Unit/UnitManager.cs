using UnityEngine;
using System.Collections.Generic;

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
    public void AddUnitList(UnitController unitObj, UnitGroup group)
    {
        if(group == UnitGroup.Player)
            playerUnitList.Add(unitObj);
        if(group == UnitGroup.Enemy)
            enemyUnitList.Add(unitObj);
    }

    // ユニットをリストから削除
    public void RemoveUnitList(UnitController unitObj, UnitGroup group)
    {
        if (group == UnitGroup.Player)
            playerUnitList.Remove(unitObj);
        if (group == UnitGroup.Enemy)
            enemyUnitList.Remove(unitObj);
    }

    // ユニットのリストを返す
    public List<UnitController> GetUnitList(UnitGroup group)
    {
        return group == UnitGroup.Player ? playerUnitList : enemyUnitList;
    }

    // 死体リストに追加
    public void AddCorpseList(UnitController unitObj)
    {
        corpseUnitList.Add(unitObj);
    }

    // 死体リストから削除
    public void RemoveCorpseList(UnitController unitObj)
    {
        corpseUnitList.Remove(unitObj);
    }

    // 死体のリストを返す
    public List<UnitController> GetCorpseList()
    {
        return corpseUnitList;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
