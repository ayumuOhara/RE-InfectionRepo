using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
    [SerializeField] AudioClip summonSe;
    [SerializeField] AudioClip failedSe;
    GameManager gameManager;

    Vector3 spawnPos = new Vector3(0, -2.0f, 0);  // プレイヤーユニットの生成座標
    Vector2 defaltSize;

    void Awake()
    {
        //インスペクターで設定したサイズを保存
        defaltSize = unitIcon.rectTransform.sizeDelta;
    }
    void Start()
    {
        assertLabel.gameObject.SetActive(false);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //slotIndexが有効ならUnitDataCarrierから該当ユニットを取得
        if (UnitDataCarrier.Instance != null &&
    UnitDataCarrier.Instance.selectedUnits.Count > slotIndex &&
    UnitDataCarrier.Instance.selectedUnits[slotIndex] != null)
        {
            unitData = UnitDataCarrier.Instance.selectedUnits[slotIndex];

            Debug.Log($"Slot{slotIndex} に選択されたユニット: {unitData.unitStats.unitName}");
        }

        if (unitData != null && unitIcon != null)
        {
            unitIcon.sprite = unitData.unitStats.unitSprite;
            unitIcon.rectTransform.sizeDelta = defaltSize;
            jobIcon.sprite = unitData.unitStats.JobSprite;
        }

        unitCostText.text = unitData.unitStats.summonCost.ToString("F0");

        StartCoroutine(UnitCntText());
        StartCoroutine(ShortageCost());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!gameManager.timeManager.isPause && gameManager.waveSpawner.IsStartWave)
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (gameManager.costManager.EnoughCost(unitData.unitStats.summonCost))
                {
                    GetComponent<AudioSource>().PlayOneShot(summonSe);
                    GetComponent<Animator>().SetTrigger("Tap");
                    GenerateUnit();
                }
                else
                {
                    GetComponent<AudioSource>().PlayOneShot(failedSe);
                    GetComponent<Animator>().SetTrigger("Tap");

                    assertLabel.gameObject.SetActive(true);
                    assertLabel.GetComponent<Animator>().SetTrigger("Assert");
                }
            }
    }

    // ユニット生成
    void GenerateUnit()
    {
        gameManager.costManager.RemoveCost(unitData.unitStats.summonCost);

        spawnPos.x = Random.Range(-1.7f, 1.7f);

        // 対応するインデックスのユニットのステータスを渡す
        var unitObj = Instantiate(Resources.Load("PlayerUnit"), spawnPos, Quaternion.identity);
        UnitBase unit = unitObj.GetComponent<UnitBase>();
        unit.transform.position = spawnPos;
        unit.Initialize(unitData.unitStats);
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
            yield return null;
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


            yield return null;
        }
    }
}
