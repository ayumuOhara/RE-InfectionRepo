using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class UnitDetailUII : MonoBehaviour
{
    public GameObject UnitStatsObj;
    public Image unitImage;
    public TextMeshProUGUI unitNameText;
    public TextMeshProUGUI statsText;
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
        unitNameText.text = stats.unitName;
        LayCastObj.SetActive(true);

        statsText.text =
    $"HP：{stats.maxHp}\n" +
    $"攻撃力：{stats.atk}\n" +
    $"射程：{stats.range}\n" +
    $"攻撃速度：{stats.atkInterbal}\n" +
    $"攻撃範囲：{stats.radius}";

    }

    public void BackButtonClick()
    {
        UnitStatsObj.SetActive(false);
        LayCastObj.SetActive(false);
    }
}
