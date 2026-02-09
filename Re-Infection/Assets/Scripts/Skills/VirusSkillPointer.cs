using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VirusPointer
{
    public static class WaitEndDrag
    {
        // ドラッグ終了まで待機
        public static async Task WaitDragEndAsync()
        {
            await VirusSkillPointer.dragEndTcs.Task;
        }
    }

    public class VirusSkillPointer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] GameObject virusPrefab;
        UnitManager unitManager;
        WaveSpawner waveSpawner;
        GameObject dragObj;

        bool isDragging = false;    // ドラッグ中フラグ
        public bool IsDragging => isDragging;

        bool isDragCancel = false;  // 使用キャンセルフラグ

        // ドラッグ終了待機
        public static TaskCompletionSource<PointerEventData> dragEndTcs;

        void Awake()
        {
            waveSpawner = FindObjectOfType<WaveSpawner>();
            if (unitManager == null)
                unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!waveSpawner.IsStartWave || Time.timeScale == 0) return;

            dragEndTcs = new TaskCompletionSource<PointerEventData>();

            if (dragObj == null)
            {
                dragObj = Instantiate(virusPrefab);
                dragObj.SetActive(false);
            }

            dragObj.SetActive(true);

            _ = WaitEndDrag.WaitDragEndAsync();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!waveSpawner.IsStartWave || Time.timeScale == 0) return;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            dragObj.transform.position = mousePos;

            isDragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!waveSpawner.IsStartWave || Time.timeScale == 0) return;

            dragEndTcs?.TrySetResult(eventData);

            if (isDragCancel)
            {
                dragObj.SetActive(false);
            }

            isDragging = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isDragging)
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
}