using System.Collections;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    private void Awake()
    {
        SetStateManager(new UnitStateManager(this, new PlayerUnitDecider(this)));
    }

    public override void Targetting()
    {
        switch (Stats.targetType)
        {
            case Types.TargetType.UNIT_NEAREST:
                TargetObj = GetTarget.GetNearestTargetUnit(this);
                break;
            case Types.TargetType.UNIT_FARTHEST:
                TargetObj = GetTarget.GetFarthestTargetUnit(this);
                break;
        }
    }

    public override void Move()
    {
        transform.position = Movement.Movement(MyPos, TargetPos, Stats.MoveSpeed);
    }

    public override void Dead()
    {
        base.Dead();
        Destroy(gameObject);
    }
}
