using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UnitPaidDialog:MonoBehaviour
{
    public GameObject UnitPaidDialogObj;

    public TextMeshProUGUI dialog_text;

    public System.Action onClickYes;

    private void Start()
    {
        UnitPaidDialogObj.SetActive(false);
    }

    public void Dialog()
    {
        UnitPaidDialogObj.SetActive(true);
    }

    public void Onclick_yesButton()
    {
        onClickYes?.Invoke();
       
    }
    public void Onclick_noButton()
    {
        UnitPaidDialogObj.SetActive(false);
    }
    
    public void SetDialogMessage(string message)
    {
        if (dialog_text != null)
        {
            dialog_text.text = message;
        }
    }
}
