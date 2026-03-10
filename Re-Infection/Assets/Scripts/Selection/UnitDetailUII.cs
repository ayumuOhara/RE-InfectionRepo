using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class UnitDetailUII : MonoBehaviour
{
    public GameObject UnitStatsObj;
    public Image unitImage;

    [Header("UnitStats")]
    public TextMeshProUGUI unitNameText; //名前
    public TextMeshProUGUI unitHpText; //HP
    public TextMeshProUGUI atkText; //攻撃力
    public TextMeshProUGUI rangeText; //射程
    public TextMeshProUGUI atkInterbalText; //攻撃速度
    public TextMeshProUGUI hitCntText; //ヒットする数
    public TextMeshProUGUI descriptionText;

    [Header("初期表示するユニット")]
    public UnitStatsData defaltUnit;
    public void Start()
    {
        if (defaltUnit != null)
        {
            SetUnit(defaltUnit.unitStats);
        }
      
    }
    public void SetUnit(UnitStats stats)
    {
        UnitStatsObj.SetActive(true);
        unitImage.sprite = stats.unitSprite;

        unitNameText.text = stats.unitName;
        unitHpText.text = $"{stats.GetCurrentLevelMaxHp()}";
        atkText.text = $"{stats.GetCurrentLevelAtk()}";
        rangeText.text = $"{stats.range}";
        atkInterbalText.text = $"{stats.atkInterbal}";
        hitCntText.text = $"{stats.hitCnt}";
        descriptionText.text = $"{stats.unitDescription}";
    }

}
