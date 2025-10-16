using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "Scriptable Objects/UnitStats")]
public class UnitStats : ScriptableObject
{
    public Sprite unitSprite;   // ���j�b�g�̃X�v���C�g
    public float maxHp;         // �ő�HP
    public float atk;           // �U����
    public float atkRate;       // �U�����x
    public float moveSpeed;     // �ړ����x
    public float range;         // �˒�����
    public int summonCost;      // �����R�X�g
}
