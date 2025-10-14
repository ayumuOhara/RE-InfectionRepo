using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class UnitStats
{
    public GameObject unitObj;
    public float maxHp;
    public float atk;
    public float atkRate;
    public float moveSpeed;
    public float range;
}

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : ScriptableObject
{
    public List<UnitStats> stats;
}
