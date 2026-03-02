using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Stage", menuName = "Scriptable Objects/Stage")]
public class Stage : ScriptableObject
{
    public int stageNum;              // ステージ数
    public WaveData[] waveData;       // ステージのウェーブデータ
    public bool isClear;              // ステージクリアフラグ

    public int waveClearCoin;
    public int stageClearCoin;
    public int firstClearCoin;

    public Sprite background;       　//背景のsprite

    private List<UnitStats> spawnUnits;
    // スポーンするユニットの種類
    public List<UnitStats> SpawnUnits
    {
        get
        {
            if(spawnUnits != null)
            {
                return spawnUnits;
            }
            else
            {
                var units = new HashSet<UnitStats>();
                foreach (var wave in waveData)
                {
                    foreach (var level in wave.waveLevels)
                    {
                        foreach (var stats in level.levelStats)
                        {
                            units.Add(stats.statsData.unitStats);
                        }
                    }
                }

                spawnUnits = units.ToList();
                return spawnUnits;
            }
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        spawnUnits = null;
    }
#endif
}
