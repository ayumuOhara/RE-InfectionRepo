using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DeadState : IUnitState
{
    UnitBase unitController;

    public DeadState(UnitBase controller)
    {
        unitController = controller;
    }

    public void Enter()
    {

    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}