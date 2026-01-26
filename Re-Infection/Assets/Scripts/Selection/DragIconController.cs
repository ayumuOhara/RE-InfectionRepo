using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragIconController : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image unitIcon;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public UnitStatsData unitStats;
    public bool isUsedInDropArea = false;
    public GameObject CheckImage;

    public UnitDetailUII detaUI;

    private Transform returnTarget;
    private Vector2 originalPos;
    private Transform originalParent;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        returnTarget = transform.parent;
        unitIcon.sprite = unitStats.unitStats.unitSprite;

        CheckImage.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isUsedInDropArea) return;

        originalParent = transform.parent;
        originalPos = GetComponent<RectTransform>().anchoredPosition;


        CheckImage.SetActive(false);

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            null,
            out Vector2 localPoint
        );

        rectTransform.localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 親を元に戻す
        transform.SetParent(originalParent, false);

        // 位置も元に戻す
        GetComponent<RectTransform>().anchoredPosition = originalPos;

        canvasGroup.blocksRaycasts = true;
    }

    public void CheckObj(bool isOn)
    {
        CheckImage.SetActive(isOn);
    }

    public void OnClickUnitIcon()
    {
        detaUI.SetUnit(unitStats.unitStats);
    }
}