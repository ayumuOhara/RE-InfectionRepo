using UnityEngine;
using System.Collections.Generic;

public class InfectioningUnit : MonoBehaviour
{
    UnitManager unitManager;

    [SerializeField] VirusStats virusStats;
    List<UnitController> targetUnits = new List<UnitController>();

    const float VISUAL_RANGE = 1.7f;

    private void Awake()
    {
        transform.localScale = new Vector3(virusStats.infectionRange * VISUAL_RANGE, virusStats.infectionRange * VISUAL_RANGE);

        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
    }

    async void OnEnable()
    {
        await WaitEndDrag.WaitDragEndAsync();
        targetUnits = new List<UnitController>(unitManager.GetCorpseList());

        if (targetUnits.Count <= 0 || targetUnits == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            foreach (UnitController target in targetUnits)
            {
                if (Vector3.Distance(target.gameObject.transform.position, transform.position) < virusStats.infectionRange)
                {
                    target.Infection();
                }
            }
        }

        gameObject.SetActive(false);
    }
}
