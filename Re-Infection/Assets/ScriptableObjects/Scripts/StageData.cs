using UnityEngine;


[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
public class StageData : ScriptableObject
{
    public Stage[] Stage;

    private int stageProgress = 0;
    public int GetStageProgress => PlayerPrefs.GetInt("Progress", 0);
    public void SetStageProgress(int stageNum)
    {
        if (stageProgress >= stageNum) return;

        stageProgress = stageNum;

        PlayerPrefs.SetInt("Progress", stageProgress);
    }

    public int SelectStageNumber;
}
