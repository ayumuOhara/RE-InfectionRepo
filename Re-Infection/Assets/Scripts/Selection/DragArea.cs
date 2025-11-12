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

        Image droppedImage = dropped.GetComponent<Image>();
        Sprite droppedSprite = droppedImage != null ? droppedImage.sprite : null;

        // 🔁 既存の表示があれば削除し、元のPrefabを再許可
        Sprite previousSprite = null;
        if (dropTargetParent.childCount > 0)
        {
            Transform previous = dropTargetParent.GetChild(0);
            Image prevImage = previous.GetComponent<Image>();
            if (prevImage != null)
            {
                previousSprite = prevImage.sprite;
            }

            Destroy(previous.gameObject);
        }

        // 🔓 元のPrefabを再許可
        DragIconController[] allDrags = FindObjectsOfType<DragIconController>();
        if (previousSprite != null)
        {
            foreach (var drag in allDrags)
            {
                Image img = drag.GetComponent<Image>();
                CanvasGroup cg = drag.GetComponent<CanvasGroup>();

                if (img != null && cg != null && img.sprite == previousSprite)
                {
                    cg.interactable = true;
                    cg.blocksRaycasts = true;
                    DragIconController controller = drag.GetComponent<DragIconController>();
                    if (controller != null)
                    {
                        img.color = controller.originalColor;
                    }
                    else
                    {
                        img.color = Color.white;
                    }
                }
            }

            DroppedSpriteRegistry.Unregister(previousSprite);
        }

        // ✅ 新しいSpriteを登録
        DroppedSpriteRegistry.Register(droppedSprite);

        // 🧱 複製生成＆表示
        GameObject clone = Instantiate(dropped, dropTargetParent);
        clone.tag = "CloneOnly";
        clone.SetActive(true);
        clone.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Destroy(clone.GetComponent<DragIconController>());

        CanvasGroup cloneGroup = clone.GetComponent<CanvasGroup>();
        if (cloneGroup != null)
        {
            cloneGroup.alpha = 1f;
            cloneGroup.interactable = false;
            cloneGroup.blocksRaycasts = false;
        }

        // 🚫 同じSpriteを持つPrefabをすべてドラッグ禁止＆グレーアウト
        foreach (var drag in allDrags)
        {
            if (drag.CompareTag("CloneOnly")) continue;

            Image img = drag.GetComponent<Image>();
            CanvasGroup cg = drag.GetComponent<CanvasGroup>();

            if (img != null && cg != null && img.sprite == droppedSprite)
            {
                cg.interactable = false; // ← 禁止！
                cg.blocksRaycasts = false;

                Color original = img.color;
                float gray = (original.r + original.g + original.b) / 3f;
                img.color = new Color(gray, gray, gray, original.a); // ← グレーアウト！
            }
        }
    }
}