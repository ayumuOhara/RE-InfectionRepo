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
        public static VirusSkillPointer Instance { get; private set; }

        [SerializeField] GameObject virusPrefab;
        UnitManager unitManager;
        WaveSpawner waveSpawner;
        GameObject dragObj;

        bool isDragging = false;    // ドラッグ中フラグ
        public bool IsDragging => isDragging;

        bool isDragCancel = false;  // 使用キャンセルフラグ

        private bool isActive = true;

        // ドラッグ終了待機
        public static TaskCompletionSource<PointerEventData> dragEndTcs;

        private void OnDestroy()
        {
            Instance = null;
        }

        void Awake()
        {
            if (Instance == null)
                Instance = this;

            dragEndTcs = new TaskCompletionSource<PointerEventData>();

            waveSpawner = FindObjectOfType<WaveSpawner>();
            if (unitManager == null)
                unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();

        }

        private void Start()
        {
            if (waveSpawner.CurrentStage.stageNum == 0 && !waveSpawner.CurrentStage.waveData[waveSpawner.currentWaveIdx].IsTutorial(WaveData.TutorialType.Virus))
            {
                Debug.Log("チュートリアルなので非アクティブ化");
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("チュートリアルでないのでアクティブ化");
                gameObject.SetActive(true);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!waveSpawner.IsStartWave || Time.timeScale == 0)
            {
                dragObj?.SetActive(false);
                return;
            }

            dragEndTcs = new TaskCompletionSource<PointerEventData>();

            if (dragObj == null)
            {
                dragObj = Instantiate(virusPrefab);
                dragObj?.SetActive(false);
            }

            dragObj?.SetActive(true);

            _ = WaitEndDrag.WaitDragEndAsync();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!waveSpawner.IsStartWave || Time.timeScale == 0)
            {
                dragObj?.SetActive(false);
                return;
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            dragObj.transform.position = mousePos;

            isDragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!waveSpawner.IsStartWave || Time.timeScale == 0)
            {
                dragObj?.SetActive(false);
                return;
            }

            dragEndTcs?.TrySetResult(eventData);

            if (isDragCancel)
            {
                dragObj?.SetActive(false);
            }

            isDragging = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isDragging)
            {
                SpriteRenderer sr = dragObj?.GetComponent<SpriteRenderer>();
                sr.enabled = false;
                isDragCancel = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isDragging)
            {
                SpriteRenderer sr = dragObj?.GetComponent<SpriteRenderer>();
                sr.enabled = true;
                isDragCancel = false;
            }
        }

        public void SetSkillActive(bool active)
        {
            isActive = active;
            gameObject.SetActive(active);
        }
    }
}