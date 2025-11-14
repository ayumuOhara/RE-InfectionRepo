using UnityEngine;
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
        Application.targetFrameRate = 120;

        waveSpawner = GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>();
        castleWallManager = GameObject.Find("CastleWall").GetComponent <CastleWallManager>();
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        costManager = GameObject.Find("CostManager").GetComponent<CostManager>();
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        inGameUIManager = GameObject.Find("InGameUIManager").GetComponent<InGameUIManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waveSpawner.IsSessionClear)
        {
            inGameUIManager.StageClear();
        }
        if (castleWallManager.isBreak)
        {
            inGameUIManager.StageFailed();
        }
    }
}
