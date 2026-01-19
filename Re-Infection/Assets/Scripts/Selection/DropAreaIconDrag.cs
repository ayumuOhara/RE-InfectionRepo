using UnityEngine;
using UnityEngine.EventSystems;

public class DropAreaIconDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private Vector2 originalPos;
    private Transform originalParent;
    public int slotIndex;
    public UnitStatsData unitStats;
    public bool droppedSuccessfully = false;

    private DropArea originalDropArea;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        originalParent = transform.parent;
        originalPos = GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
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
            Destroy(gameObject);

            foreach (var icon in FindObjectsOfType<DragIconController>())
            {
                if (icon.unitStats == unitStats)
                {
                    icon.isUsedInDropArea = false;
                    icon.CheckObj(false);
                }
            }

            UnitDataCarrier.Instance.selectedUnits[slotIndex] = null;
            transform.SetParent(originalDropArea.transform, false);
            GetComponent<RectTransform>().anchoredPosition = originalPos;
            return;
        }
    }

    public void SetOriginalPos()
    {
        originalParent = transform.parent;
        originalPos = GetComponent<RectTransform>().anchoredPosition;
    }
}