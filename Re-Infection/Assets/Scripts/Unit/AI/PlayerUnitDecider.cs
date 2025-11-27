using UnityEngine;

public class PlayerUnitDecider : IUnitAIDecider
{
    UnitBase unitBase;

    public PlayerUnitDecider(UnitBase controller)
    {
        unitBase = controller;
    }

    public UnitDicision UnitDecider()
    {
        if(unitBase.IsDead)
            return UnitDicision.Dead;
        else
            if (unitBase.TargetObj != null)
                if (GetTarget.TargetInRange(unitBase.TargetPos, unitBase.MyPos, unitBase.Stats.range))  // 敵との距離が射程内か
                    return UnitDicision.Attack; // 攻撃する
                else
                    return UnitDicision.MoveToTarget; // ターゲットまで移動
            else
                return UnitDicision.Idle; // その場で待機する
    }
}
