using Unity.VisualScripting;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    [SerializeField] UnitData unitData;

    [SerializeField] GameObject unitObj;
    [SerializeField] UnitManager unitManager;

    float generateInterbal; // 生成後の経過時間
    float generateTime = 5.0f;     // 生成する間隔

    Vector3 enemyUnitGeneratePos = new Vector3(0, 5.5f, 0);    // エネミーユニットの生成座標

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generateInterbal = 0;
        GenerateUnit();
    }

    // Update is called once per frame
    void Update()
    {
        generateInterbal += Time.deltaTime;

        if (generateInterbal > generateTime)
        {
            generateInterbal = 0;

            GenerateUnit();
        }
    }

    // エネミー生成
    void GenerateUnit()
    {
        var rnd_Idx = Random.Range(3, unitData.stats.Count - 1);
        var rndXpos = Random.Range(-1.7f, 1.7f);

        // 対応するインデックスのユニットオブジェクトを生成
        GameObject unit = null;
        enemyUnitGeneratePos.x = rndXpos;
        unit = Instantiate(unitObj, enemyUnitGeneratePos, Quaternion.identity);

        UnitController uc = unit.GetComponent<UnitController>();
        uc.SetUnitStats(unitData.stats[rnd_Idx], UnitGroup.Enemy);

        // ユニットを管理するUnitManagerに生成したユニットの陣営に対応するリストに格納
        UnitManager um = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        um.AddUnitList(unit, UnitGroup.Enemy);
    }
}
