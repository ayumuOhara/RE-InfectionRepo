using System.Collections;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    public override void Initialize(UnitStats stats)
    {
        base.Initialize(stats);

        FindObjectOfType<UnitManager>().AddPlayerUnitList(this);
    }

    public override void SetStats(UnitStats stats)
    {
        base.SetStats(stats);

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
        FindObjectOfType<UnitManager>().RemovePlayerUnitList(this);

        base.Dead();
    }
}
