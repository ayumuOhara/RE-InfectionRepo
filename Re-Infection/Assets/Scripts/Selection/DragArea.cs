using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropArea : MonoBehaviour, IDropHandler
{
    [SerializeField] private Transform dropTargetParent;
    public UnitStats currentUnitStats;
    public int slotIndex; // このDropAreaが何番目の枠か
    public TextMeshProUGUI displayTMP;
    public GameObject displayTMPObj;

    void Start()
    {
        // 復元処理（UnitDataCarrierから）
        if (UnitDataCarrier.Instance != null &&
            UnitDataCarrier.Instance.selectedUnits.Count > slotIndex &&
            UnitDataCarrier.Instance.selectedUnits[slotIndex] != null)
        {
            UnitStats unit = UnitDataCarrier.Instance.selectedUnits[slotIndex];
            currentUnitStats = unit;

            GameObject restored = new GameObject("RestoredUnit");
            restored.transform.SetParent(dropTargetParent);
            restored.AddComponent<RectTransform>().anchoredPosition = Vector2.zero;

            Image img = restored.AddComponent<Image>();
            img.sprite = unit.unitSprite;

            CanvasGroup cg = restored.AddComponent<CanvasGroup>();
            cg.alpha = 1f;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }

        displayTMP.text = "";
        displayTMPObj.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        // 既存の子オブジェクトを削除
        if (dropTargetParent.childCount > 0)
        {
            Transform previous = dropTargetParent.GetChild(0);
            Destroy(previous.gameObject);
        }

        // 先に currentUnitStats を更新
        DragIconController droppedController = dropped.GetComponent<DragIconController>();
        if (droppedController != null)
        {
            currentUnitStats = droppedController.unitStats;

            while (UnitDataCarrier.Instance.selectedUnits.Count <= slotIndex)
            {
                UnitDataCarrier.Instance.selectedUnits.Add(null);
            }
            UnitDataCarrier.Instance.selectedUnits[slotIndex] = currentUnitStats;
        }

        // クローン生成
        GameObject clone = Instantiate(dropped, dropTargetParent);
        clone.tag = "CloneOnly";
        clone.SetActive(true);
        clone.transform.localScale = new Vector3(1.3f, 1.3f, 1f);
        clone.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Destroy(clone.GetComponent<DragIconController>());
        clone.transform.SetAsFirstSibling();

        // CanvasGroupを必ず付ける
        CanvasGroup cg = clone.GetComponent<CanvasGroup>();
        if (cg == null) cg = clone.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = true;

        // ドラッグ用スクリプトをアタッチ
        DropAreaIconDrag dragScript = clone.AddComponent<DropAreaIconDrag>();
        dragScript.slotIndex = slotIndex;
        dragScript.unitStats = currentUnitStats;

        // UnitIconClick にも渡す
        UnitIconClick iconClick = clone.GetComponent<UnitIconClick>();
        if (iconClick != null)
        {
            iconClick.slotIndex = slotIndex;
            iconClick.unitStats = currentUnitStats;
        }

        // テキスト表示更新
        if (currentUnitStats != null && displayTMP != null)
        {
            displayTMPObj.SetActive(true);
            displayTMP.text = $"{currentUnitStats.unitName}";
        }
    }
    public void diaplayText()
    {
        displayTMPObj.SetActive(false);
        displayTMP.text = "";
    }
}