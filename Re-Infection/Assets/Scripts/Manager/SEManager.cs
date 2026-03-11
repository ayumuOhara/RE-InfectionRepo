using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SEManager : MonoBehaviour
{
    [SerializeField] private AudioSource seAudioSource;
    [SerializeField] private List<AudioClip> seClips;
    [SerializeField] private Slider seSlider;
    [SerializeField] private SESetting seSetting;

    // 同一SEの連続再生を制限する時間（秒）
    // 0.05秒〜0.1秒程度に設定すると、自然に聞こえつつ負荷を抑えられます
    [SerializeField] private float minPlayInterval = 0.05f;

    // 各SEタイプごとの最終再生時間を記録する辞書
    private Dictionary<SEType, float> lastPlayTimes = new Dictionary<SEType, float>();

    public static SEManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 辞書の初期化
            foreach (SEType type in System.Enum.GetValues(typeof(SEType)))
            {
                lastPlayTimes[type] = -10f; // 最初は即座に鳴るように負の値を設定
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (seSlider != null)
        {
            seSlider.value = seSetting.volume;
            seSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    private void Update()
    {
        // 毎フレーム代入するのは負荷になるため、
        // 本来はOnVolumeChanged内でのみ更新するのが理想的です
        seAudioSource.volume = seSetting.volume;
    }

    void OnVolumeChanged(float value)
    {
        seSetting.volume = value;
    }

    public enum SEType
    {
        Button_Click,
        Lord,
        StageClear,
        StageFailed,
        Summon,
        SummonFailed,
        Damage,
        Explosion,
        CanExplosion,
        BossDefeat,
        Upgrade,
        UnlockUnit,
    }

    /// <summary>
    /// SEを再生します。短時間の過剰な連打は自動でスキップされます。
    /// </summary>
    public void PlaySE(SEType seType)
    {
        // 1. 重要な音は制限をスルーさせる（必要に応じて）
        bool isImportant = (seType == SEType.StageClear || seType == SEType.StageFailed || seType == SEType.BossDefeat);

        // 2. 再生間隔のチェック
        if (!isImportant)
        {
            if (Time.time - lastPlayTimes[seType] < minPlayInterval)
            {
                // 前回の再生から時間が経っていないなら何もしない
                return;
            }
        }

        // 3. 再生処理
        int index = (int)seType;
        if (index >= 0 && index < seClips.Count)
        {
            seAudioSource.PlayOneShot(seClips[index]);
            // 再生時間を記録
            lastPlayTimes[seType] = Time.time;
        }
        else
        {
            Debug.LogWarning($"SEType {seType} に対応するクリップがリストにありません。");
        }
    }
}