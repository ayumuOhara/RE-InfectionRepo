using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;

public class ScrollChecker : MonoBehaviour
{
    public SimpleScrollSnap scrollSnap;

    [SerializeField] public int stageCount;//ステージの総数
    [SerializeField] public int stagePage;//表示しているページ番号

    private void Start()
    {
        stageCount = scrollSnap.NumberOfPanels;//ステージの総数を取得
        Debug.Log($"ステージ数{stageCount}");
    }
    // Update is called once per frame
    void Update()
    {
        stagePage = scrollSnap.SelectedPanel;//今表示しているパネル番号を取得
        Debug.Log($"パネル番号{stagePage} ステージ{stagePage + 1}");

    }

    //左右のボタンを押したときの処理
    public void OnLeftStage()
    {
        scrollSnap.GoToPreviousPanel();
    }

    public void OnRightStage()
    {
        scrollSnap.GoToNextPanel();
    }


    //ステージの要素数を取得する関数
    public int GetStageCount()
    {
        return stageCount;
    }

    //現在のステージページ番号を取得する関数
    public int GetStagePage()
    {
        return stagePage;
    }

    //ステージの最後のページ番号を取得する関数
    public int GetStageLastPage()
    {
        return stageCount;
    }
}
