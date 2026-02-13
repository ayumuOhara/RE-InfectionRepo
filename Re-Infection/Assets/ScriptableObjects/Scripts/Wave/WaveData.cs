using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class WaveData : ScriptableObject
{
    public WaveLevel[] waveLevels;   // ウェーブでスポーンさせるレベルのリスト
    public bool bossWave;            // ボスウェーブか
    public bool tutorial;            // チュートリアルをするか
    public GameObject tutorialPrefab;// 表示するチュートリアルUI

    // レベル生成コルーチン
    public IEnumerator SpawnLevels()
    {
        if (tutorial)
        {
            Canvas parent = GameObject.Find("TutorialUI").GetComponent<Canvas>();
            var p = Instantiate(tutorialPrefab, parent.transform, parent).GetComponent<RectTransform>();
            p.localPosition = new Vector2(0, 200);

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            p.GetComponent<Animator>().SetTrigger("Close");
            tutorial = false;

            yield return new WaitForSeconds(1.5f);
            Destroy(p.gameObject);
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