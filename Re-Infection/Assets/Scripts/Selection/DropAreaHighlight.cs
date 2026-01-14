using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DropAreaHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private Image image;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            // 薄い黄色にフェードイン
            StartFade(new Color(1f, 1f, 0.6f, 1f), 0.3f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 白にフェードアウト
        StartFade(Color.white, 0.3f);
    }

    public void OnDrop(PointerEventData eventData)
    {
        // ドロップ完了時も白に戻す
        StartFade(Color.white, 0.2f);
    }

    private void StartFade(Color targetColor, float duration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeColor(targetColor, duration));
    }

    private IEnumerator FadeColor(Color targetColor, float duration)
    {
        Color startColor = image.color;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            image.color = Color.Lerp(startColor, targetColor, time / duration);
            yield return null;
        }

        image.color = targetColor;
    }
}