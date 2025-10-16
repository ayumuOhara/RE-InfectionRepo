using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class WaveData : ScriptableObject
{
    public WaveLevel[] waveLevels;   // ウェーブでスポーンさせるレベルのリスト
    public float spawnInterbal;  // スポーンする時間
    public int rewardCost;       // ウェーブクリア後に獲得できるコスト
}