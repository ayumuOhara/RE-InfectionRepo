using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleCanvas : MonoBehaviour
{
    public Image stageImage; //ステージの画像表示オブジェクト
    public Sprite[] stageSprite; //ステートの画像スプライト配列
    public GameObject lookImage; //ステージのロック中の表示にスプライト


    public TextMeshProUGUI conditionsText; //ステージの解放条件を表示するテキスト
    public Button sortieButton; //出撃ボタン

    public bool[] stageClearFlag; //ステージクリアフラグ配列

    int stageNum = 0; //現在表示しているステージ番号

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //最初にステージ1を表示
        stageImage.sprite = stageSprite[0];
        stageClearFlag[0] = false; //ステージ1は最初から解放済み
    }

    // Update is called once per frame
    void Update()
    {
        //そのステージがクリア済みか
        if (stageClearFlag[stageNum] == false)
        {
            lookImage.SetActive(true);
            //暗くする
            stageImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9803922f);
            conditionsText.text = $"ステージ{stageNum - 1}クリアで解放";
        }
        else if (stageClearFlag[stageNum] == true)//クリア済みなら
        {
            lookImage.SetActive(false);
            stageImage.color = new Color(1f, 1f, 1f, 1f);
            conditionsText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("ステージクリアフラグ配列の要素数が足りません");
        }
    }

    
    public void OnLeftStage()
    {

    }

    public void OnRightStage()
    {
    }

    //出撃ボタンを押したときの処理
    public void OnSortie(int Stage)
    {
        SceneManager.LoadScene("MainScene");
    }
}
