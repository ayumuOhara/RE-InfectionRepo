using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class UnitIconClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public UnitStatsData unitData;
    [SerializeField] public int slotIndex;
    [SerializeField] Image unitIcon;
    [SerializeField] Image jobIcon;
    [SerializeField] TextMeshProUGUI unitCostText;
    [SerializeField] TextMeshProUGUI unitCntText;
    [SerializeField] Image assertLabel;
    GameManager gameManager;
    SEManager seManager;

    Vector3 spawnPos = new Vector3(0, -2.0f, 0);  // プレイヤーユニットの生成座標
    Vector2 defaltSize;

    public static event Action<UnitStats, LayerMask, Vector3> OnClickIcon;

    void Awake()
    {
        seManager = FindObjectOfType<SEManager>();

        //インスペクターで設定したサイズを保存
        defaltSize = unitIcon.rectTransform.sizeDelta;

        if(assertLabel == null)
            assertLabel = GameObject.Find("AssertLabel").GetComponent<Image>();
    }

    void Start()
    {
        assertLabel.gameObject.SetActive(false);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        var unit = UnitDataCarrier.Instance.GetUnitofSlotIndex(slotIndex);

        //slotIndexが有効ならUnitDataCarrierから該当ユニットを取得
        if(unit != null)
        {
            unitData = unit;

            Debug.Log($"Slot{slotIndex} に選択されたユニット: {unitData.unitStats.unitName}");
        }

        if (unitData != null && unitIcon != null)
        {
            unitIcon.sprite = unitData.unitStats.unitSprite;
            unitIcon.rectTransform.sizeDelta = defaltSize;
            jobIcon.sprite = unitData.unitStats.JobSprite;

            unitCostText.text = unitData.unitStats.summonCost.ToString("F0");

            StartCoroutine(UnitCntText());
            StartCoroutine(ShortageCost());
        }
        else
        {
            unitIcon.enabled = false;
            jobIcon.enabled = false;
            unitCostText.enabled = false;
            unitCntText.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (unitData == null) return;

        if(!gameManager.timeManager.isPause && gameManager.waveSpawner.IsStartWave)
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (gameManager.costManager.EnoughCost(unitData.unitStats.summonCost))
                {
                    seManager.PlaySE(SEManager.SEType.Summon);
                    GetComponent<Animator>().SetTrigger("Tap");

                    GenerateUnit();
                }
                else
                {
                    seManager.PlaySE(SEManager.SEType.SummonFailed);
                    GetComponent<Animator>().SetTrigger("Tap");

                    assertLabel.gameObject.SetActive(true);
                    assertLabel.GetComponent<Animator>().SetTrigger("Assert");
                }
            }
    }

    // ユニット生成
    void GenerateUnit()
    {
        CostManager.onRemoveCost?.Invoke(unitData.unitStats.summonCost);
        spawnPos.x = UnityEngine.Random.Range(-1.7f, 1.7f);

        // LayerMask
        // 6 == PlayerUnit
        OnClickIcon?.Invoke(unitData.unitStats, 6, spawnPos);
    }

    // ユニットの数を表示
    IEnumerator UnitCntText()
    {
        var cnt = 0;
        
        while (true)
        {
            yield return new WaitUntil(() => cnt < gameManager.unitManager.GetUnitCnt(unitData.unitStats)
                                          || cnt > gameManager.unitManager.GetUnitCnt(unitData.unitStats));

            cnt = gameManager.unitManager.GetUnitCnt(unitData.unitStats);
            unitCntText.text = cnt + " 体";
        }
    }

    // ユニットの数を表示
    IEnumerator ShortageCost()
    {
        var cnt = unitData.unitStats.summonCost;

        while (true)
        {
            if (!gameManager.costManager.EnoughCost(unitData.unitStats.summonCost))
            {
                unitCostText.color = Color.orangeRed;
                unitIcon.color = Color.gray4;
            }
            else
            {
                unitCostText.color = Color.white;
                unitIcon.color = Color.white;
            }

            yield return new WaitUntil(() => cnt < gameManager.costManager.currentCost
                                          || cnt > gameManager.costManager.currentCost);
        }
    }
}
