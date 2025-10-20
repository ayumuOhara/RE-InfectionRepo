using UnityEngine;

public class UnitStateManager
{
    UnitController unitController;

    UnitStateMachine unitStateMachine;
    public UnitStateMachine StateMachine => unitStateMachine;

    public UnitStateManager(UnitController controller)
    {
        unitController = controller;
        unitStateMachine = new UnitStateMachine(unitController);
        unitStateMachine.Initialize(unitStateMachine.idleState);
    }

    // 敵がいないかどうか
    public bool isAllTargetDefeated =>
        unitController.group == UnitGroup.Player ? unitController.unitManager.IsAllEnemyDefeated : unitController.unitManager.IsAllUnitDefeated;
    
    // 敵が射程範囲内かどうか
    public bool isTargetInRange => Vector3.Distance(unitController.targetObj.transform.position, unitController.gameObject.transform.position) <= unitController.range;

    // 拠点が射程内かどうか(エネミー専用)
    //public bool isBaseInRange => Vector3.Distance(base.transform.position, unitController.transform.position);

    // ステート遷移管理
    public void StateTransition()
    {
        if (unitController.isDead)
            unitStateMachine.Transition(unitStateMachine.deadState);

        switch (unitStateMachine.CurrentState)
        {
            case IdleState:
                if (isAllTargetDefeated)
                    unitStateMachine.Transition(unitStateMachine.moveState);
                else if (!isTargetInRange)
                    unitStateMachine.Transition(unitStateMachine.moveState);
                else if (isTargetInRange)
                    unitStateMachine.Transition(unitStateMachine.attackState);
                break;
            case MoveState:
                if (isAllTargetDefeated)
                    unitStateMachine.Transition(unitStateMachine.idleState);
                else if (isTargetInRange)
                    unitStateMachine.Transition(unitStateMachine.attackState);
                break;
            case AttackState:
                if (isAllTargetDefeated)
                    unitStateMachine.Transition(unitStateMachine.idleState);
                else if (!isTargetInRange)
                    unitStateMachine.Transition(unitStateMachine.moveState);
                break;
            case DeadState:
                if (!unitController.isDead)
                    unitStateMachine.Transition(unitStateMachine.idleState);
                break;
        }
    }
}
