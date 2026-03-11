using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropArea : MonoBehaviour, IDropHandler
{
    [SerializeField] public Transform dropTargetParent;
    public UnitStatsData currentUnitStats;
    public int slotIndex;
    [SerializeField] private UnitStatsData defaultUnit;

    private void Start()
    {
        UnitStatsData saved = UnitDataCarrier.Instance.GetUnitofSlotIndex(slotIndex);

        if (saved != null)
        {
            currentUnitStats = saved;
            CreateCloneFromExistingIcon(saved);
            UpdateAllCheckImage();
            return;
        }

        if (defaultUnit != null)
        {
            currentUnitStats = defaultUnit;
            UnitDataCarrier.Instance.SetUnitofSlotIndex(defaultUnit, slotIndex);

            CreateCloneFromExistingIcon(defaultUnit);
            UpdateAllCheckImage();
        }
    }

    //タップ編成・ドラッグ編成共通処理
    public void SetUnitFromList(DragIconController fromList)
    {
        if (dropTargetParent.childCount > 0)
            Destroy(dropTargetParent.GetChild(0).gameObject);

        currentUnitStats = fromList.unitStats;
        fromList.isUsedInDropArea = true;
        //fromList.SetDraggable(false);

        UnitDataCarrier.Instance.SetUnitofSlotIndex(currentUnitStats, slotIndex);

        CreateClone(fromList.gameObject);
        UpdateAllCheckImage();
    }

    //空きスロットを slotIndex 昇順で探す
    public static DropArea GetFirstEmptySlot()
    {
        DropArea[] areas = FindObjectsOfType<DropArea>();
        System.Array.Sort(areas, (a, b) => a.slotIndex.CompareTo(b.slotIndex));

        foreach (var area in areas)
        {
            if (area.currentUnitStats == null)
                return area;
        }

        return null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        DragIconController fromList = eventData.pointerDrag.GetComponent<DragIconController>();
        DropAreaIconDrag fromDropArea = eventData.pointerDrag.GetComponent<DropAreaIconDrag>();

        //リスト → DropArea
        if (fromList != null)
        {
            SetUnitFromList(fromList);
            return;
        }

        //DropArea → DropArea（clone 移動）
        if (fromDropArea != null)
        {
            DropArea oldArea = fromDropArea.originalDropArea;

            if (oldArea != null && oldArea != this)
            {
                oldArea.currentUnitStats = null;
                UnitDataCarrier.Instance.SetUnitofSlotIndex(null, fromDropArea.slotIndex);
            }

            if (dropTargetParent.childCount > 0)
                Destroy(dropTargetParent.GetChild(0).gameObject);

            eventData.pointerDrag.transform.SetParent(dropTargetParent);
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector2(53, -49);

            currentUnitStats = fromDropArea.unitStats;
            UnitDataCarrier.Instance.SetUnitofSlotIndex(currentUnitStats, slotIndex);

            fromDropArea.originalDropArea = this;
            fromDropArea.slotIndex = slotIndex;

            UpdateAllCheckImage();
        }
    }

    private void CreateClone(GameObject original)
    {
        GameObject clone = Instantiate(original, dropTargetParent);
        RectTransform rt = clone.GetComponent<RectTransform>();
        rt.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        //rt.anchoredPosition = new Vector2(53f, -49f);

        var d = clone.GetComponent<DragIconController>();

        Destroy(d.CheckImage);
        d.levelText.enabled = false;

        // DragIconController を削除
        Destroy(clone.GetComponent<DragIconController>());
        foreach (var comp in clone.GetComponentsInChildren<DragIconController>())
            Destroy(comp);

        // CanvasGroup（Raycast 有効）
        CanvasGroup cg = clone.GetComponent<CanvasGroup>();
        if (cg == null) cg = clone.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = true;
        cg.interactable = true;
        cg.alpha = 1f;

        // DropAreaIconDrag を付ける
        DropAreaIconDrag dragScript = clone.AddComponent<DropAreaIconDrag>();
        dragScript.slotIndex = slotIndex;
        dragScript.unitStats = currentUnitStats;
        dragScript.originalDropArea = this;
    }

    private void CreateCloneFromExistingIcon(UnitStatsData stats)
    {
        DragIconController[] icons = FindObjectsOfType<DragIconController>();
        DragIconController source = null;

        foreach (var icon in icons)
        {
            if (icon.unitStats == stats)
            {
                source = icon;
                break;
            }
        }

        if (source == null)
        {
            Debug.LogError("復元対象の DragIcon が見つかりません");
            return;
        }

        GameObject clone = Instantiate(source.gameObject, dropTargetParent);

        var d = clone.GetComponent<DragIconController>();

        Destroy(d.CheckImage);
        d.levelText.enabled = false;

        Destroy(clone.GetComponent<DragIconController>());
        foreach (var comp in clone.GetComponentsInChildren<DragIconController>())
            Destroy(comp);

        RectTransform rt = clone.GetComponent<RectTransform>();
        rt.localScale = new Vector3(0.75f, 0.75f, 0.75f);

        CanvasGroup cg = clone.GetComponent<CanvasGroup>();
        if (cg == null) cg = clone.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = true;

        DropAreaIconDrag dragScript = clone.AddComponent<DropAreaIconDrag>();
        dragScript.slotIndex = slotIndex;
        dragScript.unitStats = stats;
        dragScript.originalDropArea = this;
        //dragScript.SetOriginalPos();
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

            icon.isUsedInDropArea = isUsed;
            //icon.SetDraggable(!isUsed);
            icon.CheckObj(isUsed);
        }
    }
}