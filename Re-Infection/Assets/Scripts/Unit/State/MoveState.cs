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

    }

    public void Update()
    {
        unitBase.Move();
    }

    public void Exit()
    {

    }
}