using UnityEngine;

public class UnitStateMachine
{
    public IUnitState CurrentState { get; private set; }

    public IdleState idleState;
    public MoveState moveState;
    public AttackState attackState;
    public DeadState deadState;

    public UnitStateMachine(UnitBase unitBase)
    {
        idleState = new IdleState(unitBase);
        moveState = new MoveState(unitBase);
        attackState = new AttackState(unitBase);
        deadState = new DeadState(unitBase);
    }

    public void Initialize(IUnitState state)
    {
        CurrentState = state;
        state.Enter();
    }

    public void Transition(IUnitState state)
    {
        if (CurrentState == state) return;

        CurrentState.Exit();
        CurrentState = state;
        state.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}
