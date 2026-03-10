using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class StageDataManager : MonoBehaviour
{
    public StageData stageData;

    [SerializeField] public Stage[] stage;
    public BattleCanvas battleCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Home") return;
        SceneStart();
    }

    void SceneStart()
    {
        for (int i = 0; i < stage.Length; i++)
        {
            if (stageData.Stage[i] == null || !stageData.Stage[i].isOpened) continue;

            battleCanvas.OnClearedStage(i);
        }

        //ステージのクリア情報を確認
        for (int i = 1; i < stage.Length; i++)
        {
            if (stageData.Stage[i] == null || stageData.Stage[i].isOpened) continue;

            if (stageData.Stage[Mathf.Max(i - 1, 0)].isClear && i == stageData.GetStageProgress)
            {
                battleCanvas.OnChangeStage(i);
                stageData.Stage[i].SetIsOpend(true);
            }
        }
    }
}