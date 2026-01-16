using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleCanvas : MonoBehaviour
{
    public StageDataManager stageDataManager;
    public ScrollChecker scrollChecker;

    public Image[] stageImage; //ステートの画像スプライト配列
    public GameObject[] lookImage; //ステージのロック中の表示にスプライト

    public TextMeshProUGUI conditionsText; //ステージの解放条件を表示するテキスト

    public Button rightButton; //右矢印ボタン
    public Button leftButton; //左矢印ボタン
    public Button sortieButton; //出撃ボタン

    int stageNumber = 0; //現在表示しているステージ番号
    int stageLastNumber = 0; //ステージの最後の番号

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            lookImage[stageNumber].SetActive(false);
            stageImage[stageNumber].color = new Color(1f, 1f, 1f, 1f);
            conditionsText.gameObject.SetActive(false);
            sortieButton.interactable = true;
            return;
        }
        //前のステージがクリア済みか
        if (stageDataManager.stage[stageNumber - 1].isClear == false)
        {
            lookImage[stageNumber].SetActive(true);
            conditionsText.text = $"ステージ{stageNumber}クリアで解放";
            conditionsText.gameObject.SetActive(true);
            sortieButton.interactable = false;
        }
        else if (stageDataManager.stage[stageNumber - 1].isClear == true)//クリア済みなら
        {
            lookImage[stageNumber].SetActive(false);
            stageImage[stageNumber].color = new Color(1f, 1f, 1f, 1f);
            conditionsText.gameObject.SetActive(false);
            sortieButton.interactable = true;

        }
        else
        {
            Debug.LogError("ステージクリアフラグ配列の要素数が足りません");
        }

    }


    //出撃ボタンを押したときの処理
    public void OnSortie(int Stage)
    {
        SceneManager.LoadScene("BetaScene");
    }
}
