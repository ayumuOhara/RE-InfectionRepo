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

        // ▼ DragIconController → DropArea（一覧から来た）
        if (fromList != null)
        {
            // 重複チェック
            if (IsUnitInAnyDropArea(fromList.unitStats))
                return;

            // リスト拡張
            while (UnitDataCarrier.Instance.selectedUnits.Count <= slotIndex)
                UnitDataCarrier.Instance.selectedUnits.Add(null);

            currentUnitStats = fromList.unitStats;
            fromList.isUsedInDropArea = true;

            // 既存 Clone 削除
            if (dropTargetParent.childCount > 0)
            {
                CreateUnit();
                Destroy(dropTargetParent.GetChild(0).gameObject);
            }
            // Clone 生成
            CreateClone(dropped);

            UnitDataCarrier.Instance.selectedUnits[slotIndex] = currentUnitStats;

            UpdateAllCheckImage();
            return;
        }

        // ▼ DropAreaIconDrag → DropArea（DropArea 内の Clone を移動）
        if (fromDropArea != null)
        {
            // 重複チェック（自分自身は OK）
            if (fromDropArea.slotIndex != slotIndex &&
                IsUnitInAnyDropArea(fromDropArea.unitStats))
                return;

            // リスト拡張
            while (UnitDataCarrier.Instance.selectedUnits.Count <= slotIndex)
                UnitDataCarrier.Instance.selectedUnits.Add(null);

            // 既存 Clone 削除
            if (dropTargetParent.childCount > 0)
                Destroy(dropTargetParent.GetChild(0).gameObject);

            // Clone を移動
            dropped.transform.SetParent(dropTargetParent);
            dropped.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            fromDropArea.slotIndex = slotIndex;
            UnitDataCarrier.Instance.selectedUnits[slotIndex] = fromDropArea.unitStats;

            fromDropArea.droppedSuccessfully = true;

            UpdateAllCheckImage();
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