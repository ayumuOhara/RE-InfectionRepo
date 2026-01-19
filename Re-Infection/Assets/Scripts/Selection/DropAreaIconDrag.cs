using UnityEngine;
using UnityEngine.EventSystems;

public class DropAreaIconDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private Vector2 originalPos;
    

    public int slotIndex;
    public UnitStatsData unitStats;

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

        // ドロップ先の DropArea を取得
        DropArea dropArea = null;
        if (eventData.pointerEnter != null)
            dropArea = eventData.pointerEnter.GetComponentInParent<DropArea>();

        
        DropArea parentDropArea = originalDropArea;

        // ★ 別の DropArea にドロップしようとした場合
        if (dropArea != null && dropArea != parentDropArea)
        {
            // DropArea のデータを消す
            parentDropArea.currentUnitStats = null;
            parentDropArea.diaplayText();

            // UnitDataCarrier のデータも消す
            UnitDataCarrier.Instance.selectedUnits[slotIndex] = null;

            // DragIconController のフラグを戻す
            DragIconController[] icons = FindObjectsOfType<DragIconController>();
            foreach (var icon in icons)
            {
                if (icon.unitStats == unitStats)
                {
                    icon.isUsedInDropArea = false;
                    icon.isDropped = false;
                }
            }

            DropArea.UpdateAllCheckImage();

            // ★ Clone を削除
            Destroy(gameObject);
            return;
        }

        // ★ DropArea 外にドロップ → 削除
        if (dropArea == null)
        {
            // DropArea のデータを消す
            parentDropArea.currentUnitStats = null;
            parentDropArea.diaplayText();

            // UnitDataCarrier のデータも消す
            UnitDataCarrier.Instance.selectedUnits[slotIndex] = null;

            // DragIconController のフラグを戻す
            DragIconController[] icons = FindObjectsOfType<DragIconController>();
            foreach (var icon in icons)
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

        // ★ 同じ DropArea 内なら元位置に戻す
        GetComponent<RectTransform>().anchoredPosition = originalPos;
    }
    public void SetOriginalPos()
    {
        originalPos = GetComponent<RectTransform>().anchoredPosition;
    }
}