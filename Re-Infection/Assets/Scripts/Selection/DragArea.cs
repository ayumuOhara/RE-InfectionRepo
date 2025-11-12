using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropArea : MonoBehaviour, IDropHandler
{
    [SerializeField] private Transform dropTargetParent;
    public UnitStats currentUnitStats;
    public int slotIndex; // このDropAreaが何番目の枠か

    void Start()
    {
        if (UnitDataCarrier.Instance != null &&
            UnitDataCarrier.Instance.selectedUnits.Count > slotIndex &&
            UnitDataCarrier.Instance.selectedUnits[slotIndex] != null)
        {
            UnitStats unit = UnitDataCarrier.Instance.selectedUnits[slotIndex];
            currentUnitStats = unit;

            //表示復元
            GameObject restored = new GameObject("RestoredUnit");
            restored.transform.SetParent(dropTargetParent);
            restored.AddComponent<RectTransform>().anchoredPosition = Vector2.zero;

            Image img = restored.AddComponent<Image>();
            img.sprite = unit.unitSprite;

            CanvasGroup cg = restored.AddComponent<CanvasGroup>();
            cg.alpha = 1f;
            cg.interactable = false;
            cg.blocksRaycasts = false;

            // 🔒 ドラッグ元アイコンを無効化
            DragIconController[] allDrags = FindObjectsOfType<DragIconController>();
            foreach (var drag in allDrags)
            {
                if (drag.unitStats == unit)
                {
                    Image dragImg = drag.GetComponent<Image>();
                    CanvasGroup dragCg = drag.GetComponent<CanvasGroup>();

                    if (dragImg != null && dragCg != null)
                    {
                        dragCg.interactable = false;
                        dragCg.blocksRaycasts = false;

                        // グレーアウト
                        Color original = dragImg.color;
                        float gray = (original.r + original.g + original.b) / 3f;
                        dragImg.color = new Color(gray, gray, gray, original.a);
                    }
                }
            }

        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        Image droppedImage = dropped.GetComponent<Image>();
        Sprite droppedSprite = droppedImage != null ? droppedImage.sprite : null;

        Sprite previousSprite = null;
        if (dropTargetParent.childCount > 0)
        {
            Transform previous = dropTargetParent.GetChild(0);
            Image prevImage = previous.GetComponent<Image>();
            if (prevImage != null)
            {
                previousSprite = prevImage.sprite;
            }

            Destroy(previous.gameObject);
        }

        DragIconController[] allDrags = FindObjectsOfType<DragIconController>();
        if (previousSprite != null)
        {
            foreach (var drag in allDrags)
            {
                Image img = drag.GetComponent<Image>();
                CanvasGroup cg = drag.GetComponent<CanvasGroup>();

                if (img != null && cg != null && img.sprite == previousSprite)
                {
                    cg.interactable = true;
                    cg.blocksRaycasts = true;

                    DragIconController controller = drag.GetComponent<DragIconController>();
                    img.color = controller != null ? controller.originalColor : Color.white;
                }
            }
        }

        GameObject clone = Instantiate(dropped, dropTargetParent);
        clone.tag = "CloneOnly";
        clone.SetActive(true);
        clone.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Destroy(clone.GetComponent<DragIconController>());

        CanvasGroup cloneGroup = clone.GetComponent<CanvasGroup>();
        if (cloneGroup != null)
        {
            cloneGroup.alpha = 1f;
            cloneGroup.interactable = false;
            cloneGroup.blocksRaycasts = false;
        }

        DragIconController droppedController = dropped.GetComponent<DragIconController>();
        if (droppedController != null)
        {
            currentUnitStats = droppedController.unitStats;

            // リストの長さを確保
            while (UnitDataCarrier.Instance.selectedUnits.Count <= slotIndex)
            {
                UnitDataCarrier.Instance.selectedUnits.Add(null);
            }

            // 指定枠に保存
            UnitDataCarrier.Instance.selectedUnits[slotIndex] = currentUnitStats;
        }

        foreach (var drag in allDrags)
        {
            if (drag.CompareTag("CloneOnly")) continue;

            Image img = drag.GetComponent<Image>();
            CanvasGroup cg = drag.GetComponent<CanvasGroup>();

            if (img != null && cg != null && img.sprite == droppedSprite)
            {
                cg.interactable = false;
                cg.blocksRaycasts = false;

                Color original = img.color;
                float gray = (original.r + original.g + original.b) / 3f;
                img.color = new Color(gray, gray, gray, original.a);
            }
        }
    }
}