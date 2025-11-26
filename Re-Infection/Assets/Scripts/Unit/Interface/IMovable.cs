using UnityEngine;

public interface IMovable
{
    public Vector3 MyPos => Vector3.zero;
    public Vector3 TargetPos { get; set; }

    // ˆÚ“®
    public void Move();
}
