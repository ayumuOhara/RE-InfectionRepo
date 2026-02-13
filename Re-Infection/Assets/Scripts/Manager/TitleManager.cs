using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Canvas transitionUIprefab;

    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    // シーンロード
    public void OnLoadScene(string name)
    {
        SEManager.Instance.PlaySE(SEManager.SEType.Lord);
        SceneTransitionner transitonner = Instantiate(transitionUIprefab).GetComponent<SceneTransitionner>();
        transitonner.OnLoadScene(name);
    }
}
