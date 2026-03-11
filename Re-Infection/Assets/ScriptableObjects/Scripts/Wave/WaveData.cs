using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using VirusPointer;
using CannonPointer;
using Unity.VisualScripting;
using System;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class WaveData : ScriptableObject
{
    public enum TutorialType
    {
        Empty,
        Summon,
        WaveEnd,
        Virus,
        Cannon,
        Boss,
        AoEUnit,
        MagicUnit,
    }

    [Serializable]
    public class Tutorial
    {
        public TutorialType tutorialType;    // チュートリアルの種類
        public GameObject tutorialPrefab;// 表示するチュートリアルUI
    }

    public WaveLevel[] waveLevels;   // ウェーブでスポーンさせるレベルのリスト
    public bool bossWave;            // ボスウェーブか

    public void SetTutorial(TutorialType type)
    {
        switch(type)
        {
            case TutorialType.Summon:   PlayerPrefs.SetInt("SummonTutorial", 1); break;
            case TutorialType.WaveEnd:   PlayerPrefs.SetInt("WaveEndTutorial", 1); break;
            case TutorialType.Virus:  PlayerPrefs.SetInt("VirusTutorial", 1); break;
            case TutorialType.Cannon: PlayerPrefs.SetInt("CannonTutorial", 1); break;
            case TutorialType.Boss:   PlayerPrefs.SetInt("BossTutorial", 1); break;
            case TutorialType.AoEUnit:   PlayerPrefs.SetInt("AoEUnitTutorial", 1); break;
            case TutorialType.MagicUnit:   PlayerPrefs.SetInt("MagicUnitTutorial", 1); break;
            default: break;
        };

        PlayerPrefs.Save();
    }

    public bool IsTutorial(TutorialType type)
    {
        if(PlayerPrefs.GetInt("Stage_Tutorial" + "Clear", 0) != 1) return false;

        return type switch
        {
            TutorialType.Summon => PlayerPrefs.GetInt("SummonTutorial", 0) == 1,
            TutorialType.WaveEnd => PlayerPrefs.GetInt("WaveEndTutorial", 0) == 1,
            TutorialType.Virus => PlayerPrefs.GetInt("VirusTutorial", 0) == 1,
            TutorialType.Cannon => PlayerPrefs.GetInt("CannonTutorial", 0) == 1,
            TutorialType.Boss => PlayerPrefs.GetInt("BossTutorial", 0) == 1,
            TutorialType.AoEUnit => PlayerPrefs.GetInt("AoEUnitTutorial", 0) == 1,
            TutorialType.MagicUnit => PlayerPrefs.GetInt("MagicUnitTutorial", 0) == 1,
            _ => false,
        };
    }

    public Tutorial[] tutorial;

    public static event Action OnStopTime;

    // レベル生成コルーチン
    public IEnumerator SpawnLevels()
    {
        if(tutorial != null)
            yield return StartTutorial();

        // ウェーブ内の全てのレベルを生成するまでループ
        for (int level = 0; level < waveLevels.Length; level++)
        {
            if (level != 0)
                yield return new WaitForSeconds(waveLevels[level].spawnInterbal);

            var currentLevel = waveLevels[level];  // 現在のレベルのデータ取得

            // レベル内のユニットを全て生成
            yield return currentLevel.SpawnLevel();
        }

        yield break;
    }

    private IEnumerator StartTutorial()
    {
        if (tutorial == null) yield break;

        foreach (var t in tutorial)
        {
            if (t == null || IsTutorial(t.tutorialType)) continue;

            yield return new WaitForEndOfFrame();

            OnStopTime?.Invoke();

            Canvas parent = GameObject.Find("TutorialUI").GetComponent<Canvas>();
            parent.enabled = true;

            var p = Instantiate(t.tutorialPrefab, parent.transform, parent).GetComponent<RectTransform>();
            p.localPosition = new Vector2(0, 200);

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            p.GetComponent<Animator>().SetTrigger("Close");

            OnStopTime?.Invoke();

            yield return new WaitForSeconds(1);

            parent.enabled = false;

            switch (t.tutorialType)
            {
                case TutorialType.Virus:
                    VirusSkillPointer.Instance.SetSkillActive(true);
                    break;
                case TutorialType.Cannon:
                    CannonSkillPointer.Instance.gameObject.SetActive(true);
                    CannonSkillPointer.Instance.SetSkillCoolTimer(Resources.Load<PlayerStatusData>("PlayerStatusdata").cannonCoolTimeUpgrade.CoolTime * 0.05f);
                    break;
                default:
                    break;
            }

            SetTutorial(t.tutorialType);
            Destroy(p.gameObject);
        }
    }

    private List<UnitStats> spawnUnitsList;
    // スポーンするユニットの種類
    public List<UnitStats> SpawnUnitsList
    {
        get
        {
            if (spawnUnitsList != null)
            {
                return spawnUnitsList;
            }
            else
            {
                var units = new HashSet<UnitStats>();
                foreach (var level in waveLevels)
                {
                    foreach (var stats in level.levelStats)
                    {
                        units.Add(stats.statsData.unitStats);
                    }
                }

                spawnUnitsList = units.ToList();
                return spawnUnitsList;
            }
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        spawnUnitsList = null;
    }
#endif

    // ウェーブ内の敵の数
    public int waveEnemySum
    {
        get
        {
            int sum = 0;
            foreach (WaveLevel waveLevel in waveLevels)
            {
                foreach (LevelStats levelStats in waveLevel.levelStats)
                {
                    sum += levelStats.spawnCnt;
                }
            }
            return sum;
        }

        private set { }
    }
}