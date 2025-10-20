using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.EventSystems;

public class MoveState : IUnitState
{
    UnitController unitController;

    public MoveState(UnitController controller)
    {
        unitController = controller;
    }

    public void Enter()
    {

    }

    public void Update()
    {
        if(unitController.group == UnitGroup.Player)
            unitController.gameObject.transform.position += Vector3.up * unitController.moveSpeed * Time.deltaTime;
        if(unitController.group == UnitGroup.Enemy)
            unitController.gameObject.transform.position += Vector3.down * unitController.moveSpeed * Time.deltaTime;
    }

    public void Exit()
    {

    }
}