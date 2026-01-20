using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfectioningUnit : MonoBehaviour
{
    UnitManager unitManager;

    [SerializeField] VirusStats virusStats;
    [SerializeField] LayerMask skillTargetLayer;
    [SerializeField] GameObject infectionEffect;

    const float VISUAL_RANGE = 2f;

    private void Awake()
    {
        transform.localScale = new Vector3(virusStats.infectionRange * VISUAL_RANGE, virusStats.infectionRange * VISUAL_RANGE);

        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
    }

    async void OnEnable()
    {
        await WaitEndDrag.WaitDragEndAsync();
        if (unitManager.GetCorpseList().Count <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        var targetUnits = Physics2D.OverlapCircleAll(transform.position, virusStats.infectionRange, skillTargetLayer);

        if (targetUnits.Length <= 0 || targetUnits == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(AllTargetInfection(targetUnits));
        }
    }

    private void Update()
    {
        //UnitBase.DrawDebugCircle(transform.position, virusStats.infectionRange, Color.purple, 0.5f);
    }

    // 取得したターゲットを感染
    IEnumerator AllTargetInfection(Collider2D[] targetUnits)
    {
        var effectGenerated = false;

        foreach (Collider2D target in targetUnits)
        {
            // 範囲内にいるターゲット全てに感染
            if (target.GetComponent<EnemyUnit>()?.IsDead == true && target.GetComponent<EnemyUnit>().IsInfectioning == false)
            {
                target.GetComponent<EnemyUnit>().IsInfectioning = true;
                if (!effectGenerated)
                {
                    effectGenerated = true;
                    Instantiate(infectionEffect, transform.position, Quaternion.identity);
                }
            }

            yield return null;
        }

        // 処理終了後、非アクティブ化
        gameObject.SetActive(false);
    }
}
