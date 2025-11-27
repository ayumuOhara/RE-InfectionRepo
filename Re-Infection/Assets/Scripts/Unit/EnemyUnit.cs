using UnityEngine;

public class EnemyUnit : UnitBase
{
    GameObject castleObj;

    private void Awake()
    {
        stateManager = new UnitStateManager(this, new EnemyUnitDecider(this));
        castleObj = GameObject.Find("CastleWall");
    }

    public override void Move()
    {
        switch (Stats.targetType)
        {
            case Types.TargetType.BOTH:
                TargetPos = GetTarget.NearestTarget(GetTarget.GetPlayerUnit(MyPos), castleObj, MyPos).transform.position;
                break;
            case Types.TargetType.UNIT:
                TargetPos = GetTarget.GetPlayerUnit(MyPos).transform.position;
                break;
            case Types.TargetType.BUILDING:
                TargetPos = GameObject.Find("CastleWall").transform.position;
                break;
        }

        transform.position = Movement.Movement(MyPos, TargetPos, Stats.MoveSpeed);
    }
}
