using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopButtonHover : MonoBehaviour
{
    public GameObject targetObject;

    [Header("ボタン")]
    public Button UpGradeButton;
    public Button UnitUpGradeButton;

    [Header("表示/非表示切替OBJ")]
    public GameObject UpGradeObj;
    public GameObject UnitUpGradeObj;

    //[Header("アニメーション")]
    //public Animator anim;
    private void Start()
    {
        targetObject.SetActive(false);

        UpGradeButton.interactable = false;
        UnitUpGradeButton.interactable = true;

        UpGradeObj.SetActive(true);
        UnitUpGradeObj.SetActive(false);
    }
 
    public void InputShopButton()
    {
        targetObject.SetActive(true);
    }

    public void UpGrade()
    {
        UpGradeButton.interactable = false;
        UnitUpGradeButton.interactable = true;

        UpGradeObj.SetActive(true);
        UnitUpGradeObj.SetActive(false);
    }

    public void UnitUpGrade()
    {
        UpGradeButton.interactable =true;
        UnitUpGradeButton.interactable = false;

        UpGradeObj.SetActive(false);
        UnitUpGradeObj.SetActive(true);
    }
}