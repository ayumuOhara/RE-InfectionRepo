using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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


    private void Update()
    {
        //TODO 後で消すデバック用 
        if (Input.GetKey(KeyCode.X))
        {
            for (int i = 0; i < stage.Length; i++)
            {
                stageData.isStageOpen[i] = false;
            }
        }

        if (Input.GetKey(KeyCode.C))
        {
            //ステージのクリア情報を確認
            for (int i = 0; i < stage.Length; i++)
            {
                //解放されたら
                if (stageData.isStageClear[i] == true && stageData.isStageOpen[i] == false)
                {
                    battleCanvas.OnChangeStage(i + 1);
                    stageData.isStageOpen[i] = true;
                }
            }
        }
    }

    IEnumerator SceneStart()
    {
        for (int i = 0; i < stage.Length; i++)
        {
            stageData.isStageClear[i] = stage[i].isClear;
            
            //解放済みステージの処理
            if (stageData.isStageClear[i] == true && stageData.isStageOpen[i] == true)
            {
                battleCanvas.OnClearedStage(i + 1);
            }
        }

        yield return new WaitForSeconds(2f);

        //ステージのクリア情報を確認
        for (int i = 0; i < stage.Length; i++)
        {
            //解放されたら
            if (stageData.isStageClear[i] == true && stageData.isStageOpen[i] == false)
            {
                battleCanvas.OnChangeStage(i + 1);
                stageData.isStageOpen[i] = true;
            }
        }
    }
}