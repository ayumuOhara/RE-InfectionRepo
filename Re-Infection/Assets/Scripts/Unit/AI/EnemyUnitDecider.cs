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
                if (unitController.targetDistance <= unitController.range)  // “G‚Æ‚Ì‹——£‚ªŽË’ö“à‚©
                    return UnitDicision.Attack; // UŒ‚‚·‚é
                else
                    return UnitDicision.MoveToTarget; // ƒ^[ƒQƒbƒg‚Ü‚ÅˆÚ“®
            else if (unitController.castleDistance <= unitController.range) // ‹’“_‚Æ‚Ì‹——£‚ªŽË’ö“à‚©
                return UnitDicision.Attack; // UŒ‚‚·‚é
            else
                return UnitDicision.MoveToCastle; // ‹’“_‚Ü‚ÅˆÚ“®
    }
}