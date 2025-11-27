using UnityEngine;

public class AttackMeleeOfPoint : AttackBase
{
    public override void Attack(UnitBase attacker)
    {
        UnitBase unit = GetTarget.GetNearestTargetUnit(attacker).GetComponent<UnitBase>();
        unit.Damage(attacker.Stats.atk);

        //Debug.Log($"UŒ‚Ò: {attacker.gameObject.layer} UŒ‚‘ÎÛ”: {cnt}");
    }
}
