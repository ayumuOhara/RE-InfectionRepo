using UnityEngine;
using UnityEngine.UI;

public class LoopScrollImage : MonoBehaviour
{
    private const float maxOffset = 1f;
    private const string property_Mane = "_MainTex";

    [SerializeField] private Vector2 offsetSpeed;
    [SerializeField] private Material material;

    private void Reset()
    {
        // コンポーネントがアタッチされたタイミングでマテリアルを取得する
        if (TryGetComponent(out Image image))
        {
            material = image.material;
        }
    }

    private void Update()
    {
        if (material != null)
        {
            var x = Mathf.Repeat(Time.time * offsetSpeed.x, maxOffset);
            var y = Mathf.Repeat(Time.time * offsetSpeed.y, maxOffset);
            var offset = new Vector2(x, y);
            material.SetTextureOffset(property_Mane, offset);
        }
    }

    private void OnDestroy()
    {
        // オブジェクトが破棄されるタイミングに位置をリセットする
        if (material != null)
        {
            material.SetTextureOffset(property_Mane, Vector2.zero);
        }
    }
}