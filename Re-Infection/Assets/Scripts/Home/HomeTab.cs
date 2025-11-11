using UnityEngine;
using UnityEngine.UI;

public class HomeTab : MonoBehaviour
{
    public Button shopButton; // ショップボタン
    public Button battleButton; // ステージ選択ボタン
    public Button organizationButton; // 編成ボタン

    public GameObject shopCanvas; // ショップ画面
    public GameObject battleCanvas; //ステージ選択画面
    public GameObject organizationCanvas;//編成画面

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shopButton.interactable = true;
        battleButton.interactable = false;
        organizationButton.interactable = true;
        shopCanvas.SetActive(false);
        battleCanvas.SetActive(true);
        organizationCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ショップ画面を表示
    public void OnShop()
    {
        shopButton.interactable = false;
        battleButton.interactable = true;
        organizationButton.interactable = true;
        shopCanvas.SetActive(true);
        battleCanvas.SetActive(false);
        organizationCanvas.SetActive(false);
    }

    //ステージ選択画面を表示
    public void OnBattle()
    {
        shopButton.interactable = true;
        battleButton.interactable = false;
        organizationButton.interactable = true;
        shopCanvas.SetActive(false);
        battleCanvas.SetActive(true);
        organizationCanvas.SetActive(false);
    }

    //編成画面を表示
    public void OnOrganization()
    {
        shopButton.interactable = true;
        battleButton.interactable = true;
        organizationButton.interactable = false;
        shopCanvas.SetActive(false);
        battleCanvas.SetActive(false);
        organizationCanvas.SetActive(true);
    }
}
