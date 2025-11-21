using System.Collections;
using TMPro;
using UnityEngine;

public class CostManager : MonoBehaviour
{
    WaveSpawner waveSpawner;
    InGameUIManager gameUIManager;

    [SerializeField] Animator costAnimator;

    [SerializeField] TextMeshProUGUI costText;

    [SerializeField] int startCost;
    [SerializeField] int maxCost;
    [SerializeField] float generateInterbal;

    public int currentCost { get; private set; } = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AddCost(startCost);
        waveSpawner = FindObjectOfType<WaveSpawner>();
        gameUIManager = FindObjectOfType<InGameUIManager>();
        StartCoroutine(GenerateCost());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GenerateCost()
    {
        var timer = 0f;

        while (true)
        {
            timer = 0;
            gameUIManager.CostGenerateGauge(timer / generateInterbal);

            yield return new WaitUntil(() => waveSpawner.IsStartWave);
            
            while (waveSpawner.IsStartWave)
            {
                timer += Time.deltaTime;
                gameUIManager.CostGenerateGauge(timer / generateInterbal);

                if (timer >= generateInterbal)
                {
                    timer = 0f;
                    AddCost(1);
                }

                yield return null;
            }

            yield return null;
        }        
    }

    // コスト追加
    public void AddCost(int value)
    {
        costAnimator.SetTrigger("Generate");
        currentCost += value;
        if (currentCost >= maxCost)
        {
            currentCost = maxCost;
        }
        costText.text = currentCost.ToString();
    }

    // コスト減少
    public void RemoveCost(int value)
    {
        costAnimator.SetTrigger("Used");
        currentCost -= value;
        currentCost = Mathf.Max(currentCost, 0);
        costText.text = currentCost.ToString();
    }

    // コストが足りているか
    public bool EnoughCost(int unitCost)
    {
        return currentCost >= unitCost;
    }
}
