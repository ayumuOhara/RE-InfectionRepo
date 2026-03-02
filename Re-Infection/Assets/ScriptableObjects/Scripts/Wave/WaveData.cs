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
        Unit,
        Virus,
        Cannon,
        Boss,
    }

    [Serializable]
    public class Tutorial
    {
        public TutorialType tutorialType;    // チュートリアルの種類
        public GameObject tutorialPrefab;// 表示するチュートリアルUI
    }

    public WaveLevel[] waveLevels;   // ウェーブでスポーンさせるレベルのリスト
    public bool bossWave;            // ボスウェーブか
    public bool isTutorial;            // チュートリアルをするか
    public Tutorial[] tutorial;

    // レベル生成コルーチン
    public IEnumerator SpawnLevels()
    {
        if (isTutorial)
        {
            foreach (var t in tutorial)
            {
                Canvas parent = GameObject.Find("TutorialUI").GetComponent<Canvas>();
                var p = Instantiate(t.tutorialPrefab, parent.transform, parent).GetComponent<RectTransform>();
                p.localPosition = new Vector2(0, 200);

                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                p.GetComponent<Animator>().SetTrigger("Close");

                yield return new WaitForSeconds(1);

                switch (t.tutorialType)
                {
                    case TutorialType.Virus:
                        VirusSkillPointer.isEndVirusTutorial = true;
                        VirusSkillPointer.Instance.SetSkillActive(true);
                        break;
                    case TutorialType.Cannon:
                        CannonSkillPointer.isEndCannonTutorial = true;
                        CannonSkillPointer.Instance.gameObject.SetActive(true);
                        CannonSkillPointer.Instance.SetSkillCoolTimer(0);
                        break;
                    default:
                        break;
                }

                Destroy(p.gameObject);
            }

            isTutorial = false;
        }

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