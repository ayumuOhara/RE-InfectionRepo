using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Canvas transitionUIprefab;

    // シーンロード
    public void OnLoadScene(string name)
    {
        SceneTransitionner transitonner = Instantiate(transitionUIprefab).GetComponent<SceneTransitionner>();
        transitonner.OnLoadScene(name);
    }
}
