using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragIconController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    private Image image;

    public Color originalColor;

    public UnitStats unitStats;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();

        Image img = GetComponent<Image>();
        if (img != null)
        {
            originalColor = img.color;

            //アイコンの見た目をUnitStatsから設定
            if (unitStats != null && unitStats.unitSprite != null)
            {
                img.sprite = unitStats.unitSprite;
            }
        }
       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canvasGroup.interactable || DroppedSpriteRegistry.IsDropped(image.sprite))
        {
            canvasGroup.blocksRaycasts = true;
            eventData.pointerDrag = null;
           
        }

        originalPosition = rectTransform.localPosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            null, // ← Overlayならnull
            out localPoint
        );

        rectTransform.localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.localPosition = originalPosition;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}