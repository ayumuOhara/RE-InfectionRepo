using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VirusPointer;
using System;

public class InfectioningUnit : MonoBehaviour
{
    PlayerStatusData playerStatusData;

    public static event Action<float, float> OnInfection;

    UnitManager unitManager;

    [SerializeField] private float infectionRange;
    [SerializeField] LayerMask skillTargetLayer;
    [SerializeField] GameObject infectionEffect;

    const float VISUAL_RANGE = 2f;

    private void Awake()
    {
        playerStatusData = Resources.Load<PlayerStatusData>("PlayerStatusData");

        transform.localScale = new Vector3(infectionRange * VISUAL_RANGE, infectionRange * VISUAL_RANGE);

        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
    }

    async private void Update()
    {
        //UnitBase.DrawDebugCircle(transform.position, virusStats.infectionRange, Color.purple, 0.5f);

        await WaitEndDrag.WaitDragEndAsync();
        if (unitManager.GetCorpseList().Count <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        var targetUnits = Physics2D.OverlapCircleAll(transform.position, infectionRange, skillTargetLayer);

        if (targetUnits.Length <= 0 || targetUnits == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            AllTargetInfection(targetUnits);
        }
    }

    // 取得したターゲットを感染
    void AllTargetInfection(Collider2D[] targetUnits)
    {
        var effectGenerated = false;

        foreach (Collider2D target in targetUnits)
        {
            EnemyUnit enemy = target.GetComponent<EnemyUnit>();

            // 範囲内にいるターゲット全てに感染
            if (enemy?.IsDead == true && enemy.IsInfectioning == false)
            {
                OnInfection += enemy.StartInfection;
                OnInfection?.Invoke(playerStatusData.virusUpgrade.VirusStats.infectionTime, playerStatusData.virusUpgrade.VirusStats.reviveHealthRate);
                OnInfection -= enemy.StartInfection;

                if (!effectGenerated)
                {
                    effectGenerated = true;
                    Instantiate(infectionEffect, transform.position, Quaternion.identity);
                }
            }
        }

        // 処理終了後、非アクティブ化
        gameObject.SetActive(false);
    }
}
