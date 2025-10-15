using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnitIconClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] UnitData unitData;
    [SerializeField] int unitIdx;
    [SerializeField] GameObject unitObj;

    Vector3 playerUnitGenaretePos = new Vector3(0, -1.5f, 0);  // プレイヤーユニットの生成座標

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GenerateUnit();
        }
    }

    // ユニット生成
    void GenerateUnit()
    {
        var rndXpos = Random.Range(-1.7f, 1.7f);

        // ユニットオブジェクトを生成
        GameObject unit = null;
        playerUnitGenaretePos.x = rndXpos;
        unit = Instantiate(unitObj, playerUnitGenaretePos, Quaternion.identity);

        // 対応するインデックスのユニットのステータスを渡す
        UnitController uc = unit.GetComponent<UnitController>();
        uc.SetUnitStats(unitData.stats[unitIdx], UnitGroup.Player);

        // ユニットを管理するUnitManagerに生成したユニットの陣営に対応するリストに格納
        UnitManager um = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        um.AddUnitList(unit, UnitGroup.Player);
    }
}
