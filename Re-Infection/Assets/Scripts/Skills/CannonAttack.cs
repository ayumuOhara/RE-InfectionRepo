using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CannonPointer;

public class CannonAttack : MonoBehaviour
{
    PlayerStatusData playerStatusData;

    public static event Action<float> OnSkillUsed;

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

    async void OnEnable()
    {
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
        //UnitBase.DrawDebugCircle(transform.position, cannonSkillStats.cannonRadius, Color.red, 0.5f);
    }

    // 取得したターゲットにダメージ
    void AllTargetDamage(Collider2D[] targetUnits)
    {
        Instantiate(cannonEffect, transform.position + new Vector3(0, -1.7f, 0), Quaternion.identity);
        audioSource.PlayOneShot(cannonSE);

        foreach (Collider2D target in targetUnits)
        {
            var enemy = target.GetComponent<EnemyUnit>();

            // 範囲内にいるターゲット全てにダメージ
            if (enemy.IsDead == false)
            {
                enemy.Damage(playerStatusData.cannonUpgrade.Damage);
                // 倒した敵の死体を複製(ボスユニット除外)
                if (enemy.CurrentHealth <= 0 && !enemy.Stats.bossUnit)
                {
                    EnemyUnit clone = Instantiate(target.gameObject, target.transform.position + new Vector3(0.1f, 0, 0), Quaternion.identity).GetComponent<EnemyUnit>();
                    clone.Initialize(enemy.Stats, true);
                }
            }
        }

        OnSkillUsed += cannonSkillPointer.OnSkillUse;
        OnSkillUsed?.Invoke(playerStatusData.cannonUpgrade.CoolTime);
        OnSkillUsed -= cannonSkillPointer.OnSkillUse;

        // 処理終了後、非アクティブ化
        gameObject.SetActive(false);
    }
}