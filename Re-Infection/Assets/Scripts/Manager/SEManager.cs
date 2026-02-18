using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SEManager : MonoBehaviour
{
    [SerializeField] private AudioSource seAudioSource;
    [SerializeField] private List<AudioClip> seClips;
    [SerializeField] private Slider seSlider;
    [SerializeField] private SESetting seSetting;

    public static SEManager Instance;

    private void Start()
    {
        if (seSlider != null)
        {
            seSlider.value = seSetting.volume;
            seSlider.onValueChanged.AddListener(OnVolumeChenged);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン跨ぎたいなら
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        seAudioSource.volume = seSetting.volume;
    }

    void OnVolumeChenged(float value)
    {
        seSetting.volume = value;
    }

    // EnumによるSE管理
    public enum SEType
    {
        Button_Click,  // ボタンをクリックしたときの音
        Lord,          // シーンロード
        StageClear,    // ステージクリア
        StageFailed,   // ステージ失敗
        Summon,        // ユニット召喚
        SummonFailed,  // ユニット召喚失敗
        Damage,        // ユニットがダメージを受けた時
        Explosion,     // 爆弾
        CanExplosion,  // 爆弾が使用可能
        BossDefeat,    // ボス撃破
        Upgrade,       // 強化
    }

    // SE再生メソッド
    public void PlaySE(SEType seType)
    {
        int index = (int)seType;  // Enumからインデックスへ変換
        PlaySEFromList(index);
    }

    // リストからSEを再生
    private void PlaySEFromList(int index)
    {
        if (index >= 0 && index < seClips.Count)
        {
            seAudioSource.PlayOneShot(seClips[index]);
        }
        else
        {
            // Debug.LogWarning("指定されたインデックスに該当するSEがありません");
        }
    }
}
