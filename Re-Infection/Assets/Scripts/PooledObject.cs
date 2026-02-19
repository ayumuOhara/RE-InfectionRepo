using UnityEngine;

public abstract class PooledObject : MonoBehaviour, IPooledObject
{
    public ObjectPool pool;

    public void Release()
    {
        pool.ReturnToPool(this);
    }
}
