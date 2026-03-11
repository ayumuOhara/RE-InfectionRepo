using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Stage", menuName = "Scriptable Objects/Stage")]
public class Stage : ScriptableObject
{
    public int stageNum;                // ステージ数
    public WaveData[] waveData;         // ステージのウェーブデータ
    
    private bool _isClear;
    public bool isClear
    {
        get
        {
            return PlayerPrefs.GetInt(name + "Clear", 0) == 1;
        }
    }
    
    private bool _isOpened;
    public bool isOpened
    {
        get
        {
            return PlayerPrefs.GetInt(name + "Open", 0) == 1;
        }
    }

    public void SetIsClear(bool clear)
    {
        _isClear = clear;
        PlayerPrefs.SetInt(name + "Clear", _isClear ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    public void SetIsOpend(bool open)
    {
        _isOpened = open;
        PlayerPrefs.SetInt(name + "Open", _isOpened ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetStageBestClearTime(float time)
    {
        PlayerPrefs.SetFloat(name + "Time", time < GetStageClearTime() ? time : GetStageClearTime());
        PlayerPrefs.Save();
    }

    public float GetStageClearTime()
    {
        return PlayerPrefs.GetFloat(name + "Time", 999999);
    }

    public string GetStageClearTimeText()
    {
        float time = GetStageClearTime();

        int minutes = (int)(time / 60f);
        int secondes = (int)time - (minutes * 60);

        return minutes.ToString("D2") + ":" + secondes.ToString("D2");
    }

    public UnitStatsData[] unlockUnits; // ステージクリアでアンロックされるユニット

    public int waveClearCoin;
    public int stageClearCoin;
    public int firstClearCoin;

    public void SetUnitsCanUnLock()
    {
        if (unlockUnits != null || unlockUnits.Length <= 0)
        {
            foreach (var unit in unlockUnits)
            {
                unit.unitStats.UnitUnLock();
            }
        }       
    }

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
