using System.Collections;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    private void Awake()
    {
        stateManager = new UnitStateManager(this, new PlayerUnitDecider(this));
        StartCoroutine(Targetting());
    }

    IEnumerator Targetting()
    {
        while (true)
        {
            yield return new WaitUntil(() => GetTarget.GetEnemyUnit(MyPos) != null);

            TargetPos = GetTarget.GetEnemyUnit(MyPos).transform.position;
            yield return null;
        }
    }

    public override void Move()
    {
        transform.position = Movement.Movement(MyPos, TargetPos, Stats.MoveSpeed);
    }
}
