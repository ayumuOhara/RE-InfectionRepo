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
            if (originalDropArea != null)
            {
                int oldIndex = originalDropArea.slotIndex;

                originalDropArea.currentUnitStats = null;
                UnitDataCarrier.Instance.selectedUnits[oldIndex] = null;
            }

            foreach (var icon in FindObjectsOfType<DragIconController>())
            {
                if (icon.unitStats == unitStats)
                {
                    icon.isUsedInDropArea = false;
                    icon.SetDraggable(true);
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