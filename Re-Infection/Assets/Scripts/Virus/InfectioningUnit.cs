using UnityEngine;
using System.Collections.Generic;

public class InfectioningUnit : MonoBehaviour
{
    [SerializeField] VirusStats virusStats;
    UnitManager unitManager;
    List<UnitController> targetUnits = new List<UnitController>();

    private void Awake()
    {
        transform.localScale = new Vector3(virusStats.infectionRange * 1.4f, virusStats.infectionRange * 1.4f);

        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
    }

    async void OnEnable()
    {
        await WaitEndDrag.WaitDragEndAsync();
        targetUnits = new List<UnitController>(unitManager.GetCorpseList());

        foreach (UnitController target in targetUnits)
        {
            if (Vector3.Distance(target.gameObject.transform.position, transform.position) < virusStats.infectionRange)
            {
                //target.Infection();
                Debug.Log("Š´õŠ®—¹");
            }
        }

        gameObject.SetActive(false);
    }
}
