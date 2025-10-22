using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] WaveSpawner waveSpawner;
    [SerializeField] CastleWallManager castleWallManager;
    [SerializeField] StageUIManager stageUIManager;
    [SerializeField] UnitManager unitManager;

    [SerializeField] Image resultUI;
    [SerializeField] Image clearUI;
    [SerializeField] Image failedUI;

    void Awake()
    {
        Application.targetFrameRate = 120;
        resultUI.gameObject.SetActive(false);
        clearUI.gameObject.SetActive(false);
        failedUI.gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waveSpawner.isStageCompleted)
        {
            Debug.Log("Stage Completed !!");
            resultUI.gameObject.SetActive(true);
            clearUI.gameObject.SetActive(true);
        }

        if (castleWallManager.isBreak)
        {
            Debug.Log("Stage Failed ...");
            resultUI.gameObject.SetActive(true);
            failedUI.gameObject.SetActive(true);
        }

    }
}
