using UnityEngine;

public class EnemyUnitDecider : IUnitAIDecider
{
    UnitBase unitBase;

    public EnemyUnitDecider(UnitBase unitBase)
    {
        this.unitBase = unitBase;
    }

    public UnitDicision UnitDecider()
    {
        if (unitBase.IsDead)
            return UnitDicision.Dead;
        else
            if (unitBase.TargetPos != null)
                if (GetTarget.TargetInRange(unitBase.TargetPos, unitBase.MyPos, unitBase.Stats.range))  // “G‚Æ‚Ì‹——£‚ªË’ö“à‚©
                    return UnitDicision.Attack; // UŒ‚‚·‚é
                else
                    return UnitDicision.MoveToTarget; // ƒ^[ƒQƒbƒg‚Ü‚ÅˆÚ“®
            else if (GetTarget.TargetInRange(unitBase.TargetPos, unitBase.MyPos, unitBase.Stats.range)) // ‹’“_‚Æ‚Ì‹——£‚ªË’ö“à‚©
                return UnitDicision.Attack; // UŒ‚‚·‚é
            else
                return UnitDicision.MoveToCastle; // ‹’“_‚Ü‚ÅˆÚ“®
    }
}