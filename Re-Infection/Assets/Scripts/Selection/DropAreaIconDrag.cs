using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropAreaIconDrag : MonoBehaviour, IPointerClickHandler
{
    public int slotIndex;
    public UnitStatsData unitStats;
    public DropArea originalDropArea;

    public bool isAnimating = false;
    private void Start()
    {
        originalDropArea = GetComponentInParent<DropArea>();

        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = true;
        cg.interactable = true;

        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }


    //タップでスロット解除
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(ClickAndRemove());
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

    private IEnumerator ClickAndRemove()
    {
        // ① アニメーションを待つ
        yield return StartCoroutine(ClickAnimation());

        // ② DropArea が無いなら終了
        if (originalDropArea == null)
            yield break;

        // ③ DropArea のデータを空にする
        originalDropArea.currentUnitStats = null;
        UnitDataCarrier.Instance.SetUnitofSlotIndex(null, slotIndex);

        // ④ リスト側の DragIconController を復活
        foreach (var icon in FindObjectsOfType<DragIconController>())
        {
            if (icon.unitStats == unitStats)
            {
                icon.isUsedInDropArea = false;
                icon.CheckObj(false);
            }
        }

        // ⑤ clone を削除
        Destroy(gameObject);

        // ⑥ チェック更新
        DropArea.UpdateAllCheckImage();

    }
}