using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DeadState : IUnitState
{
    UnitController unitController;

    public DeadState(UnitController controller)
    {
        unitController = controller;
    }

    public void Enter()
    {
        unitController.Dead();

        if (unitController.group == UnitGroup.Enemy && !unitController.bossUnit)
        {
            if (unitController.isInfection)
            {
                unitController.DestroyUnit();
            }

            SpriteRenderer sr = unitController.gameObject.GetComponent<SpriteRenderer>();
            sr.sprite = unitController.corpseSprite;

            unitController.unitManager.AddCorpseList(unitController);
        }
        else
        {
            unitController.DestroyUnit();
        }
    }

    public void Update()
    {

    }

    public void Exit()
    {
        // •œŠˆ‚Ìˆ—
        unitController.unitManager.RemoveCorpseList(unitController);
        unitController.unitManager.AddUnitList(unitController, UnitGroup.Player);

        SpriteRenderer sr = unitController.gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = unitController.unitSprite;
    }

    // ŠÔ‚É’B‚µ‚½‚çŠ´õ‚³‚¹‚é
    public IEnumerator Infectioning()
    {
        Debug.Log("Š´õŠJn");

        unitController.unitUI.SetActive(true);

        float timer = 0;

        while(timer < unitController.infecitonTime)
        {
            timer += Time.deltaTime;

            unitController.infectionRateGauge.fillAmount = timer / unitController.infecitonTime;

            yield return null;
        }

        unitController.unitUI.SetActive(false);

        unitController.HealHelth(unitController.maxHp * 0.5f);
    }
}