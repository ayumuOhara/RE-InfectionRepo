using UnityEngine;

public class EffectDestroy : MonoBehaviour
{
    public void OnEndAnimation()
    {
        Destroy(gameObject);
    }
}
