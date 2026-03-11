using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

namespace CannonPointer
{
    public static class WaitEndDrag
    {
        // ドラッグ終了まで待機
        public static async Task WaitDragEndAsync()
        {
            await CannonSkillPointer.dragEndTcs.Task;
        }
    }

    public class CannonSkillPointer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public static CannonSkillPointer Instance {  get; private set; }

        PlayerStatusData playerStatusData;

        [SerializeField] GameObject cannonPrefab;
        UnitManager unitManager;
        WaveSpawner waveSpawner;
        GameObject dragObj;

        [SerializeField] Image cannonPointerFilled;
        [SerializeField] TextMeshProUGUI coolTimeProgressText;

        [SerializeField, Range(0, 1f)] private float firstCoolTimeRate;

        private bool canUseSkill;

        private bool isDragging = false;    // ドラッグ中フラグ
        public bool IsDragging => isDragging;

        private bool isDragCancel = false;  // 使用キャンセルフラグ

        private bool isActive = true;

        // ドラッグ終了待機
        public static TaskCompletionSource<PointerEventData> dragEndTcs;
        
        private void OnDestroy()
        {
            Instance = null;
        }

        void Awake()
        {
            if(Instance == null)
                Instance = this;

            playerStatusData = Resources.Load<PlayerStatusData>("PlayerStatusData");
            dragEndTcs = new TaskCompletionSource<PointerEventData>();

            if (cannonPointerFilled != null) cannonPointerFilled.fillAmount = 0;
            canUseSkill = true;

            waveSpawner = FindObjectOfType<WaveSpawner>();
            if (unitManager == null)
                unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        }

        private void Start()
        {
            if (waveSpawner.CurrentStage.stageNum == 0 && !waveSpawner.CurrentStage.waveData[waveSpawner.currentWaveIdx].IsTutorial(WaveData.TutorialType.Cannon))
            {
                SetSkillActive(false);

            }
            else
            {
                SetSkillActive(true);
                SetSkillCoolTimer(playerStatusData.cannonCoolTimeUpgrade.CoolTime * (1 - firstCoolTimeRate));
            }
        }

        public void SetSkillCoolTimer(float coolTime)
        {
            StartCoroutine(SkillCoolTimer(coolTime));
        }

        IEnumerator SkillCoolTimer(float coolTime)
        {
            canUseSkill = false;
            cannonPointerFilled.fillAmount = 1;
            var time = coolTime;
            coolTimeProgressText.enabled = true;

            while (time >= 0)
            {
                CoolTimeProgress(time);

                yield return new WaitUntil(() => waveSpawner.IsStartWave);

                time -= Time.deltaTime;
            }

            FindObjectOfType<SEManager>().PlaySE(SEManager.SEType.CanExplosion);
            cannonPointerFilled.fillAmount = 0;
            canUseSkill = true;
            coolTimeProgressText.enabled = false;
        }

        private void CoolTimeProgress(float time)
        {
            coolTimeProgressText.text = (100 - (time / playerStatusData.cannonCoolTimeUpgrade.CoolTime) * 100).ToString("F0") + " <size=25>%";
            cannonPointerFilled.fillAmount = time / playerStatusData.cannonCoolTimeUpgrade.CoolTime;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            dragEndTcs = new TaskCompletionSource<PointerEventData>();
            if (!waveSpawner.IsStartWave || !canUseSkill || Time.timeScale == 0)
            {
                dragObj?.SetActive(false);
                return;
            }

            if (dragObj == null)
            {
                dragObj = Instantiate(cannonPrefab);
                dragObj?.SetActive(false);
            }

            dragObj?.SetActive(true);

            _ = WaitEndDrag.WaitDragEndAsync();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!waveSpawner.IsStartWave || !canUseSkill || Time.timeScale == 0)
            {
                dragObj?.SetActive(false);
                return;
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            if(dragObj != null)
                dragObj.transform.position = mousePos;

            isDragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!waveSpawner.IsStartWave || !canUseSkill || Time.timeScale == 0)
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