using UnityEngine;

public class RunMovement : MovementBase
{
    public Vector3 Movement(Vector3 myPos, Vector3 targetPos, float moveSpeed = 0)
    {
        Vector3 moveDirection = targetPos - myPos;
        myPos += moveDirection.normalized * moveSpeed * Time.deltaTime;
        return myPos;
    }
}
