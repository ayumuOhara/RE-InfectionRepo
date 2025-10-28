using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Slider loadingBar;
    [SerializeField] TextMeshProUGUI loadingProgressText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // シーンロード
    public void OnLoadScene(string name)
    {
        startButton.gameObject.SetActive(false);
        loadingBar.gameObject.SetActive(true);
        StartCoroutine(LoadAsyncScene(name));
    }

    // ローディング処理
    IEnumerator LoadAsyncScene(string name)
    {
        AsyncOperation ope = SceneManager.LoadSceneAsync(name);

        while (!ope.isDone)
        {
            float progress = Mathf.Clamp01(ope.progress / 0.9f);
            loadingBar.value = progress;
            loadingProgressText.text = progress * 100f + "%";

            yield return null;
        }
    }
}
