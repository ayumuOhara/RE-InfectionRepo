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
        unitController.unitManager.RemoveUnitList(unitController, unitController.group);
        unitController.InstanceObjHeadUp(unitController.deadIconPrefab);

        if (unitController.group == UnitGroup.Enemy)
        {
            SpriteRenderer sr = unitController.gameObject.GetComponent<SpriteRenderer>();
            sr.sprite = unitController.corpseSprite;

            unitController.unitManager.AddCorpseList(unitController);
        }
        else
        {
            unitController.gameObject.SetActive(false);
        }
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}