using UnityEngine;

public class AttackState : IUnitState
{
    UnitBase unitBase;
    float atkTimer = 0; // 攻撃タイマー

    public AttackState(UnitBase unitBase)
    {
        this.unitBase = unitBase;
    }

    public void Enter()
    {

    }

    public void Update()
    {
        atkTimer += Time.deltaTime;

        if (atkTimer >= unitBase.Stats.atkInterbal)
        {
            GetTarget.TargetInRange(unitBase.TargetPos, unitBase.MyPos, unitBase.Stats.range);
            unitBase.Attack();

            atkTimer = 0;
        }
    }

    public void Exit()
    {

    }
}