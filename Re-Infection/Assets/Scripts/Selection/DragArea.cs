using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropArea : MonoBehaviour, IDropHandler
{
    [SerializeField] private Transform dropTargetParent;
    [SerializeField] private Vector2 cloneOffset = Vector2.zero;
    public UnitStatsData currentUnitStats;
    public int slotIndex; // このDropAreaが何番目の枠か
    public TextMeshProUGUI displayTMP;
    public GameObject displayTMPObj;
    //public DragIconController dragIconController;

      void Start()
    {
        // 復元処理（UnitDataCarrierから）
        if (UnitDataCarrier.Instance != null &&
            UnitDataCarrier.Instance.selectedUnits.Count > slotIndex &&
            UnitDataCarrier.Instance.selectedUnits[slotIndex] != null)
        {
            UnitStatsData unit = UnitDataCarrier.Instance.selectedUnits[slotIndex];
            currentUnitStats = unit;

            GameObject restored = new GameObject("RestoredUnit");
            restored.transform.SetParent(dropTargetParent);
            restored.AddComponent<RectTransform>().anchoredPosition = Vector2.zero;

            Image img = restored.AddComponent<Image>();
            img.sprite = unit.unitStats.unitSprite;

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
        //Debug.Log("Dropped object: " + dropped.name);
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
            droppedController.isUsedInDropArea = true;

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
        //clone.transform.localScale = new Vector3(1.3f, 1.3f, 1f);
        RectTransform rt = clone.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;

        if (clone.CompareTag("CloneOnly"))
        {
            rt.anchoredPosition += cloneOffset;
        }
        Destroy(clone.GetComponent<DragIconController>());
        clone.transform.SetAsFirstSibling();
        //dragIconController.CheckObj();
        foreach (var comp in clone.GetComponentsInChildren<DragIconController>())
        {
            Destroy(comp);
        }
       
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
            iconClick.unitData = currentUnitStats;
        }


        // ★ 位置調整が終わったあとに originalPos をセット
        dragScript.SetOriginalPos();

        //// テキスト表示更新
        //if (currentUnitStats != null && displayTMP != null)
        //{
        //    displayTMPObj.SetActive(true);
        //    displayTMP.text = $"{currentUnitStats.unitStats.unitName}";
        //}

        DropArea.UpdateAllCheckImage();
    }

    public static void UpdateAllCheckImage()
    {
        //全てのDropAreaを取得
        DropArea[] dropAreas = FindObjectsOfType<DropArea>();

        //全てのDragIconControllerを取得
        DragIconController[] icons = FindObjectsOfType<DragIconController>();

        foreach(var icon in icons)
        {
            bool isUsed = false;

            //どれかのDropAreaに同じUnitStatsDataが入っていればON
            foreach(var da in dropAreas)
            {
                if(da.currentUnitStats!=null &&
                    da.currentUnitStats == icon.unitStats)
                {
                    isUsed = true;
                    break;
                }
            }
            icon.CheckObj(isUsed);
        }
    }

    public static bool IsUnitInAnyDropArea(UnitStatsData target)
    {
        DropArea[] dropAreas = FindObjectsOfType<DropArea>();

        foreach(var da in dropAreas)
        {
            if(da.currentUnitStats!=null &&
                da.currentUnitStats == target)
            {
                return true;
            }
        }
        return false;
    }
    public void diaplayText()
    {
        displayTMPObj.SetActive(false);
        displayTMP.text = "";
    }
}