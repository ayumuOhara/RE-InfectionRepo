using UnityEngine;

public class UnitStateManager
{
    UnitController unitController;

    IUnitAIDecider unitAI;

    UnitStateMachine unitStateMachine;
    public UnitStateMachine StateMachine => unitStateMachine;

    public UnitStateManager(UnitController controller)
    {
        unitController = controller;
        unitStateMachine = new UnitStateMachine(unitController);
        unitStateMachine.Initialize(unitStateMachine.idleState);

        if(unitController.group == UnitGroup.Player)
            unitAI = new PlayerUnitDecider(controller);
        if(unitController.group == UnitGroup.Enemy)
            unitAI = new EnemyUnitDecider(controller);
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
