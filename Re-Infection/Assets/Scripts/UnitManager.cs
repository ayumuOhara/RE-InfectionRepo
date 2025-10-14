using UnityEngine;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{
    public List<GameObject> playerUnitList { get; private set; } = new List<GameObject>();    // プレイヤーユニット格納リスト
    public List<GameObject> enemyUnitList { get; private set; } = new List<GameObject>();     // エネミーユニット格納リスト

    // ユニットをリストに追加
    public void AddUnitList(GameObject unitObj, UnitGroup group)
    {
        if(group == UnitGroup.Player)
            playerUnitList.Add(unitObj);
        if(group == UnitGroup.Enemy)
            enemyUnitList.Add(unitObj);
    }

    // ユニットをリストから削除
    public void RemoveUnitList(GameObject unitObj, UnitGroup group)
    {
        if (group == UnitGroup.Player)
            playerUnitList.Remove(unitObj);
        if (group == UnitGroup.Enemy)
            enemyUnitList.Remove(unitObj);
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
