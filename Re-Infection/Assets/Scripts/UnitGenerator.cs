using Unity.VisualScripting;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    [SerializeField] UnitData unitData;

    [SerializeField] UnitManager unitManager;

    float generateInterbal; // 生成後の経過時間
    float generateTime = 2.0f;     // 生成する間隔

    UnitGroup generateGroup = UnitGroup.Enemy;  // 生成するユニットの陣営

    Vector3 playerUnitGenaretePos = new Vector3(0, -3, 0);  // プレイヤーユニットの生成座標
    Vector3 enemyUnitGeneratePos = new Vector3(0, 6, 0);    // エネミーユニットの生成座標

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generateInterbal = 0;

        for(int i = 0; i < 2; i++)
            GenerateUnit();
    }

    // Update is called once per frame
    void Update()
    {
        //generateInterbal += Time.deltaTime;

        //if(generateInterbal > generateTime)
        //{
        //    generateInterbal = 0;

        //    GenerateUnit();
        //}
    }

    // エネミー生成
    void GenerateUnit()
    {
        var rnd_Idx = Random.Range(0, unitData.stats.Count);    // ランダムなユニットのインデックスを生成

        // 対応するインデックスのユニットオブジェクトを生成
        GameObject unit = null;
        if (generateGroup == UnitGroup.Player)
            unit = Instantiate(unitData.stats[rnd_Idx].unitObj, playerUnitGenaretePos, Quaternion.identity);
        else
            unit = Instantiate(unitData.stats[rnd_Idx].unitObj, enemyUnitGeneratePos, Quaternion.identity);

        // 生成したオブジェクトにUnitControllerコンポーネントを付与&代入
        UnitController uc = unit.AddComponent<UnitController>();
        // 生成したユニットに生成したインデックスのユニットのスタッツを代入
        uc.SetUnitStats(unitData.stats[rnd_Idx], generateGroup);

        // ユニットを管理するUnitManagerに生成したユニットの陣営に対応するリストに格納
        unitManager.AddUnitList(unit, generateGroup);

        // 次回生成するユニットの陣営を切り替え
        generateGroup = (generateGroup == UnitGroup.Enemy ? UnitGroup.Player : UnitGroup.Enemy);
    }
}
