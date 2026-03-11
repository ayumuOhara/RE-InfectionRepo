using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] WaveSpawner waveSpawner;
    [SerializeField] GameObject gameSpdButton;
    [SerializeField] Image pauseIcon;
    [SerializeField] Sprite normalSpdIcon;
    [SerializeField] Sprite doubleSpdIcon;

    Image gameSpdSprite;

    public float sessionTimer { get; private set; } = 0;
    public float sessionClearTime { get; private set; }

    public bool isPause { get; private set; } = false;
    bool isAcceleration = false;
    float timeSpeed = 1.0f;

    private void OnEnable()
    {
        WaveData.OnStopTime += GamePause;
    }

    private void OnDisable()
    {
        WaveData.OnStopTime -= GamePause;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = timeSpeed;
        gameSpdSprite = gameSpdButton.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // セッションのクリア時間を計る
    public IEnumerator SessionTimer()
    {
        sessionTimer = 0;

        while (true)
        {
            yield return new WaitUntil(() => waveSpawner.IsStartWave);

            sessionTimer += Time.deltaTime;
        }
    }

    public void SetSessionClearTime()
    {
        sessionClearTime = sessionTimer;
    }

    public string GetSessionClearTimeText()
    {
        int minutes = (int)(sessionClearTime / 60f);
        int secondes = (int)sessionClearTime - (minutes * 60);

        return minutes.ToString("D2") + ":" + secondes.ToString("D2");
    }

    // 停止
    public void OnPause()
    {
        GamePause();
    }

    // 速度切り替え
    public void OnSpeedChage()
    {
        if (isAcceleration)
        {
            timeSpeed = 1.0f;
            gameSpdSprite.sprite = normalSpdIcon;
        }
        else
        {
            timeSpeed = 2.0f;
            gameSpdSprite.sprite = doubleSpdIcon;
        }

        isAcceleration = !isAcceleration;

        if (!isPause)
            Time.timeScale = timeSpeed;
    }

    public void SpeedReset()
    {
        timeSpeed = 1.0f;
        Time.timeScale = timeSpeed;
    }

    public void GamePause()
    {
        isPause = !isPause;

        if (isPause)
        {
            Time.timeScale = 0;
            pauseIcon.color = Color.red;
        }
        else
        {
            Time.timeScale = timeSpeed;
            pauseIcon.color = Color.white;
        }
    }
}
