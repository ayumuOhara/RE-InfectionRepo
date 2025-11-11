using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;

public class ScrollChecker : MonoBehaviour
{
    public SimpleScrollSnap scrollSnap;

    [SerializeField] public int stagePage;//表示しているページ番号

    // Update is called once per frame
    void Update()
    {
        stagePage = scrollSnap.SelectedPanel;//今表示しているパネル番号を取得
        Debug.Log($"ステージ{stagePage + 1}");

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


    //現在のステージページ番号を取得する関数
    public int GetStagePage()
    {
        return stagePage;
    }
}
