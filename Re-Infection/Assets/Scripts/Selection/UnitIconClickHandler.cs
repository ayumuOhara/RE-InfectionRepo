using UnityEngine;
using UnityEngine.UI;

public class UnitIconClickHandler : MonoBehaviour
{
    public DragIconController dragIcon; // アイコンのデータを参照するだけ
    public UnitDetailUII detailUI;

    public void OnClick()
    {
        detailUI.SetUnit(dragIcon.unitStats.unitStats);
    }
}