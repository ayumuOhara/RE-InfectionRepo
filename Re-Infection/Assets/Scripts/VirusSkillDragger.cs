using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public static class WaitEndDrag
{
    // �h���b�O�I���܂őҋ@
    public static async Task WaitDragEndAsync()
    {
        await VirusSkillDragger.dragEndTcs.Task;
    }
}

public class VirusSkillDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject virusAreaPrefab;
    GameObject dragObj;

    bool isDragging = false;    // �h���b�O���t���O
    bool isDragCancel = false;  // �g�p�L�����Z���t���O

    // �h���b�O�I���ҋ@
    public static TaskCompletionSource<PointerEventData> dragEndTcs;

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragEndTcs = new TaskCompletionSource<PointerEventData>();

        if (dragObj == null)
        {
            dragObj = Instantiate(virusAreaPrefab);
            dragObj.SetActive(false);
        }

        dragObj.SetActive(true);
        _ = WaitEndDrag.WaitDragEndAsync();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        dragObj.transform.position = mousePos;

        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragEndTcs?.TrySetResult(eventData);

        if(isDragCancel)
        {
            dragObj.SetActive(false);
        }

        isDragging = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isDragging)
        {
            SpriteRenderer sr = dragObj.GetComponent<SpriteRenderer>();
            sr.enabled = false;
            isDragCancel = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging)
        {
            SpriteRenderer sr = dragObj.GetComponent<SpriteRenderer>();
            sr.enabled = true;
            isDragCancel = false;
        }
    }
}
