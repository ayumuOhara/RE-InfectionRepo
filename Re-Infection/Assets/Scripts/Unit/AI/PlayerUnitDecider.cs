using UnityEngine;

public class PlayerUnitDecider : IUnitAIDecider
{
    UnitController unitController;

    public PlayerUnitDecider(UnitController controller)
    {
        unitController = controller;
        Debug.Log("プレイヤーAIを使用します");
    }

    public UnitDicision UnitDecider()
    {
        if(unitController.isDead)
            return UnitDicision.Dead;
        else
            if (unitController.targetObj != null)
                if (unitController.targetDistance <= unitController.range)  // 敵との距離が射程内か
                    return UnitDicision.Attack; // 攻撃する
                else
                    return UnitDicision.MoveToTarget; // ターゲットまで移動
            else
                return UnitDicision.Idle; // その場で待機する
    }
}
