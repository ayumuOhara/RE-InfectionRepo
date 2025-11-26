using UnityEngine;

[CreateAssetMenu(fileName = "AttackOfPoint", menuName = "Scriptable Objects/AttackOfPoint")]
public class AttackOfPoint : AttackDataBase
{
    public override void Attack(LayerMask targetLayer, UnitBase attacker, float damage, float range)
    {
        //Debug.Log($"UŒ‚Ò: {attacker.gameObject.layer} UŒ‚‘ÎÛ”: {cnt}");
    }
}
