using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UnitDataCarrier : MonoBehaviour
{
    public static UnitDataCarrier Instance;

    [SerializeField] private List<UnitStatsData> playableUnits;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも残す
        }
        else
        {
            Destroy(gameObject); // 重複防止
        }
    }

    // アンロック時に再生するユニットのアニメーションのキーを取得
    public void SetUnitofSlotIndex(UnitStatsData data, int slotIdx)
    {
        if (data == null)
        {
            PlayerPrefs.DeleteKey($"UnitSlot{slotIdx}");
            return;
        }

        if(data?.unitStats == null || !data.unitStats.IsUnitUnlocked()) return;

        PlayerPrefs.SetString($"UnitSlot{slotIdx}", data.unitStats.unitName);
    }

    public UnitStatsData GetUnitofSlotIndex(int slotIdx)
    {
        string unitName = PlayerPrefs.GetString($"UnitSlot{slotIdx}");

        if (string.IsNullOrEmpty(unitName)) return null;

        var unit = playableUnits.Where(u => u.unitStats.unitName == unitName)?.FirstOrDefault();

        if(unit == null) return null;

        // アンロック済みでなければnullを返す
        if(unit.unitStats.IsUnitUnlocked())
            return unit;
        else
            return null;
    }
}