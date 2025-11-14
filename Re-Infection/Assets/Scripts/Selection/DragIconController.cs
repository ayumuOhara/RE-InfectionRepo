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
    private Transform originalParent;
    
    public Color originalColor;

    public Transform returnTarget;
    public UnitStats unitStats;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();

        // サイズを固定（例：80%スケール）
        transform.localScale = new Vector3(0.8f, 0.8f, 1f);


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
        if (UnitDataCarrier.Instance != null &&
        UnitDataCarrier.Instance.selectedUnits.Contains(unitStats))
        {
            canvasGroup.blocksRaycasts = true;
            eventData.pointerDrag = null;
            return;
        }

        originalParent = transform.parent;

        // Canvas直下に移動（描画順を最前面に）
        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;


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
        // 戻り先に戻す（インスペクターで指定された Transform）
        rectTransform.SetParent(returnTarget, false); // ← 親を戻す（ローカル座標維持）
        rectTransform.anchoredPosition = Vector2.zero; // ← 親の基準で位置を揃える

        transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
}