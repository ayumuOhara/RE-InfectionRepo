using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "Scriptable Objects/UnitStats")]
public class UnitStats : ScriptableObject
{
    public int unitId;                  // ユニット番号
    public Sprite unitSprite;           // ユニットのスプライト
    public string unitName;             // ユニット名
    public float maxHp;                 // 最大HP
    public float atk;                   // 攻撃力
    public float atkInterbal;           // 攻撃速度
    public float moveSpeed;             // 移動速度
    public float range;                 // 射程距離
    public int summonCost;              // 召喚コスト
    public float infecitonTime;         // 感染するまでの時間
    public float infectioningLifeTime;  // 感染後の生存可能時間
    public float deleteTime;            // 死体が消滅するまでの時間
}
