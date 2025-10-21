using UnityEngine;

public class UnitStateMachine
{
    public IUnitState CurrentState { get; private set; }

    public IdleState idleState;
    public MoveState moveState;
    public AttackState attackState;
    public DeadState deadState;

    public UnitStateMachine(UnitController controller)
    {
        idleState = new IdleState(controller);
        moveState = new MoveState(controller);
        attackState = new AttackState(controller);
        deadState = new DeadState(controller);
    }

    public void Initialize(IUnitState state)
    {
        CurrentState = state;
        state.Enter();
    }

    public void Transition(IUnitState state)
    {
        if (CurrentState == state) return;

        Debug.Log($"NextState : {state}");

        CurrentState.Exit();
        CurrentState = state;
        state.Enter();
    }

    public void Update()
    {
        Debug.Log($"CurrentState : {CurrentState}");
        CurrentState?.Update();
    }
}
