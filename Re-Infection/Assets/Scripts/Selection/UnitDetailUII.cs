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
    public GameObject LayCastObj; //ステータス表示した際、ほかのボタンが触れないように
    public void Start()
    {
        UnitStatsObj.SetActive(false);
        LayCastObj.SetActive(false);
    }
    public void SetUnit(UnitStats stats)
    {
        UnitStatsObj.SetActive(true);
        unitImage.sprite = stats.unitSprite;
        LayCastObj.SetActive(true);

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
        LayCastObj.SetActive(false);
    }
}
