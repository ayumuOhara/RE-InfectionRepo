using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private ObjectPool player_pool;
    [SerializeField]
    private ObjectPool enemy_pool;

    public List<UnitBase> playerUnitList { get; private set; } = new List<UnitBase>();    // プレイヤーユニット格納リスト
    public List<UnitBase> enemyUnitList { get; private set; } = new List<UnitBase>();     // エネミーユニット格納リスト
    public List<EnemyUnit> corpseUnitList { get; private set; } = new List<EnemyUnit>();    // 死体ユニット格納リスト

    // プレイヤーユニットの数を返す
    public int PlayerCnt => playerUnitList.Count;

    // 敵の数を返す
    public int EnemyCnt => enemyUnitList.Count;

    // プレイヤーユニットがいないか返す
    public bool IsAllUnitDefeated => playerUnitList.Count <= 0;

    // 敵がいないか返す
    public bool IsAllEnemyDefeated => enemyUnitList.Count <= 0;

    public void OnDisable()
    {
        WaveLevel.OnSpawnUnit -= SpawnUnit;
        UnitIconClick.OnClickIcon -= SpawnUnit;
        CannonAttack.OnCloneUnit -= CloneUnit;
    }

    private void Awake()
    {
        WaveLevel.OnSpawnUnit += SpawnUnit;
        UnitIconClick.OnClickIcon += SpawnUnit;
        CannonAttack.OnCloneUnit += CloneUnit;
    }

    // 指定された場所にユニットをスポーン
    public void SpawnUnit(UnitStats stats, LayerMask unitLayer, Vector3 spawnPos)
    {
        var pool = LayerMask.LayerToName(unitLayer) == "PlayerUnit" ? player_pool : enemy_pool;

        UnitBase unit = pool.GetPooledObject().GetComponent<UnitBase>();
        unit.transform.position = spawnPos;
        unit.Initialize(stats);
    }

    // ユニットのクローン生成
    public void CloneUnit(UnitStats stats, Vector3 spawnPos)
    {
        var pool = enemy_pool;

        UnitBase unit = pool.GetPooledObject().GetComponent<UnitBase>();
        unit.transform.position = spawnPos;
        unit.Initialize(stats, true);

        Debug.Log("ユニットをクローン生成します");
    }

    public void AddPlayerUnitList(UnitBase unit)
    {
        playerUnitList.Add(unit);
    }

    public void AddEnemyUnitList(UnitBase unit)
    {
        enemyUnitList.Add(unit);
    }

    public void RemovePlayerUnitList(UnitBase unit)
    {
        playerUnitList.Remove(unit);
    }

    public void RemoveEnemyUnitList(UnitBase unit)
    {
        enemyUnitList.Remove(unit);
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
    public List<EnemyUnit> GetCorpseList()
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
                    unit.Release();

                playerUnitList.Clear();
                break;
            case "Enemy":
                foreach (var unit in enemyUnitList)
                    unit.Release();

                enemyUnitList.Clear();
                break;
        }
    }
}
