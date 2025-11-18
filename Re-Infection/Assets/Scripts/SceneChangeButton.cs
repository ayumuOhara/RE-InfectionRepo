using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButton:MonoBehaviour
{
    public GameObject SceneInChange;
    public GameObject Loading_text;
    //ï“ê¨
    private void Start()
    {
        SceneInChange.SetActive(false);
        Loading_text.SetActive(false);
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(SceneInChange);
        DontDestroyOnLoad(Loading_text);
    }
    public void InputActionButton()
    {
        StartCoroutine(InputActionScene());
    }
    public void InputSceneButton()
    {
        
        SceneManager.LoadScene("SelectionScene");
    }

    //êÌì¨
    public IEnumerator InputActionScene()
    {
        SceneInChange.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        Loading_text.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("MainScene");
    }

    //ÉVÉáÉbÉv
    public void InputShopButton()
    {
        SceneManager.LoadScene("");
    }
}
