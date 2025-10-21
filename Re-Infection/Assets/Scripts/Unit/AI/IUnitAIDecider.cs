using UnityEngine;

public enum UnitDicision
{
    Idle,
    MoveToTarget,
    MoveToCastle,
    Attack,
    Dead,
}

public interface IUnitAIDecider
{
    public UnitDicision UnitDecider();
}
