using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragIconController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image unitIcon;

    private CanvasGroup canvasGroup;

    public bool isAnimating = false;

    public UnitStatsData unitStats;
    public bool isUsedInDropArea = false;
    public GameObject CheckImage;

    public TextMeshProUGUI cost_text;

    public UnitDetailUII detailUI;

    private void OnEnable()
    {
        
    }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        unitIcon.sprite = unitStats.unitStats.unitSprite;
        cost_text.text = $"{unitStats.unitStats.summonCost}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!unitStats.unitStats.isUnlocked) return;

        StartCoroutine(ClickAnimation());
        // ① 未編成なら編成する（最優先）
        if (!isUsedInDropArea)
        {
            DropArea empty = DropArea.GetFirstEmptySlot();
            if (empty != null)
            {
                empty.SetUnitFromList(this);
            }
        }

        // ② そのあと必ず詳細 UI を開く
        if (detailUI != null)
        {
            detailUI.SetUnit(unitStats.unitStats);
        }
    }

private IEnumerator ClickAnimation()
    {
        if (isAnimating) yield break;
        isAnimating = true;

        RectTransform rt = GetComponent<RectTransform>();

        //元のスケール
        Vector3 originalScale = rt.localScale;

        //少し縮む
        rt.localScale = originalScale * 0.9f;
        yield return new WaitForSeconds(0.05f);

        //元に戻る
        rt.localScale = originalScale;
        yield return new WaitForSeconds(0.05f);

        isAnimating = false;
    }

    public void CheckObj(bool isOn)
    {
        CheckImage.SetActive(isOn);
    }
}