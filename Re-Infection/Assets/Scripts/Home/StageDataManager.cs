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
        StartCoroutine(SceneStart());
    }

    IEnumerator SceneStart()
    {
        StartCoroutine(StartPanel());

        for (int i = 0; i < stage.Length; i++)
        {
            //解放済みステージの処理
            if (stageData.Stage[i].isClear && i <= stageData.GetStageProgress)
            {
                battleCanvas.OnClearedStage(i + 1);
            }
        }

        yield return new WaitForSeconds(1f);

        //ステージのクリア情報を確認
        for (int i = 0; i < stage.Length; i++)
        {
            //解放されたら
            if (stageData.Stage[i].isClear && i > stageData.GetStageProgress)
            {
                battleCanvas.OnChangeStage(i + 1);
            }
        }
    }

    //ステージクリアの処理が終わるまで表示する
    private IEnumerator StartPanel()
    {
        battleCanvas.responsePanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        battleCanvas.responsePanel.SetActive(false);
    }
}