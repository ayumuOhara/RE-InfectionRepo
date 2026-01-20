using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StageDataManager : MonoBehaviour
{
    public static StageDataManager Instance;

    [SerializeField] public Stage[] stage;
    public BattleCanvas battleCanvas;

    public bool[] saveisClear;//ステージのクリア情報保存用配列

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Home") return;
        StartCoroutine(SceneStart());
    }

    IEnumerator SceneStart()
    { 

        for (int i = 0; i < stage.Length; i++)
        {
            saveisClear[i] = stage[i].isClear;
        }

        yield return new WaitForSeconds(4.5f);

        //ステージのクリア情報を確認
        for (int i = 0; i < stage.Length; i++)
        {
            if (saveisClear[i] == true)
            {
                battleCanvas.OnChangeStage(i + 1);

            }
        }
    }
}
