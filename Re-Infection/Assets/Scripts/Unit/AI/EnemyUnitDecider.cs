using UnityEngine;

public class EnemyUnitDecider : IUnitAIDecider
{
    UnitController unitController;

    public EnemyUnitDecider(UnitController controller)
    {
        unitController = controller;
    }

    public UnitDicision UnitDecider()
    {
        if (unitController.isDead)
            return UnitDicision.Dead;
        else
            if (unitController.targetObj != null)
                if (GetTarget.TargetInRange(unitController.targetPos, unitController.myPos, unitController.range))  // “G‚Æ‚Ì‹——£‚ªË’ö“à‚©
                    return UnitDicision.Attack; // UŒ‚‚·‚é
                else
                    return UnitDicision.MoveToTarget; // ƒ^[ƒQƒbƒg‚Ü‚ÅˆÚ“®
            else if (GetTarget.TargetInRange(unitController.castlePos, unitController.myPos, unitController.range)) // ‹’“_‚Æ‚Ì‹——£‚ªË’ö“à‚©
                return UnitDicision.Attack; // UŒ‚‚·‚é
            else
                return UnitDicision.MoveToCastle; // ‹’“_‚Ü‚ÅˆÚ“®
    }
}