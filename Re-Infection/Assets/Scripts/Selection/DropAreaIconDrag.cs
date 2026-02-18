using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;

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

    private DropArea hoveredArea = null;
    private GameObject removedClone = null;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.blocksRaycasts = true;

        originalParent = transform.parent;
        originalPos = GetComponent<RectTransform>().anchoredPosition;

       originalDropArea = GetComponentInParent<DropArea>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        droppedSuccessfully = false;

        if (originalDropArea != null)
        {
            int oldIndex = originalDropArea.slotIndex;
            originalDropArea.currentUnitStats = null;
            UnitDataCarrier.Instance.selectedUnits[oldIndex] = null;
        }

        canvasGroup.blocksRaycasts = false;

        transform.SetParent(transform.root, true);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        DetectDropAreaAndClearClone(eventData);

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

            if (originalDropArea.transform.childCount > 0)
            {
                foreach(Transform child in originalDropArea.transform)
                {
                    if (child != null && child.GetComponent<DropAreaIconDrag>()!=null)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }

           foreach(var icon in FindObjectsOfType<DragIconController>())
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
        //originalDropArea = GetComponentInParent<DropArea>();
    }

    private void DetectDropAreaAndClearClone(PointerEventData eventData)
{
    var results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventData, results);

    DropArea hitArea = null;

    foreach (var r in results)
    {
        hitArea = r.gameObject.GetComponentInParent<DropArea>();
        if (hitArea != null)
            break;
    }

    // DropArea が変わった瞬間
    if (hitArea != hoveredArea)
    {
        // ① 前の DropArea の clone を復元
        if (hoveredArea != null && removedClone != null)
        {
            Transform prevParent = hoveredArea.dropTargetParent;
            removedClone.transform.SetParent(prevParent);
            removedClone.SetActive(true);
        }

        // ② 新しい DropArea に入った場合
        if (hitArea != null)
        {
            Transform newParent = hitArea.dropTargetParent;

            if (newParent.childCount > 0)
            {
                removedClone = newParent.GetChild(0).gameObject;
                removedClone.SetActive(false);
            }
            else
            {
                removedClone = null;
            }
        }
        else
        {
            removedClone = null;
        }

        hoveredArea = hitArea;
    }
}
}