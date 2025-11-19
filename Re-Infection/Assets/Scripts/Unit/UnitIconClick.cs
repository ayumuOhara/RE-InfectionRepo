using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnitIconClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public UnitStats unitStats;
    [SerializeField] GameObject unitObj;
    [SerializeField] public int slotIndex;
    [SerializeField] Image iconImage;
    GameManager gameManager;

    Vector3 spawnPos = new Vector3(0, -1.0f, 0);  // プレイヤーユニットの生成座標

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //slotIndexが有効ならUnitDataCarrierから該当ユニットを取得
        if(UnitDataCarrier.Instance!=null&&
            UnitDataCarrier.Instance.selectedUnits.Count>slotIndex&&
            UnitDataCarrier.Instance.selectedUnits[slotIndex] != null)
        {
            unitStats = UnitDataCarrier.Instance.selectedUnits[slotIndex];

            Debug.Log($"Slot{slotIndex}に選択されたユニット:{unitStats.unitName}");
        }

        if (unitStats != null && iconImage != null)
        {
            iconImage.sprite = unitStats.unitSprite;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!gameManager.timeManager.isPause && gameManager.waveSpawner.IsStartWave)
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

        // 対応するインデックスのユニットのステータスを渡す
        UnitController uc = Instantiate(unitObj, spawnPos, Quaternion.identity).GetComponent<UnitController>();
        uc.transform.position = spawnPos;
        uc.SetUnitStats(unitStats, UnitGroup.Player);
    }
}
