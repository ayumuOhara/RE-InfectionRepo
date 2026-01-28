using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class UnitDetailUII : MonoBehaviour
{
    public GameObject UnitStatsObj;
    public Image unitImage;

    [Header("UnitStats")]
    public TextMeshProUGUI unitNameText; //–¼‘O
    public TextMeshProUGUI unitHpText; //HP
    public TextMeshProUGUI atkText; //UŒ‚—Í
    public TextMeshProUGUI rangeText; //Ë’ö
    public TextMeshProUGUI atkInterbalText; //UŒ‚‘¬“x
    public TextMeshProUGUI hitCntText; //ƒqƒbƒg‚·‚é”
   
    public void Start()
    {
        UnitStatsObj.SetActive(false);
      
    }
    public void SetUnit(UnitStats stats)
    {
        UnitStatsObj.SetActive(true);
        unitImage.sprite = stats.unitSprite;

        unitNameText.text = stats.unitName;
        unitHpText.text = $"{stats.maxHp}";
        atkText.text = $"{stats.atk}";
        rangeText.text = $"{stats.range}";
        atkInterbalText.text = $"{stats.atkInterbal}";
        hitCntText.text = $"{stats.hitCnt}";

    }

    public void BackButtonClick()
    {
        UnitStatsObj.SetActive(false);
  
    }
}
