using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


//DropAreaから保存された編成をMainシーンに引き継ぐためのスクリプトです
public class UnitDataCarrier:MonoBehaviour
{
    public static UnitDataCarrier Instance;

    public List<UnitStats> selectedUnits = new List<UnitStats>(); // ← 複数ユニット保持

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ユニット追加用メソッド（任意）
    public void AddUnit(UnitStats unit)
    {
        if (unit != null && !selectedUnits.Contains(unit))
        {
            selectedUnits.Add(unit);
        }
    }

}
