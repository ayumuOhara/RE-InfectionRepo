using UnityEngine;

public class AttackOfSingle : AttackBase
{
    public override void Attack(UnitBase attacker)
    {
        DamageToTarget(attacker, attacker.TargetObj);

        //Debug.Log($"UŒ‚Ò: {attacker.gameObject.layer} UŒ‚‘ÎÛ”: {cnt}");
    }
}
