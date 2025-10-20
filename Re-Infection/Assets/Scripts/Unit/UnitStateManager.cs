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

    // 死亡判定
    public bool isDead => unitController.currentHp <= 0;

    // 敵がいないかどうか
    public bool isAllTargetDefeated =>
        unitController.group == UnitGroup.Player ? 
        unitController.unitManager.IsAllUnitDefeated : 
        unitController.unitManager.IsAllEnemyDefeated;
    
    // 敵が射程範囲内かどうか
    public bool isTargetInRange =>
        Vector3.Distance(GetTarget.GetTargetObj(unitController.group == UnitGroup.Player ? UnitGroup.Enemy : UnitGroup.Player,
                             unitController.gameObject.transform.position).transform.position,
                         unitController.gameObject.transform.position) <= unitController.range;

    // 拠点が射程内かどうか(エネミー専用)
    //public bool isBaseInRange => Vector3.Distance(base.transform.position, unitController.transform.position);

    // ステート遷移管理
    public void StateTransition()
    {
        if (isDead)
            unitStateMachine.Transition(unitStateMachine.deadState);

        switch (unitStateMachine.CurrentState)
        {
            case IdleState:
                if (isAllTargetDefeated)
                    break;
                if (!isTargetInRange)
                    unitStateMachine.Transition(unitStateMachine.moveState);
                if (isTargetInRange)
                    unitStateMachine.Transition(unitStateMachine.attackState);
                break;
            case MoveState:
                if (isAllTargetDefeated)
                    unitStateMachine.Transition(unitStateMachine.idleState);
                if (isTargetInRange)
                    unitStateMachine.Transition(unitStateMachine.attackState);
                break;
            case AttackState:
                if (isAllTargetDefeated)
                    unitStateMachine.Transition(unitStateMachine.idleState);
                if (!isTargetInRange)
                    unitStateMachine.Transition(unitStateMachine.moveState);
                break;
            case DeadState:
                if (!isDead)
                    unitStateMachine.Transition(unitStateMachine.idleState);
                break;
        }
    }
}
