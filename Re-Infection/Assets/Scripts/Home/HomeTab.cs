using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HomeTab : MonoBehaviour
{
    private int canvasPage;//表示しているページ番号

    public Button shopButton; // ショップボタン
    public Button battleButton; // ステージ選択ボタン
    public Button selectionButton; // 編成ボタン

    public GameObject shopCanvas; // ショップ画面
    public GameObject battleCanvas; //ステージ選択画面
    public GameObject selectionCanvas;//編成画面

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        shopButton.interactable = true;
        battleButton.interactable = false;
        selectionButton.interactable = true;

        battleCanvas.SetActive(true);
        shopCanvas.SetActive(false);
        selectionCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        //switch (canvasPage)
        //{
        //    case 0:
        //        Debug.Log("ショップ画面表示");
        //        //shopButton.interactable = false;
        //        battleButton.interactable = true;
        //        selectionButton.interactable = true;
        //        break;
        //    case 1:
        //        Debug.Log("ステージ選択画面表示");
        //        //shopButton.interactable = true;
        //        battleButton.interactable = false;
        //        selectionButton.interactable = true;
        //        break;
        //    case 2:
        //        Debug.Log("編成画面表示");
        //        //shopButton.interactable = true;
        //        battleButton.interactable = true;
        //        selectionButton.interactable = false;
        //        break;
        //    default:
        //        Debug.LogError("存在しないページです");
        //        break;
        //}
    }

    //ショップ画面を表示
    public void OnShop()
    {
        shopButton.interactable = false;
        battleButton.interactable = true;
        selectionButton.interactable = true;

        shopCanvas.SetActive(true);
        battleCanvas.SetActive(false);
        selectionCanvas.SetActive(false);
    }

    //ステージ選択画面を表示
    public void OnBattle()
    {
        shopButton.interactable = true;
        battleButton.interactable = false;
        selectionButton.interactable = true;

        shopCanvas.SetActive(false);
        battleCanvas.SetActive(true);
        selectionCanvas.SetActive(false);
    }

    //編成画面を表示
    public void OnSelection()
    {
        shopButton.interactable = true;
        battleButton.interactable = true;
        selectionButton.interactable = false;

        shopCanvas.SetActive(false);
        battleCanvas.SetActive(false);
        selectionCanvas.SetActive(true);
    }

}
