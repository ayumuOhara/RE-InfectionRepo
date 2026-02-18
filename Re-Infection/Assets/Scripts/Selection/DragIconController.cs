using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("ユニット購入")]
    public bool isPaidUnit = false;
    public int price = 0;
    public GameObject paidUnitKey;
    public GameObject notEnoughMoneyObj;
    public UnitPaidDialog paidDialog;
    public TextMeshProUGUI price_text;
    public Wallet wallet;

    public TextMeshProUGUI cost_text;

    private Transform originalParent;
    private Vector2 originalPos;

    private DropArea lastHoveredDropArea = null;
    private GameObject removedClone = null;
    private DropArea hoveredArea = null;
    private bool droppedSuccessfully = false;
    void Awake()
    {
        wallet = Resources.Load<PlayerStatusData>("PlayerStatusData").wallet;
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        originalParent = transform.parent;
        unitIcon.sprite = unitStats.unitStats.unitSprite;
        cost_text.text = $"{unitStats.unitStats.summonCost}";

        notEnoughMoneyObj.SetActive(false);

        if (isPaidUnit)
        {
            paidUnitKey.SetActive(true);
            price_text.text = price.ToString();

            paidDialog.onClickYes = () =>
            {
                if (!wallet.CanBuy(price))
                {
                    if (notEnoughMoneyObj != null)
                        StartCoroutine(NotEnoughMoney());
                    return;
                }

                wallet.RemoveMoney(price);

                isPaidUnit = false;
                paidUnitKey.SetActive(false);

                paidDialog.UnitPaidDialogObj.SetActive(false);
            };
        }
        else
        {
            paidUnitKey.SetActive(false);
        }
    }

    // ★ ドラッグ可能/不可能を切り替える
    public void SetDraggable(bool canDrag)
    {
        canvasGroup.blocksRaycasts = canDrag;
        unitIcon.raycastTarget = canDrag;  
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isUsedInDropArea)
            return; // ★ DropArea に入っているならドラッグ開始禁止

        originalParent = transform.parent;
        originalPos = rectTransform.anchoredPosition;

        CheckImage.SetActive(false);

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isUsedInDropArea)
            return; // ★ DropArea に入っているならドラッグ中も禁止

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        rectTransform.localPosition = localPoint;

        DetectDropAreaAndClearClone(eventData);


    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent, false);
        rectTransform.anchoredPosition = originalPos;

        canvasGroup.blocksRaycasts = true;

        // ★ Drop が成功していない場合 → Clone を復元
        if (!droppedSuccessfully && removedClone != null && hoveredArea != null)
        {
           
            Transform parent = hoveredArea.transform.GetChild(0);
            removedClone.transform.SetParent(parent);
            removedClone.SetActive(true);
        }

        // リセット
        removedClone = null;
        hoveredArea = null;
        droppedSuccessfully = false;
    }

    public void CheckObj(bool isOn)
    {
        CheckImage.SetActive(isOn);
    }

    public void OnClickUnitIcon()
    {
        if (isPaidUnit)
        {
            string msg = $"${price} を支払って\n「{unitStats.unitStats.unitName}」を\n購入しますか？";
            paidDialog.SetDialogMessage(msg);
            paidDialog.Dialog();
            return;
        }

        detaUI.SetUnit(unitStats.unitStats);
    }

    public IEnumerator NotEnoughMoney()
    {
        notEnoughMoneyObj.SetActive(true);
        yield return new WaitForSeconds(1f);
        notEnoughMoneyObj.SetActive(false);
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