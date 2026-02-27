using UnityEngine;

public abstract class PooledObject : MonoBehaviour, IPooledObject
{
    public ObjectPool pool;

    protected bool isReleased = false;

    protected void OnEnable()
    {
        isReleased = false;

        OnSpawn();
    }

    protected virtual void OnSpawn() { }

    public void Release()
    {
        if (isReleased) return;

        isReleased = true;
        pool.ReturnToPool(this);
    }
}
