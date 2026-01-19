using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] WaveSpawner waveSpawner;
    [SerializeField] GameObject gameSpdButton;
    [SerializeField] Image pauseCover;
    [SerializeField] Sprite normalSpdIcon;
    [SerializeField] Sprite doubleSpdIcon;

    Image gameSpdSprite;

    float seconds = 0;
    public int Seconds => (int)seconds;
    int minutes = 0;
    public int Minutes => minutes;

    public bool isPause { get; private set; } = false;
    bool isAcceleration = false;
    float timeSpeed = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseCover.enabled = isPause;
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
        while (true)
        {
            yield return new WaitUntil(() => waveSpawner.IsStartWave);

            seconds += Time.deltaTime;

            if (seconds >= 60)
            {
                minutes++;
                seconds = 0;
            }
        }
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

    public void GamePause()
    {
        if (isPause)
            Time.timeScale = timeSpeed;
        else
            Time.timeScale = 0;

        isPause = !isPause;
        pauseCover.enabled = isPause;
    }
}
