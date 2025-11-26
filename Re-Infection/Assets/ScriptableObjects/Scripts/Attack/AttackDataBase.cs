using UnityEngine;

[System.Serializable]
public abstract class AttackDataBase : ScriptableObject
{
    public virtual void Attack(LayerMask targetLayer, UnitBase attacker = null, float damage = 0, float range = 0)
    {
        // UŒ‚ˆ—
    }
}
