using UnityEngine;
using UnityEngine.EventSystems;

public class DropAreaIconDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private Vector2 originalPos;
    

    public int slotIndex;
    public UnitStatsData unitStats;
    public bool droppedSuccessfully = false;

    private DropArea originalDropArea;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        originalPos = GetComponent<RectTransform>().anchoredPosition;

        Transform check = transform.Find("CheckImage");
        if (check != null)
        {
            Debug.Log($"[Clone Start] CheckImage FOUND on {gameObject.name}");
            check.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log($"[Clone Start] CheckImage NOT FOUND on {gameObject.name}");
        }
        originalDropArea = transform.GetComponentInParent<DropArea>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        // ★ Clone を Canvas の最前面に移動
        transform.SetParent(transform.root, true);
        transform.SetAsLastSibling();

        Transform check = transform.Find("CheckImage");
        if (check != null)
        {
            check.gameObject.SetActive(false);
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

   public void OnEndDrag(PointerEventData eventData)
{
    canvasGroup.blocksRaycasts = true;

    // DropArea が受け取った場合は droppedSuccessfully = true
    if (!droppedSuccessfully)
    {
        // 失敗扱い → 元アイコンの CheckImage を OFF にしてしまう処理
        // （ここは本当に失敗したときだけ実行される）
        originalDropArea.currentUnitStats = null;
        originalDropArea.diaplayText();
        UnitDataCarrier.Instance.selectedUnits[slotIndex] = null;

        foreach (var icon in FindObjectsOfType<DragIconController>())
        {
            if (icon.unitStats == unitStats)
            {
                icon.isUsedInDropArea = false;
                icon.isDropped = false;
            }
        }

        DropArea.UpdateAllCheckImage();
        Destroy(gameObject);
        return;
    }

    //// 成功していた場合は元位置に戻すだけ
    //GetComponent<RectTransform>().anchoredPosition = originalPos;
}
    public void SetOriginalPos()
    {
        originalPos = GetComponent<RectTransform>().anchoredPosition;
    }
}