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
            if (unitBase.TargetObj != null)
                if (GetTarget.TargetInRange(unitBase.TargetPos, unitBase.MyPos, unitBase.Stats.range))  // 敵との距離が射程内か
                    return UnitDicision.Attack; // 攻撃する
                else
                    return UnitDicision.MoveToTarget; // ターゲットまで移動
            else
                return UnitDicision.MoveToCastle; // 拠点まで移動
    }
}