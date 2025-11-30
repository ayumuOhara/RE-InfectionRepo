using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfectioningUnit : MonoBehaviour
{
    UnitManager unitManager;

    [SerializeField] VirusStats virusStats;

    const float VISUAL_RANGE = 1.7f;

    private void Awake()
    {
        transform.localScale = new Vector3(virusStats.infectionRange * VISUAL_RANGE, virusStats.infectionRange * VISUAL_RANGE);

        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
    }

    async void OnEnable()
    {
        await WaitEndDrag.WaitDragEndAsync();
        var targetUnits = new List<EnemyUnit>(unitManager.GetCorpseList());

        if (targetUnits.Count <= 0 || targetUnits == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(AllTargetInfection(targetUnits));
        }
    }

    // 取得したターゲットを感染
    IEnumerator AllTargetInfection(List<EnemyUnit> targetUnits)
    {
        foreach (EnemyUnit target in targetUnits)
        {
            // 範囲内にいるターゲット全てに感染
            if (GetTarget.TargetInRange(target.gameObject.transform.position, transform.position, virusStats.infectionRange))
            {
                target.IsInfectioning = true;
            }

            yield return null;
        }

        // 処理終了後、非アクティブ化
        gameObject.SetActive(false);
    }
}
