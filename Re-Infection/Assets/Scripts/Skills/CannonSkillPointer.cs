using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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
        PlayerStatusData playerStatusData;

        [SerializeField] GameObject cannonPrefab;
        UnitManager unitManager;
        WaveSpawner waveSpawner;
        GameObject dragObj;

        [SerializeField] Image cannonPointerFilled;

        bool canUseSkill;

        bool isDragging = false;    // ドラッグ中フラグ
        public bool IsDragging => isDragging;

        bool isDragCancel = false;  // 使用キャンセルフラグ

        // ドラッグ終了待機
        public static TaskCompletionSource<PointerEventData> dragEndTcs;

        void Awake()
        {
            playerStatusData = Resources.Load<PlayerStatusData>("PlayerStatusData");

            if (cannonPointerFilled != null) cannonPointerFilled.fillAmount = 0;
            canUseSkill = true;

            waveSpawner = FindObjectOfType<WaveSpawner>();
            if (unitManager == null)
                unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
            
            StartCoroutine(SkillCoolTimer(playerStatusData.cannonAbility.CoolTime / 2));
        }

        public void OnSkillUse(float coolTime)
        {
            StartCoroutine(SkillCoolTimer(coolTime));
        }

        IEnumerator SkillCoolTimer(float coolTime)
        {
            canUseSkill = false;
            var time = coolTime;

            while (time > 0)
            {
                time -= Time.deltaTime;
                cannonPointerFilled.fillAmount = time / playerStatusData.cannonAbility.CoolTime;

                yield return new WaitUntil(() => waveSpawner.IsStartWave);
            }

            canUseSkill = true;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!waveSpawner.IsStartWave || !canUseSkill) return;

            dragEndTcs = new TaskCompletionSource<PointerEventData>();

            if (dragObj == null)
            {
                dragObj = Instantiate(cannonPrefab);
                dragObj.SetActive(false);
            }

            dragObj.SetActive(true);

            _ = WaitEndDrag.WaitDragEndAsync();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!waveSpawner.IsStartWave || !canUseSkill) return;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            dragObj.transform.position = mousePos;

            isDragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!waveSpawner.IsStartWave || !canUseSkill) return;

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