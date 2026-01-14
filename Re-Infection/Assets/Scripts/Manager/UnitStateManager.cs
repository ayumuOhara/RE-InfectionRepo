using UnityEngine;

public class UnitStateManager
{
    UnitBase unitController;

    IUnitAIDecider unitAI;

    UnitStateMachine unitStateMachine;
    public UnitStateMachine StateMachine => unitStateMachine;

    public UnitStateManager(UnitBase controller, IUnitAIDecider ai)
    {
        unitController = controller;
        unitStateMachine = new UnitStateMachine(unitController);
        unitStateMachine.Initialize(unitStateMachine.idleState);
        SetUnitAI(ai);
    }

    public void SetUnitAI(IUnitAIDecider aI)
    {
        unitAI = aI;
    }

    // ステート遷移管理AI
    public void StateTransition()
    {
        switch (unitAI.UnitDecider())
        {
            case UnitDicision.Idle:
                unitStateMachine.Transition(unitStateMachine.idleState);
                break;
            case UnitDicision.MoveToTarget:
            case UnitDicision.MoveToCastle:
                unitStateMachine.Transition(unitStateMachine.moveState);
                break;
            case UnitDicision.Attack:
                unitStateMachine.Transition(unitStateMachine.attackState);
                break;
            case UnitDicision.Dead:
                unitStateMachine.Transition(unitStateMachine.deadState);
                break;
        }
    }
}
