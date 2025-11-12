using UnityEngine;
using TMPro;
public class UnitStatsDislay:MonoBehaviour
{
    public UnitStats unitStats;
    public TextMeshProUGUI displayTMP;

    private void Start()
    {
        if (unitStats != null && displayTMP != null)
        {
            displayTMP.text = GetUnitStatsText(unitStats);
        }
    }

    string GetUnitStatsText(UnitStats stats)
    {
        return $"ユニット番号：{stats.unitId}   攻撃力：{stats.atk}\n" +
               $"ユニット名：{stats.unitName}   最大HP：{stats.maxHp}\n" +
               $"召喚コスト：{stats.summonCost}   射程距離：{stats.range}";
    }
}
