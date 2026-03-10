using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragIconController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image unitIcon;
    [SerializeField] private Image unitSilhouette;
    [SerializeField] private Image jobIcon;

    [SerializeField] private Sprite keySprite;

    private CanvasGroup canvasGroup;

    public bool isAnimating = false;

    public UnitStatsData unitStats;
    public bool isUsedInDropArea = false;
    public GameObject CheckImage;

    public TextMeshProUGUI cost_text;
    public TextMeshProUGUI levelText;

    public UnitDetailUII detailUI;

    private void OnEnable()
    {
        UnitStats.OnUnlockUnit += SetUnitSlot;
        UpGradeManager.OnUnitLevelChanged += UnitLevelChanged;

        SetUnitSlot();
    }

    private void OnDisable()
    {
        UnitStats.OnUnlockUnit -= SetUnitSlot;
        UpGradeManager.OnUnitLevelChanged -= UnitLevelChanged;
    }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!unitStats.unitStats.IsUnitUnlocked()) return;

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

    private void SetUnitSlot()
    {
        unitIcon.sprite = unitStats.unitStats.unitSprite;

        if (!unitStats.unitStats.IsUnitUnlocked())
        {
            unitSilhouette.color = Color.black;
            jobIcon.sprite = keySprite;
            cost_text.text = "?";
            levelText.text = $"???";
        }
        else
        {
            unitSilhouette.color = new Color(0, 0, 0, 0);
            jobIcon.sprite = unitStats.unitStats.JobSprite;
            cost_text.text = $"{unitStats.unitStats.summonCost}";

            UnitLevelChanged();
        }
    }

    private void UnitLevelChanged()
    {
        if (unitStats.unitStats.lv != unitStats.unitStats.MaxLevel)
            levelText.text = $"Lv.<size=40>{unitStats.unitStats.lv}</size>";
        else
            levelText.text = $"<sprite=0>";
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