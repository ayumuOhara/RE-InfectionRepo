using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "Scriptable Objects/UnitStats")]
public class UnitStats : ScriptableObject
{
    public Sprite unitSprite;   // ユニットのスプライト
    public float maxHp;         // 最大HP
    public float atk;           // 攻撃力
    public float atkRate;       // 攻撃速度
    public float moveSpeed;     // 移動速度
    public float range;         // 射程距離
    public int summonCost;      // 召喚コスト
}
