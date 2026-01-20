using UnityEngine;
using UnityEngine.EventSystems;

public class MoveState : IUnitState
{
    UnitBase unitBase;

    public MoveState(UnitBase unitBase)
    {
        this.unitBase = unitBase;
    }

    public void Enter()
    {
        if (unitBase.animator.enabled) unitBase.animator.SetBool("Move", true);
    }

    public void Update()
    {
        unitBase.Move();
    }

    public void Exit()
    {
        if (unitBase.animator.enabled) unitBase.animator.SetBool("Move", false);
    }
}