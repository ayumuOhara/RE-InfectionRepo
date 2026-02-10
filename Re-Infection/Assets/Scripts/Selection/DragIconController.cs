using System.Collections;
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
    public bool isPaidUnit = false;//このユニットは購入が必要か
    public int price = 0; //価格
    public GameObject paidUnitKey;
    public GameObject notEnoughMoneyObj;
    public UnitPaidDialog paidDialog;
    public TextMeshProUGUI price_text;
    public Wallet wallet;

    public TextMeshProUGUI cost_text;
   

    private Transform returnTarget;
    private Vector2 originalPos;
    private Transform originalParent;
    void Awake()
    {
        wallet = Resources.Load<PlayerStatusData>("PlayerStatusData").wallet;
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        returnTarget = transform.parent;
        unitIcon.sprite = unitStats.unitStats.unitSprite;

        cost_text.text = $"{unitStats.unitStats.summonCost}";

        //CheckImage.SetActive(false);
        //CheckImage.SetActive(isUsedInDropArea);

        notEnoughMoneyObj.SetActive(false);

        //有料ユニット
        if (isPaidUnit)
        {
            paidUnitKey.SetActive(true);
            price_text.text = price.ToString();



            paidDialog.onClickYes = () =>
            {

                //所持金✅
                if (!wallet.CanBuy(price))
                {
                    Debug.Log("お金が足りません。現在の所持金: " + wallet.CurrentMoney);
                    if (notEnoughMoneyObj != null)
                    {
                        StartCoroutine(NotEnoughMoney());
                    }
                    return;
                }


                //お金を引く
                wallet.RemoveMoney(price);


                //購入完了
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

  
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isUsedInDropArea)
        {
            return;
        }

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
            return;
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
        if (isPaidUnit)
        {
            // メッセージを作成
            string msg = $"${price} を支払って\n「{unitStats.unitStats.unitName}」を\n購入しますか？";

            // ダイアログにメッセージを渡す
            paidDialog.SetDialogMessage(msg);

            // ダイアログを表示（ボタンと同じ関数）
            paidDialog.Dialog();
            return;
        }

        // 無料ユニットなら詳細を開く
        detaUI.SetUnit(unitStats.unitStats);
    }

    public IEnumerator NotEnoughMoney()
    {
        notEnoughMoneyObj.SetActive(true);
        yield return new WaitForSeconds(1f);
        notEnoughMoneyObj.SetActive(false);
    }
    public void SetDraggable(bool canDrag)
    {
        canvasGroup.blocksRaycasts = canDrag;
        unitIcon.raycastTarget = canDrag;
    }
}