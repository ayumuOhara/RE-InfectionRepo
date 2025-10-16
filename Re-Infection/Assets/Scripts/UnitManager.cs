using UnityEngine;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{
    public List<GameObject> playerUnitList { get; private set; } = new List<GameObject>();    // �v���C���[���j�b�g�i�[���X�g
    public List<GameObject> enemyUnitList { get; private set; } = new List<GameObject>();     // �G�l�~�[���j�b�g�i�[���X�g

    // �G�̐���Ԃ�
    public int EnemyCnt => enemyUnitList.Count;

    // �G���S�ł������Ԃ�
    public bool IsAllEnemyDefeated => enemyUnitList.Count <= 0;

    // ���j�b�g�����X�g�ɒǉ�
    public void AddUnitList(GameObject unitObj, UnitGroup group)
    {
        if(group == UnitGroup.Player)
            playerUnitList.Add(unitObj);
        if(group == UnitGroup.Enemy)
            enemyUnitList.Add(unitObj);
    }

    // ���j�b�g�����X�g����폜
    public void RemoveUnitList(GameObject unitObj, UnitGroup group)
    {
        if (group == UnitGroup.Player)
            playerUnitList.Remove(unitObj);
        if (group == UnitGroup.Enemy)
            enemyUnitList.Remove(unitObj);
    }

    // ���j�b�g�̃��X�g���擾
    public List<GameObject> GetUnitList(UnitGroup group)
    {
        return group == UnitGroup.Player ? playerUnitList : enemyUnitList;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
