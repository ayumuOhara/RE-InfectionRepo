using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAppearsSpace : MonoBehaviour
{
    public StageDataManager stageDataManager;
    public ScrollChecker scrollChecker;

    public Image EnemyImage;

    private Image[] spawnedImages;
    private int lastStagePage = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateEnemyImages(scrollChecker.GetStagePage());
        lastStagePage = scrollChecker.GetStagePage();

    }

    // Update is called once per frame
    void Update()
    {
        int currentStagePage = scrollChecker.GetStagePage();

        //ページが変わったら敵画像を更新
        if (currentStagePage != lastStagePage)
        {
            //クリアしているステージか、ステージ1は表示
            if (stageDataManager.stage[currentStagePage].isClear == true || currentStagePage == 0)
            {
                UpdateEnemyImages(currentStagePage);
            }else
            {
                //クリアしていないステージなら画像を消す
                if (spawnedImages != null)
                {
                    foreach (var img in spawnedImages)
                    {
                        if (img != null)
                        {
                            Destroy(img.gameObject);
                        }
                    }
                }
            }
            lastStagePage = currentStagePage;
        }
    }

    //敵画像の更新
    private void UpdateEnemyImages(int stagePage)
    {
        //生成先を取得
        Transform content = transform.Find("Viewport/Content");

        //既存の画像を削除
        if (spawnedImages != null)
        {
            foreach (var img in spawnedImages)
            {
                if (img != null)
                {
                    Destroy(img.gameObject);
                }
            }
        }

        var units = stageDataManager.stage[stagePage].SpawnUnits;
        spawnedImages = new Image[units.Count];

        for (int i = 0; i < units.Count; i++)
        {   
            var unit = units[i];

            GameObject go = Instantiate(EnemyImage.gameObject, content, false);
            var img = go.GetComponent<Image>();

            img.sprite = unit.unitSprite;
            img.gameObject.SetActive(true);

            spawnedImages[i] = img;
        }

    }

}
