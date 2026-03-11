using System.Collections;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleCanvas : MonoBehaviour
{
    [SerializeField] private Canvas transitionUIprefab;

    public StageDataManager stageDataManager;
    public ScrollChecker scrollChecker;
    public StageData stageData;

    public Image[] stageSilhouette; //ステートの画像スプライト配列
    public Animator[] lockAnime; //ステージのロック中の表示にスプライト
    public GameObject messageBox; //ステージ解放時のメッセージボックス
    public GameObject messageCanvas;
    public GameObject responsePanel; //ステージクリアの判別の処理が終わるまで表示するパネル
    public GameObject clearedUIElements;
    public TextMeshProUGUI bestClearTimeText;

    public TextMeshProUGUI stageTitleText; //ステージのタイトルを表示するテキスト
    public TextMeshProUGUI conditionsText; //ステージの解放条件を表示するテキスト
    public TextMeshProUGUI releaseText; //ステージ解放時のテキスト 

    public Button rightButton; //右矢印ボタン
    public Button leftButton; //左矢印ボタン
    public Button sortieButton; //出撃ボタン

    private bool isInitialization = false; //最初の一回だけ実行するためのフラグ
    int stageNumber = 0; //現在表示しているステージ番号
    int stageLastNumber = 0; //ステージの最後の番号

    private void Awake()
    {
        Time.timeScale = 1.0f;
        responsePanel.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        messageCanvas.SetActive(false);
        messageBox.SetActive(false);

        foreach (var image in stageSilhouette)
            image.color = Color.black;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialization)
        {
            scrollChecker.scrollSnap.GoToPanel(stageData.SelectStageNumber);
            isInitialization = true;
        }

        stageNumber = scrollChecker.GetStagePage(); //現在のステージ番号を取得
        stageLastNumber = scrollChecker.GetStageLastPage(); //ステージの最後の番号を取得

        //ステージのテキスト判別
        if (stageNumber == 0)
        {
            stageTitleText.text = "チュートリアル";
        }
        else
        {
            stageTitleText.text = $"ステージ{stageNumber}";
        }

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
            stageSilhouette[stageNumber].color = new Color(1f, 1f, 1f, 0f);
            conditionsText.gameObject.SetActive(false);
            sortieButton.interactable = true;
        }
        //前のステージがクリア済みか
        if (stageNumber != 0 && stageDataManager.stage[stageNumber - 1].isClear == false)
        {
            conditionsText.text = $"ステージ{stageNumber - 1}クリアで解放";
            if (stageNumber == 1)
            {
                conditionsText.text = $"チュートリアル\nをクリアで解放";
            }
            conditionsText.gameObject.SetActive(true);
            sortieButton.interactable = false;
        }
        else
        {
            conditionsText.gameObject.SetActive(false);
            sortieButton.interactable = true;
        }

        SetBestClearTimeText();
    }

    private void SetBestClearTimeText()
    {
        if (stageData.Stage[scrollChecker.stagePage].isClear)
        {
            clearedUIElements.GetComponent<CanvasGroup>().alpha = 1f;
            Debug.Log(stageData.Stage[scrollChecker.stagePage].GetStageClearTimeText());
            bestClearTimeText.text = "<size=40>最速クリアタイム</size>\n" + stageData.Stage[scrollChecker.stagePage].GetStageClearTimeText();

        }
        else
        {
            clearedUIElements.GetComponent<CanvasGroup>().alpha = 0;
        }
    }


    //出撃ボタンを押したときの処理
    public void OnSortie()
    {
        stageData.SelectStageNumber = stageNumber;
        SEManager.Instance.PlaySE(SEManager.SEType.Lord);
        SceneTransitionner transitonner = Instantiate(transitionUIprefab).GetComponent<SceneTransitionner>();
        transitonner.OnLoadScene("MainScene");
    }

    //クリア済みステージの処理
    public void OnClearedStage(int stage)
    {
        if (stage >= stageData.Stage.Length) return;

        lockAnime[stage].gameObject.SetActive(false);
        stageSilhouette[stage].color = new Color(1f, 1f, 1f, 0f);
        conditionsText.gameObject.SetActive(false);
        sortieButton.interactable = true;
    }

    //クリア後に解放されたステージに移る処理
    public void OnChangeStage(int stage)
    {
        if (stage >= stageData.Stage.Length) return;
        messageCanvas.SetActive(true);


        StartCoroutine(PlayAnimetion(stage));
    }

    //鍵が外れるアニメーション
    private IEnumerator PlayAnimetion(int openStage)
    {
        scrollChecker.scrollSnap.StartingPanel = openStage;

        responsePanel.SetActive(true);

        isInitialization = true;

        scrollChecker.scrollSnap.GoToPanel(openStage);


        yield return new WaitForSeconds(0.7f);
        yield return new WaitForSeconds(0.7f);
        lockAnime[openStage].SetBool("IsOpen", true);

        yield return new WaitForSeconds(0.7f);
        lockAnime[stageNumber].gameObject.SetActive(false);
        stageSilhouette[openStage].color = new Color(1f, 1f, 1f, 0f);

        yield return new WaitForSeconds(0.5f);
        Debug.Log($"{openStage + 1}ステージ解放");
        messageBox.SetActive(true);
        releaseText.text = $"ステージ{openStage}が解放された!";

        responsePanel.SetActive(false);
    }

    //OKボタン
    public void OnOkButton()
    {
        messageBox.SetActive(false);

        messageCanvas.SetActive(false);
    }
}