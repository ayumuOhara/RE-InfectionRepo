using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropArea : MonoBehaviour, IDropHandler
{
    [SerializeField] private Transform dropTargetParent;
    public UnitStatsData currentUnitStats;
    public int slotIndex;
    [SerializeField] private UnitStatsData defaultUnit;

    private void Start()
    {
        // ▼ selectedUnits を slotIndex まで拡張
        while (UnitDataCarrier.Instance.selectedUnits.Count <= slotIndex)
            UnitDataCarrier.Instance.selectedUnits.Add(null);

        // ▼ すでに保存されているデータがある場合（シーン復元）
        UnitStatsData saved = UnitDataCarrier.Instance.selectedUnits[slotIndex];

        if (saved != null)
        {
            currentUnitStats = saved;
            CreateCloneFromExistingIcon(saved);

            MarkDragIconAsUsed(saved);

            UpdateAllCheckImage();
            return;
        }

        // ▼ 保存データが無い → defaultUnit を初期値として使う
        if (defaultUnit != null)
        {
            currentUnitStats = defaultUnit;
            UnitDataCarrier.Instance.selectedUnits[slotIndex] = defaultUnit;

            CreateCloneFromExistingIcon(defaultUnit);

            MarkDragIconAsUsed(defaultUnit);

            UpdateAllCheckImage();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        GameObject dropped = eventData.pointerDrag;
        DragIconController fromList = dropped.GetComponent<DragIconController>();
        DropAreaIconDrag fromDropArea = dropped.GetComponent<DropAreaIconDrag>();

        // selectedUnits サイズ保証
        while (UnitDataCarrier.Instance.selectedUnits.Count <= slotIndex)
            UnitDataCarrier.Instance.selectedUnits.Add(null);

        // ▼ DragIconController → DropArea（リストからの新規 or 上書き）
        if (fromList != null)
        {
            // 古い Clone があるなら解除
            if (dropTargetParent.childCount > 0)
            {
                var oldClone = dropTargetParent.GetChild(0);
                var oldDrag = oldClone.GetComponent<DropAreaIconDrag>();

                if (oldDrag != null)
                {
                    foreach (var icon in FindObjectsOfType<DragIconController>())
                    {
                        if (icon.unitStats == oldDrag.unitStats)
                        {
                            icon.isUsedInDropArea = false;
                            icon.SetDraggable(true);
                            icon.CheckObj(false);
                        }
                    }
                }

                Destroy(oldClone.gameObject);
            }

            // 新しいユニット登録
            currentUnitStats = fromList.unitStats;
            fromList.isUsedInDropArea = true;
            fromList.SetDraggable(false);

            UnitDataCarrier.Instance.selectedUnits[slotIndex] = currentUnitStats;

            CreateClone(dropped);
            UpdateAllCheckImage();
            return;
        }

        // ▼ DropAreaIconDrag → DropArea（Clone の移動）※1回だけ
        if (fromDropArea != null)
        {
            DropArea oldArea = fromDropArea.originalDropArea;

            // 元の DropArea のデータを消す
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

        rt.anchoredPosition = new Vector2(53f, -49f);

        // DragIconController を削除
        Destroy(clone.GetComponent<DragIconController>());
        foreach (var comp in clone.GetComponentsInChildren<DragIconController>())
            Destroy(comp);

        // ★ CanvasGroup を必ず付ける
        CanvasGroup cg = clone.GetComponent<CanvasGroup>();
        if (cg == null) cg = clone.AddComponent<CanvasGroup>();

        // ★ ここを false にすることで、DropArea に Raycast を通す
        cg.blocksRaycasts = false;
        cg.interactable = true;
        cg.alpha = 1f;

        // DropAreaIconDrag を付ける
        DropAreaIconDrag dragScript = clone.AddComponent<DropAreaIconDrag>();
        dragScript.slotIndex = slotIndex;
        dragScript.unitStats = currentUnitStats;
        dragScript.SetOriginalPos();

        // Clone の CheckImage を非表示
        var checkImages = clone.GetComponentsInChildren<Image>(true);
        foreach (var img in checkImages)
        {
            if (img.gameObject.name == "CheckImage")
                img.enabled = false;
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
        // DragIconController の制限解除
        foreach (var icon in FindObjectsOfType<DragIconController>())
        {
            if (icon.unitStats == currentUnitStats)
            {
                icon.isUsedInDropArea = false;
                icon.SetDraggable(true); 
                icon.CheckObj(false);
            }
        }

       
       

        // データ削除
        UnitDataCarrier.Instance.selectedUnits[slotIndex] = null;
        currentUnitStats = null;
    }

    private void CreateCloneFromExistingIcon(UnitStatsData stats)
    {
        // シーン内の DragIconController を全部探す
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
            Debug.LogError("復元対象の DragIcon がシーン内に見つかりません");
            return;
        }

        // Clone を作成
        GameObject clone = Instantiate(source.gameObject, dropTargetParent);

        // DragIconController を削除
        Destroy(clone.GetComponent<DragIconController>());

        // 子に DragIconController がいたら削除
        foreach (var comp in clone.GetComponentsInChildren<DragIconController>())
            Destroy(comp);

        // 位置調整
        RectTransform rt = clone.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(53f, -49f);

        // CanvasGroup を付ける
        CanvasGroup cg = clone.GetComponent<CanvasGroup>();
        if (cg == null) cg = clone.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
        cg.interactable = true;
        cg.alpha = 1f;

        // DropAreaIconDrag を付ける
        DropAreaIconDrag dragScript = clone.AddComponent<DropAreaIconDrag>();
        dragScript.slotIndex = slotIndex;
        dragScript.unitStats = stats;
        dragScript.originalDropArea = this;
        dragScript.SetOriginalPos();

        // CheckImage を非表示
        foreach (var img in clone.GetComponentsInChildren<Image>(true))
        {
            if (img.gameObject.name == "CheckImage")
                img.enabled = false;
        }
    }

    private void MarkDragIconAsUsed(UnitStatsData stats)
    {
        foreach (var icon in FindObjectsOfType<DragIconController>())
        {
            if (icon.unitStats == stats)
            {
                icon.isUsedInDropArea = true;
                icon.SetDraggable(false); // ★ ドラッグ禁止
                icon.CheckObj(true);      // チェックON
            }
        }
    }
}