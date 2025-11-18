using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Canvas transitionUIprefab;
    [SerializeField] AudioClip startSe;

    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    // シーンロード
    public void OnLoadScene(string name)
    {
        AudioSource source = GetComponent<AudioSource>();
        source.PlayOneShot(startSe);

        SceneTransitionner transitonner = Instantiate(transitionUIprefab).GetComponent<SceneTransitionner>();
        transitonner.OnLoadScene(name);
    }
}
