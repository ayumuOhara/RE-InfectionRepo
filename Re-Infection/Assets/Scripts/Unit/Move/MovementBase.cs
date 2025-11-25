using UnityEngine;

public interface MovementBase
{
    public Vector3 Movement(Vector3 myPos, Vector3 targetPos, float moveSpeed = 0);
}
