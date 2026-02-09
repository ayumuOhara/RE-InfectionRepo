using UnityEngine;

public class OptionManager : MonoBehaviour
{
    [SerializeField] private GameObject volumePanel;

    void Start()
    {
        volumePanel.SetActive(false); // 最初は非表示
    }

    // ボタンから呼ぶ（音量設定を開く）
    public void ShowVolumePanel()
    {
        volumePanel.SetActive(true);
        AudioManager.Instance.PlaySE(SEManager.SEType.Button_Click);
    }

    // 閉じるボタンから呼ぶ
    public void HideVolumePanel()
    {
        volumePanel.SetActive(false);
        AudioManager.Instance.PlaySE(SEManager.SEType.Button_Click);
    }
}
