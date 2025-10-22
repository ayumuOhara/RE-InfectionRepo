using UnityEngine;

public class PlayerUnitDecider : IUnitAIDecider
{
    UnitController unitController;

    public PlayerUnitDecider(UnitController controller)
    {
        unitController = controller;
    }

    public UnitDicision UnitDecider()
    {
        if(unitController.isDead)
            return UnitDicision.Dead;
        else
            if (unitController.targetObj != null)
                if (unitController.targetDistance <= unitController.range)  // “G‚Æ‚Ì‹——£‚ªŽË’ö“à‚©
                    return UnitDicision.Attack; // UŒ‚‚·‚é
                else
                    return UnitDicision.MoveToTarget; // ƒ^[ƒQƒbƒg‚Ü‚ÅˆÚ“®
            else
                return UnitDicision.Idle; // ‚»‚Ìê‚Å‘Ò‹@‚·‚é
    }
}
