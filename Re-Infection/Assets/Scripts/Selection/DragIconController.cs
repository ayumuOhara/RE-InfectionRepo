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
    private Vector3 originalScale;

    public Color originalColor;
    public Vector3 tergetPositoin=new Vector3(0,0,0);
    public Image Icon;
    public Transform returnTarget;
    public UnitStatsData unitStats;
    public bool isDropped=false;
    public bool isUsedInDropArea = false;
    public GameObject CheckImage;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponentInChildren<Image>();

        //// サイズを固定（例：80%スケール）
        //transform.localScale = new Vector3(1.2f, 1.2f, 1f);


        Image img = GetComponent<Image>();
        if (img != null)
        {
            originalColor = img.color;

            //アイコンの見た目をUnitStatsから設定
            if (unitStats != null && unitStats.unitStats.unitSprite != null)
            {
                img.sprite = unitStats.unitStats.unitSprite;
            }
        }
        CheckImage.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // すでにどこかの DropArea で使われているならドラッグ不可
        if (isUsedInDropArea)
        {
            
            return;
        }
        if (isUsedInDropArea == true)
        {
            CheckImage.SetActive(false);
        }
        else
        {
            CheckImage.SetActive(true);
        }
        originalParent = transform.parent;

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
        transform.position = tergetPositoin;

        //transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        isDropped = true;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void CheckObj()
    {
        if (isUsedInDropArea == true)
        {
            CheckImage.SetActive(false);
        }
        else
        {
            CheckImage.SetActive(true);
        }
    }
}