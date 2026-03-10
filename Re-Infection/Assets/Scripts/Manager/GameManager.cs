using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public WaveSpawner waveSpawner {  get; private set; }
    public CastleWallManager castleWallManager {  get; private set; }
    public UnitManager unitManager {  get; private set; }
    public CostManager costManager { get; private set; }
    public TimeManager timeManager { get; private set; }
    public InGameUIManager inGameUIManager { get; private set; }

    void Awake()
    {
        Application.targetFrameRate = 60;

        waveSpawner = GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>();
        castleWallManager = GameObject.Find("CastleWall").GetComponent <CastleWallManager>();
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        costManager = GameObject.Find("CostManager").GetComponent<CostManager>();
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        inGameUIManager = GameObject.Find("InGameUI").GetComponent<InGameUIManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StayEndSession());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StayEndSession()
    {
        do
        {
            if (waveSpawner.IsSessionClear)
            {
                StartCoroutine(inGameUIManager.SessionClear());
                SessionEnd();
                StartCoroutine(inGameUIManager.SessionReward());
                timeManager.SpeedReset();
                yield break;
            }
            if (castleWallManager.isBreak)
            {
                StartCoroutine(inGameUIManager.SessionFailed());
                SessionEnd();
                StartCoroutine(inGameUIManager.SessionReward());
                timeManager.SpeedReset();
                yield break;
            }

            yield return null;
        } while (!waveSpawner.IsSessionClear || !castleWallManager.isBreak);

        yield break;
    }

    void SessionEnd()
    {
        unitManager.AllUnitDestroy("Player");
        unitManager.AllUnitDestroy("Enemy");
        StopCoroutine(timeManager.SessionTimer());
    }
}
