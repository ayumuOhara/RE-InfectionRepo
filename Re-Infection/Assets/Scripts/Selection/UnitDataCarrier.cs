using UnityEngine;
using System.Collections.Generic;

public class UnitDataCarrier : MonoBehaviour
{
    public static UnitDataCarrier Instance;

    public List<UnitStats> selectedUnits = new List<UnitStats>(); // 複数ユニット保持

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

    // ユニット追加用メソッド
    public void AddUnit(UnitStats unit)
    {
        if (unit != null && !selectedUnits.Contains(unit))
        {
            selectedUnits.Add(unit);
        }
    }

    // ユニットリストをクリア
    public void ClearUnits()
    {
        selectedUnits.Clear();
    }
}