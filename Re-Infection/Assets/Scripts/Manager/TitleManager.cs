using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Canvas transitionUIprefab;
    [SerializeField] Canvas dataResetUI;
    [SerializeField] Toggle deleteToggle;
    [SerializeField] Button deleteButton;

    private void Awake()
    {
        Application.targetFrameRate = 120;
        
        dataResetUI.enabled = false;
        deleteToggle.isOn = false;
        deleteButton.interactable = false;
    }

    // シーンロード
    public void OnLoadScene(string name)
    {
        SEManager.Instance.PlaySE(SEManager.SEType.Lord);
        SceneTransitionner transitonner = Instantiate(transitionUIprefab).GetComponent<SceneTransitionner>();
        transitonner.OnLoadScene(name);
    }

    public void OnToggleChanged()
    {
        deleteButton.interactable = !deleteButton.interactable;
    }

    public void OnOpenDataResetUI()
    {
        dataResetUI.enabled = !dataResetUI.enabled;

        deleteButton.interactable = false;
        deleteToggle.isOn = false;
    }

    public void OnDataReset()
    {
        PlayerPrefs.DeleteAll();
    }
}
