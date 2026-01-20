using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropArea : MonoBehaviour, IDropHandler
{
    [SerializeField] private Transform dropTargetParent;
    public UnitStatsData currentUnitStats;
    public int slotIndex;

  
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        DragIconController fromList = dropped.GetComponent<DragIconController>();
        DropAreaIconDrag fromDropArea = dropped.GetComponent<DropAreaIconDrag>();

        // ▼ selectedUnits を slotIndex まで拡張（どのケースでも必ず必要）
        while (UnitDataCarrier.Instance.selectedUnits.Count <= slotIndex)
        {
            UnitDataCarrier.Instance.selectedUnits.Add(null);
        }

        // DragIconController → DropArea のときだけ重複チェック
        if (fromList != null)
        {
            UnitStatsData incoming = fromList.unitStats;

            // 他の DropArea に同じユニットが入っていたら拒否
            foreach (var da in FindObjectsOfType<DropArea>())
            {
                if (da != this && da.currentUnitStats == incoming)
                {
                    return; // 重複禁止
                }
            }
        }

        
        //  既存の Clone があれば削除（上書き用）
        if (dropTargetParent.childCount > 0)
        {
            Destroy(dropTargetParent.GetChild(0).gameObject);
            currentUnitStats = null;

            while (UnitDataCarrier.Instance.selectedUnits.Count <= slotIndex)
                UnitDataCarrier.Instance.selectedUnits.Add(null);

            UnitDataCarrier.Instance.selectedUnits[slotIndex] = null;
        }

       
        //  DragIconController → DropArea（新規登録）
        if (fromList != null)
        {
            currentUnitStats = fromList.unitStats;

            while (UnitDataCarrier.Instance.selectedUnits.Count <= slotIndex)
                UnitDataCarrier.Instance.selectedUnits.Add(null);

            UnitDataCarrier.Instance.selectedUnits[slotIndex] = currentUnitStats;

            CreateClone(dropped);
            UpdateAllCheckImage();
            return;
        }

     
        //  DropAreaIconDrag → DropArea（Clone を新しく作らず移動）

        if (fromDropArea != null)
        {
            // 元の DropArea のデータを消す
            DropArea oldArea = fromDropArea.originalDropArea;
            if (oldArea != null && oldArea != this)
            {
                oldArea.currentUnitStats = null;
                UnitDataCarrier.Instance.selectedUnits[fromDropArea.slotIndex] = null;
            }

            // Clone を移動
            dropped.transform.SetParent(dropTargetParent);
            dropped.GetComponent<RectTransform>().anchoredPosition = new Vector2(53, -49);

            // データ更新
            currentUnitStats = fromDropArea.unitStats;
            UnitDataCarrier.Instance.selectedUnits[slotIndex] = currentUnitStats;

           
            fromDropArea.originalDropArea = this;

            // 移動成功
            fromDropArea.droppedSuccessfully = true;
            fromDropArea.slotIndex = slotIndex;

            UpdateAllCheckImage();
            return;
        }


    }

    private void CreateClone(GameObject original)
    {
        GameObject clone = Instantiate(original, dropTargetParent);
        RectTransform rt = clone.GetComponent<RectTransform>();

        // Clone の初期位置
        rt.anchoredPosition = new Vector2(53f, -49f);

        
        // DragIconController を削除
        Destroy(clone.GetComponent<DragIconController>());
        foreach (var comp in clone.GetComponentsInChildren<DragIconController>())
            Destroy(comp);

        // ★ CanvasGroup を必ず付ける（これが無いとドラッグできない）
        CanvasGroup cg = clone.GetComponent<CanvasGroup>();
        if (cg == null) cg = clone.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = true;   // ← DropAreaIconDrag が OnBeginDrag で false にする
        cg.interactable = true;
        cg.alpha = 1f;

        // DropAreaIconDrag を付ける
        DropAreaIconDrag dragScript = clone.AddComponent<DropAreaIconDrag>();
        dragScript.slotIndex = slotIndex;
        dragScript.unitStats = currentUnitStats;
        dragScript.SetOriginalPos();

        // ★ Clone の CheckImage を非表示にする
        var checkImages = clone.GetComponentsInChildren<Image>(true);
        bool found = false;

        foreach (var img in checkImages)
        {
            if (img.gameObject.name == "CheckImage")
            {
                img.enabled = false;
                found = true;
                  }
        }

       
    }

    public static bool IsUnitInAnyDropArea(UnitStatsData target)
    {
        DropArea[] areas = FindObjectsOfType<DropArea>();
        foreach (var da in areas)
        {
            if (da.currentUnitStats == target)
                return true;
        }
        return false;
    }

    public static void UpdateAllCheckImage()
    {
        DropArea[] dropAreas = FindObjectsOfType<DropArea>();
        DragIconController[] icons = FindObjectsOfType<DragIconController>();

        foreach (var icon in icons)
        {
            bool isUsed = false;

            foreach (var da in dropAreas)
            {
                if (da.currentUnitStats == icon.unitStats)
                {
                    isUsed = true;
                    break;
                }
            }

            icon.CheckObj(isUsed);
        }
    }

    public void CreateUnit()
    {
       //DragIconControllerの制限解除
       foreach(var icon in FindObjectsOfType<DragIconController>())
        {
            if (icon.unitStats == currentUnitStats)
            {
                icon.isUsedInDropArea = false;
                icon.CheckObj(false);
            }
        }
        //データ削除
        UnitDataCarrier.Instance.selectedUnits[slotIndex] = null;

        currentUnitStats = null;
    }
}