using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnitIconClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] UnitStats unitStats;
    [SerializeField] GameObject unitObj;

    GameManager gameManager;

    Vector3 spawnPos = new Vector3(0, -1.0f, 0);  // プレイヤーユニットの生成座標

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!gameManager.timeManager.isPause)
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (gameManager.costManager.EnoughCost(unitStats.summonCost))
                    GenerateUnit();
                else
                    Debug.Log("コストが足りません");
            }
    }

    // ユニット生成
    void GenerateUnit()
    {
        gameManager.costManager.RemoveCost(unitStats.summonCost);

        spawnPos.x = Random.Range(-1.7f, 1.7f);

        // ユニットオブジェクトを生成
        GameObject unit = Instantiate(unitObj, spawnPos, Quaternion.identity);

        // 対応するインデックスのユニットのステータスを渡す
        UnitController uc = unit.GetComponent<UnitController>();
        uc.SetUnitStats(unitStats, UnitGroup.Player);
    }
}
