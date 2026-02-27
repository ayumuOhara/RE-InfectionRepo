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
        this.stats = new UnitStats()
        {
            animatorController = stats.animatorController,
            unitSprite = stats.unitSprite,
            attackEffect = stats.attackEffect,
            unitName = stats.unitName,
            jobType = stats.jobType,
            targetType = stats.targetType,
            maxHp = stats.GetCurrentLevelMaxHp(),
            attackType = stats.attackType,
            hitCnt = stats.hitCnt,
            atk = stats.GetCurrentLevelAtk(),
            atkInterbal = stats.atkInterbal,
            moveSpeed = stats.moveSpeed,
            range = stats.range,
            radius = stats.radius,
            bossUnit = stats.bossUnit,
            attackSe = stats.attackSe,
        };

        movementBase = stats.MovementBase;
        attackBase = stats.AttackBase;

        if (stats.animatorController != null)
            animator.runtimeAnimatorController = (RuntimeAnimatorController)stats.animatorController;

        spriteRenderer.sprite = this.stats.unitSprite;

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
