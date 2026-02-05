using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CastleWallManager : MonoBehaviour
{
    PlayerStatusData playerStatusData;

    [SerializeField] TextMeshProUGUI currentHpText;
    [SerializeField] Slider healthBar;

    public float currentHp { get; private set; }

    public bool isBreak => currentHp <= 0;

    private void Awake()
    {
        playerStatusData = Resources.Load<PlayerStatusData>("PlayerStatusData");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = playerStatusData.castleUpgrade.Health;
        currentHpText.text = currentHp.ToString("F0");
        healthBar.value = currentHp / playerStatusData.castleUpgrade.Health;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            currentHp = 0;
        }
        currentHpText.text = currentHp.ToString("F0");
        healthBar.value = currentHp / playerStatusData.castleUpgrade.Health;
    }
}
