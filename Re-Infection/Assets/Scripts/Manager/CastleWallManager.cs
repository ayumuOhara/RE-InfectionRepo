using TMPro;
using UnityEngine;

public class CastleWallManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentHpText;

    public float maxHp { get; private set; } = 1000;

    public float currentHp { get; private set; }

    public bool isBreak => currentHp <= 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHp;
        currentHpText.text = currentHp.ToString("F0");
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
            FindAnyObjectByType<InGameUIManager>().StageFailed();
        }
        currentHpText.text = currentHp.ToString("F0");
    }
}
