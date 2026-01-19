using UnityEngine;
using UnityEngine.EventSystems;

public class DropAreaIconDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private Vector2 originalPos;
    

    public int slotIndex;
    public UnitStats unitStats;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        originalPos = GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // DropArea外なら削除
        if (eventData.pointerEnter == null ||
            eventData.pointerEnter.GetComponentInParent<DropArea>() == null)
        {
            if (UnitDataCarrier.Instance != null &&
                UnitDataCarrier.Instance.selectedUnits.Count > slotIndex)
            {
                UnitDataCarrier.Instance.selectedUnits[slotIndex] = null;
                // 🔽 DropAreaを取得してテキストを消す
                DropArea parentDropArea = transform.parent.GetComponentInParent<DropArea>();
                if (parentDropArea != null)
                {
                    parentDropArea.diaplayText();
                }

            }
            Destroy(gameObject);
           
        }
        else
        {
            // DropArea内なら元位置に戻す
            GetComponent<RectTransform>().anchoredPosition = originalPos;
        }
    }
    public void SetOriginalPos()
    {
        originalPos = GetComponent<RectTransform>().anchoredPosition;
    }
}