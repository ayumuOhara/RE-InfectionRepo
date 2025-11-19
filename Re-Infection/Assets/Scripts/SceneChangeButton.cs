using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButton:MonoBehaviour
{
    //ï“ê¨
    public void InputSelectionButton()
    {
        SceneManager.LoadScene("SelectionScene");
    }

    //êÌì¨
    public void InputActionButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    //ÉVÉáÉbÉv
    public void InputShopButton()
    {
        SceneManager.LoadScene("");
    }
}
