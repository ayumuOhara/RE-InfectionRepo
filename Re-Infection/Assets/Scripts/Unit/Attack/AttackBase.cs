using UnityEngine;

public abstract class AttackBase
{
    public virtual void DamageToTarget(UnitBase attacker, GameObject target)
    {
        if (target.tag == "Castle")
        {
            CastleWallManager castle = target.GetComponent<CastleWallManager>();
            castle.TakeDamage(attacker.Stats.atk);
        }
        else
        {
            UnitBase unit = target.GetComponent<UnitBase>();
            unit.Damage(attacker.Stats.atk);

            //Debug.Log($"{attacker.Stats.unitName}‚ª{target.GetComponent<UnitBase>().Stats.unitName}‚É{attacker.Stats.atk}ƒ_ƒ[ƒW");
        }
    }

    public virtual void Attack(UnitBase attacker)
    {
        // UŒ‚ˆ—
        attacker.InstanceEffect(attacker.TargetObj.transform.position);
    }
}
