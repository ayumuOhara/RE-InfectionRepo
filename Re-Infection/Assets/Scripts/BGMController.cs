using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMController : MonoBehaviour
{
    [SerializeField] private BGMManager bgmManager;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Œ»İ‚ÌƒV[ƒ“‚ğæ“¾‚µ‚ÄABGM‚ğÄ¶‚·‚é
        Scene currentScene = SceneManager.GetActiveScene();
        OnSceneLoaded(currentScene, LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bgmManager.StopBGM();

        switch (scene.name)
        {
            case "TitleScene":
                bgmManager.PlayBGM(BGMManager.BGMType.BGM_1);
                break;
            case "Home":
                bgmManager.PlayBGM(BGMManager.BGMType.BGM_2);
                break;
            case "MainScene":
                bgmManager.PlayBGM(BGMManager.BGMType.BGM_3);
                break;
        }
    }
}
