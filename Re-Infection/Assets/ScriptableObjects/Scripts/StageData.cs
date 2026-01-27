using UnityEngine;


[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
public class StageData : ScriptableObject
{
    public Stage[] Stage;
    public bool[] isStageClear;//クリア状況の記録
    public bool[] isStageOpen;//解放済みかのフラグ

    public int SelectStageNumber;
}
