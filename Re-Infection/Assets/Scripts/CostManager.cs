using TMPro;
using UnityEngine;

public class CostManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI costText;

    public int currentCost { get; private set; } = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AddCost(30);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �R�X�g�ǉ�
    public void AddCost(int value)
    {
        currentCost += value;
        costText.text = currentCost.ToString();
    }

    // �R�X�g����
    public void RemoveCost(int value)
    {
        currentCost -= value;
        currentCost = Mathf.Max(currentCost, 0);
        costText.text = currentCost.ToString();
    }

    // �R�X�g������Ă��邩
    public bool EnoughCost(int unitCost)
    {
        return currentCost >= unitCost;
    }
}
