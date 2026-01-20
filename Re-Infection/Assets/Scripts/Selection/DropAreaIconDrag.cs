using UnityEngine.EventSystems;
using UnityEngine;

public class DropAreaIconDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private Vector2 originalPos;
    private Transform originalParent;

    public int slotIndex;
    public UnitStatsData unitStats;
    public bool droppedSuccessfully = false;

    public DropArea originalDropArea; // ★ 追加

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        originalParent = transform.parent;
        originalPos = GetComponent<RectTransform>().anchoredPosition;

       originalDropArea = GetComponentInParent<DropArea>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        droppedSuccessfully = false;

        canvasGroup.blocksRaycasts = false;
        transform.SetParent(transform.root, true);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (!droppedSuccessfully)
        {
            // ★ 元の DropArea のデータを確実にクリア
            if (originalDropArea != null)
            {
                originalDropArea.currentUnitStats = null;

                while (UnitDataCarrier.Instance.selectedUnits.Count <= slotIndex)
                    UnitDataCarrier.Instance.selectedUnits.Add(null);

                UnitDataCarrier.Instance.selectedUnits[slotIndex] = null;
            }


            // DragIconController の制限解除
            foreach (var icon in FindObjectsOfType<DragIconController>())
            {
                if (icon.unitStats == unitStats)
                {
                    icon.isUsedInDropArea = false;
                    icon.CheckObj(false);
                }
            }

            Destroy(gameObject);
            return;
        }
    }

    public void SetOriginalPos()
    {
        originalParent = transform.parent;
        originalPos = GetComponent<RectTransform>().anchoredPosition;
        originalDropArea = GetComponentInParent<DropArea>();
    }
}