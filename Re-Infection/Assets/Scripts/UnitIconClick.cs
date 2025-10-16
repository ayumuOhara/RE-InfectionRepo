using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnitIconClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] UnitStats unitStats;
    [SerializeField] GameObject unitObj;

    CostManager costManager;

    Vector3 spawnPos = new Vector3(0, -1.5f, 0);  // プレイヤーユニットの生成座標

    void Start()
    {
        costManager = GameObject.Find("CostManager").GetComponent<CostManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (costManager.EnoughCost(unitStats.summonCost))
                GenerateUnit();
            else
                Debug.Log("コストが足りません");
        }
    }

    // ユニット生成
    void GenerateUnit()
    {
        costManager.RemoveCost(unitStats.summonCost);

        spawnPos.x = Random.Range(-1.7f, 1.7f);

        // ユニットオブジェクトを生成
        GameObject unit = Instantiate(unitObj, spawnPos, Quaternion.identity);

        // 対応するインデックスのユニットのステータスを渡す
        UnitController uc = unit.GetComponent<UnitController>();
        uc.SetUnitStats(unitStats, UnitGroup.Player);

        // ユニットを管理するUnitManagerに生成したユニットの陣営に対応するリストに格納
        UnitManager um = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        um.AddUnitList(unit, UnitGroup.Player);
    }
}
