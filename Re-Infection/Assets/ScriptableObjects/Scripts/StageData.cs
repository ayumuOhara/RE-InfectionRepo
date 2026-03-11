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

        stageProgress = Mathf.Clamp(stageNum, 0, Stage.Length - 1);

        PlayerPrefs.SetInt("Progress", stageProgress);
        PlayerPrefs.Save();
    }

    public int SelectStageNumber;
}
