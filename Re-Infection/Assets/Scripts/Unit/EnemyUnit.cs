using UnityEngine;

public class EnemyUnit : UnitBase
{
    GameObject castleObj;

    private void Awake()
    {
        SetStateManager(new UnitStateManager(this, new EnemyUnitDecider(this)));
        castleObj = GameObject.Find("CastleWall");
    }

    public override void Targetting()
    {
        switch (Stats.targetType)
        {
            case Types.TargetType.UNIT_NEAREST:
                var targetN = GetTarget.GetNearestTargetUnit(this);
                TargetObj = targetN != null ? targetN : castleObj;
                break;
            case Types.TargetType.UNIT_FARTHEST:
                var targetF = GetTarget.GetFarthestTargetUnit(this);
                TargetObj = targetF != null ? targetF : castleObj;
                break;
            case Types.TargetType.BUILDING:
                TargetObj = castleObj;
                break;
        }
    }

    public override void Move()
    {
        transform.position = Movement.Movement(MyPos, TargetPos, Stats.MoveSpeed);
    }
}
