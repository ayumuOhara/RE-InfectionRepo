using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropArea : MonoBehaviour, IDropHandler
{
    [SerializeField] private Transform dropTargetParent;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        // ドロップされたImageのSpriteを取得
        Image droppedImage = dropped.GetComponent<Image>();
        Sprite droppedSprite = droppedImage != null ? droppedImage.sprite : null;

        // ドロップ先に複製を生成
        GameObject clone = Instantiate(dropped, dropTargetParent);
        clone.SetActive(true);
        clone.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        // 複製側をアルファ1＆ドラッグ禁止
        CanvasGroup cloneGroup = clone.GetComponent<CanvasGroup>();
        if (cloneGroup != null)
        {
            cloneGroup.alpha = 1f;
            cloneGroup.interactable = false;
            cloneGroup.blocksRaycasts = false;
        }

        // 🔽 ここに書く！元のImageをグレーアウト＆ドラッグ禁止にする処理
        DragIconController[] allDrags = FindObjectsOfType<DragIconController>();
        foreach (var drag in allDrags)
        {
            Image img = drag.GetComponent<Image>();
            CanvasGroup cg = drag.GetComponent<CanvasGroup>();

            // dropped（元のImage）でも clone（複製）でもないものだけを対象にする
            if (img != null && cg != null && img.sprite == droppedSprite && drag.gameObject != dropped)
            {
                cg.alpha = 0.5f;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
        }
    
}
}