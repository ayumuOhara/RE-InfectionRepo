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

    public Canvas shopCanvas; // ショップ画面
    public Canvas battleCanvas; //ステージ選択画面
    public Canvas selectionCanvas;//編成画面

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        shopButton.interactable = true;
        battleButton.interactable = false;
        selectionButton.interactable = true;

        battleCanvas.enabled = true;
        shopCanvas.enabled = false;
        selectionCanvas.enabled = false;
    }

    //ショップ画面を表示
    public void OnShop()
    {
        shopButton.interactable = false;
        battleButton.interactable = true;
        selectionButton.interactable = true;

        shopCanvas.enabled = true;
        battleCanvas.enabled = false;
        selectionCanvas.enabled = false;
    }

    //ステージ選択画面を表示
    public void OnBattle()
    {
        shopButton.interactable = true;
        battleButton.interactable = false;
        selectionButton.interactable = true;

        shopCanvas.enabled = false;
        battleCanvas.enabled = true;
        selectionCanvas.enabled = false;
    }

    //編成画面を表示
    public void OnSelection()
    {
        shopButton.interactable = true;
        battleButton.interactable = true;
        selectionButton.interactable = false;

        shopCanvas.enabled = false;
        battleCanvas.enabled = false;
        selectionCanvas.enabled = true;
    }

}
