using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private uint size;
    [SerializeField]
    private PooledObject pooledObject;

    private Stack<PooledObject> stack;

    private void SetupPool()
    {
        stack = new Stack<PooledObject>();
        PooledObject instance = null;

        for(int i = 0; i < size; i++)
        {
            instance = Instantiate(pooledObject);
            instance.gameObject.SetActive(false);
            instance.pool = this;
            stack.Push(instance);
        }
    }

    public PooledObject GetPooledObject()
    {
        if (stack.Count <= 0 || stack == null)
        {
            PooledObject newInstance = Instantiate(pooledObject);
            newInstance.pool = this;

            Debug.Log("新しいインスタンスを返します");

            return newInstance;
        }

        PooledObject nextInstance = stack.Pop();
        nextInstance.gameObject.SetActive(true);

        Debug.Log("プール内のインスタンスを返します");

        return nextInstance;
    }

    public void ReturnToPool(PooledObject pooledObject)
    {
        stack.Push(pooledObject);
        pooledObject.gameObject.SetActive(false);
    }

    private void Awake()
    {
        SetupPool();
    }
}
