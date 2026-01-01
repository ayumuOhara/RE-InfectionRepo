using UnityEngine;

public class DeadState : IUnitState
{
    UnitBase unit;

    public DeadState(UnitBase unit)
    {
        this.unit = unit;
    }

    public void Enter()
    {
        unit.Dead();
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}