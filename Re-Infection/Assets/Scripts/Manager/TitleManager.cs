using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Canvas transitionUIprefab;
    [SerializeField] Button startButton;

    // シーンロード
    public void OnLoadScene(string name)
    {
        startButton.enabled = false;

        SceneTransitionner transitonner = Instantiate(transitionUIprefab).GetComponent<SceneTransitionner>();
        transitonner.OnLoadScene(name);
    }
}
