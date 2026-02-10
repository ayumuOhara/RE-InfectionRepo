using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CannonPointer;
using Unity.VisualScripting;

public class CannonAttack : MonoBehaviour
{
    PlayerStatusData playerStatusData;

    public static event Action<float> OnSkillUsed;
    private bool endSkill = true;

    UnitManager unitManager;
    CannonSkillPointer cannonSkillPointer;

    AudioSource audioSource;

    [SerializeField] float cannonRadius;
    [SerializeField] LayerMask skillTargetLayer;
    [SerializeField] GameObject cannonEffect;
    [SerializeField] AudioClip cannonSE;

    const float VISUAL_RANGE = 2f;

    private void Awake()
    {
        playerStatusData = Resources.Load<PlayerStatusData>("PlayerStatusData");

        cannonSkillPointer = GameObject.Find("CannonSkillPointer").GetComponent<CannonSkillPointer>();
        transform.localScale = new Vector3(cannonRadius * VISUAL_RANGE, cannonRadius * VISUAL_RANGE);

        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        audioSource = GameObject.Find("WaveSpawner").GetComponent<AudioSource>();
    }

    async private void OnEnable()
    {
        endSkill = false;
        await WaitEndDrag.WaitDragEndAsync();
        if (unitManager.EnemyCnt <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        var targetUnits = Physics2D.OverlapCircleAll(transform.position, cannonRadius, skillTargetLayer);

        if (targetUnits.Length <= 0 || targetUnits == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            AllTargetDamage(targetUnits);
        }
    }

    private void Update()
    {
        if (endSkill)
        {
            gameObject.SetActive(false);
        }
    }

    // 取得したターゲットにダメージ
    void AllTargetDamage(Collider2D[] targetUnits)
    {
        endSkill = false;

        Instantiate(cannonEffect, transform.position + new Vector3(0, -1.7f, 0), Quaternion.identity);
        audioSource.PlayOneShot(cannonSE);

        foreach (Collider2D target in targetUnits)
        {
            var enemy = target.GetComponent<EnemyUnit>();

            // 範囲内にいるターゲット全てにダメージ
            if (enemy.IsDead == false)
            {
                enemy.Damage(playerStatusData.cannonDamageUpgrade.Damage);
                // 倒した敵の死体を複製(ボスユニット除外)
                if (enemy.CurrentHealth <= 0 && !enemy.Stats.bossUnit)
                {
                    EnemyUnit clone = Instantiate(target.gameObject, target.transform.position + new Vector3(0.1f, 0, 0), Quaternion.identity).GetComponent<EnemyUnit>();
                    clone.Initialize(enemy.Stats, true);
                }
            }
        }

        OnSkillUsed += cannonSkillPointer.OnSkillUse;
        OnSkillUsed?.Invoke(playerStatusData.cannonCoolTimeUpgrade.CoolTime);
        OnSkillUsed -= cannonSkillPointer.OnSkillUse;

        endSkill = true;
    }
}