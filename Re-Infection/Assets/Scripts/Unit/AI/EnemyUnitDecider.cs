using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyUnitDecider : IUnitAIDecider
{
    UnitController unitController;

    public EnemyUnitDecider(UnitController controller)
    {
        unitController = controller;
        Debug.Log("エネミーAIを使用します");
    }

    public UnitDicision UnitDecider()
    {
        if (unitController.isDead)
            return UnitDicision.Dead;
        else
            if (unitController.targetObj != null)
                if (unitController.targetDistance <= unitController.range)  // 敵との距離が射程内か
                    return UnitDicision.Attack; // 攻撃する
                else
                    return UnitDicision.MoveToTarget; // ターゲットまで移動
            else if (unitController.castleDistance <= unitController.range) // 拠点との距離が射程内か
                return UnitDicision.Attack; // 攻撃する
            else
                return UnitDicision.MoveToCastle; // 拠点まで移動
    }
}