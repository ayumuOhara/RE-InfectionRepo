using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleCanvas : MonoBehaviour
{
    public StageDataManager stageDataManager;
    public ScrollChecker scrollChecker;
    public StageData stageData;
    public EnemyAppearsSpace enemyAppearsSpace;

    public Image[] stageImage; //ステートの画像スプライト配列
    public Animator[] lockAnime; //ステージのロック中の表示にスプライト
    public GameObject messageBox; //ステージ解放時のメッセージボックス
    public GameObject messageCanvas;

    public TextMeshProUGUI conditionsText; //ステージの解放条件を表示するテキスト
    public TextMeshProUGUI releaseText; //ステージ解放時のテキスト 

    public Button rightButton; //右矢印ボタン
    public Button leftButton; //左矢印ボタン
    public Button sortieButton; //出撃ボタン

    int stageNumber = 0; //現在表示しているステージ番号
    int stageLastNumber = 0; //ステージの最後の番号

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        messageCanvas.SetActive(false);
        messageBox.SetActive(false);

        //最初は全てのステージを暗くする
        for (int i = 0; i < stageImage.Length; i++)
        {
            stageImage[i].color = new Color(0.1f, 0.1f, 0.1f, 0.9803922f);
        }


        //最初にステージ1を表示
        scrollChecker.scrollSnap.GoToPanel(0);

    }

    // Update is called once per frame
    void Update()
    {
        stageNumber = scrollChecker.GetStagePage(); //現在のステージ番号を取得
        stageLastNumber = scrollChecker.GetStageLastPage(); //ステージの最後の番号を取得

        //ページの端に来たら矢印ボタンを押せなくする
        //左端のとき
        if (stageNumber == 0)
        {
            leftButton.interactable = false;
        }
        else
        {
            leftButton.interactable = true;
        }

        //右端のとき
        if (stageNumber == stageLastNumber - 1)
        {
            rightButton.interactable = false;
        }
        else
        {
            rightButton.interactable = true;
        }

        //ステージ1のときは必ず出撃可能
        if (stageNumber == 0)
        {
            lockAnime[stageNumber].gameObject.SetActive(false);
            stageImage[stageNumber].color = new Color(1f, 1f, 1f, 1f);
            conditionsText.gameObject.SetActive(false);
            sortieButton.interactable = true;
        }
        //前のステージがクリア済みか
        if (stageNumber != 0 && stageDataManager.stage[stageNumber - 1].isClear == false)
        {
            conditionsText.text = $"ステージ{stageNumber}クリアで解放";
            if(stageNumber == 1)
            {
                conditionsText.text = $"チュートリアルステージ\nをクリアで解放";
            }
            conditionsText.gameObject.SetActive(true);
            sortieButton.interactable = false;
        }
        else
        {
            conditionsText.gameObject.SetActive(false);
            sortieButton.interactable = true;
        }
    }


    //出撃ボタンを押したときの処理
    public void OnSortie()
    {

        stageData.SelectStageNumber = stageNumber;
        SceneManager.LoadScene("MainScene");
    }

    //クリア済みステージの処理
    public void OnClearedStage(int stage)
    {
        if (stage >= 4) return;

        lockAnime[stage].gameObject.SetActive(false);
        stageImage[stage].color = new Color(1f, 1f, 1f, 1f);
        conditionsText.gameObject.SetActive(false);
        sortieButton.interactable = true;
    }

    //クリア後に解放されたステージに移る処理
    public void OnChangeStage(int stage)
    {
        if (stage >= 4) return;
        messageCanvas.SetActive(true);


        StartCoroutine(PlayAnimetion(stage));
    }

    //鍵が外れるアニメーション
    private IEnumerator PlayAnimetion(int openStage) 
    {
        scrollChecker.scrollSnap.GoToPanel(openStage);
        

        yield return new WaitForSeconds(0.7f);
        yield return new WaitForSeconds(0.7f);
        lockAnime[openStage].SetBool("IsOpen", true);

        yield return new WaitForSeconds(0.7f);
        lockAnime[stageNumber].gameObject.SetActive(false);
        stageImage[openStage].color = new Color(1f, 1f, 1f, 1f);

        yield return new WaitForSeconds(0.5f);
        Debug.Log($"{openStage + 1}ステージ解放");
        messageBox.SetActive(true);
        releaseText.text = $"ステージ{openStage + 1}が解放された!";
        Time.timeScale = 0f; 

    }

    //OKボタン
    public void OnOkButton()
    {
        messageBox.SetActive(false);

        messageCanvas.SetActive(false);
        Time.timeScale = 1f; 
    }
}