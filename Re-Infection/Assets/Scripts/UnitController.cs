using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ���j�b�g�̏������Ă���O���[�v
public enum UnitGroup
{
    Player,
    Enemy,
}

public class UnitController : MonoBehaviour
{
    GameObject targetObj;   // �G�I�u�W�F�N�g

    public Vector3 kingPos { get; private set; }    // ���l�̍��W
    public Vector3 targetPos { get; private set; }  // �G�̍��W
    public Vector3 myPos { get; private set; }      // ���g�̍��W

    Vector3 moveDirection;  // �ړ�����

    float atkInterbal = 0;  // �U����̌o�ߎ���

    // ���j�b�g�̃X�e�[�^�X
    public UnitGroup group { get; private set; }   // �������G��
    public float currentHp { get; private set; }   // ����HP
    public float maxHp { get; private set; }       // �ő�HP
    public float atk { get; private set; }         // �U����
    public float atkRate { get; private set; }     // �U���Ԋu
    public float moveSpeed { get; private set; }   // �ړ����x
    public float range { get; private set; }       // �U������
    bool isDead => currentHp <= 0;                 // ���S�t���O

    bool isMoving = true;   // �ړ��t���O

    // ������
    public void SetUnitStats(UnitStats original, UnitGroup group)
    {
        this.group = group;

        // �f�[�^�𕡐�
        var stats = new UnitStats()
        {
            maxHp = original.maxHp,
            atk = original.atk,
            atkRate = original.atkRate,
            moveSpeed = original.moveSpeed,
            range = original.range,
        };

        // ���������f�[�^����
        currentHp = stats.maxHp;
        maxHp = stats.maxHp;
        atk = stats.atk;
        atkRate = stats.atkRate;
        moveSpeed = stats.moveSpeed;
        range = stats.range;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        kingPos = GameObject.Find("King").transform.position;
        myPos = transform.position;
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        Dead();

        if (isMoving)
        {
            if (group == UnitGroup.Player)
                MovePlayerUnit();
            if (group == UnitGroup.Enemy)
                MoveEnemyUnit();
        }

        targetObj = GetTarget.GetTargetObj(group == UnitGroup.Player ? UnitGroup.Enemy : UnitGroup.Player, myPos);
        if(targetObj != null)
            targetPos = targetObj.transform.position;

        if (Vector3.Distance(targetPos, transform.position) <= range && targetObj != null)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    void MovePlayerUnit()
    {
        myPos += Vector3.up * moveSpeed * Time.deltaTime;
        transform.position = myPos;
    }

    void MoveEnemyUnit()
    {
        moveDirection = (kingPos - myPos).normalized;
        myPos += moveDirection * moveSpeed * Time.deltaTime;
        transform.position = myPos;
    }

    // �_���[�W����
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        Debug.Log($"{damage}�_���[�W���󂯂��I");
    }

    // �U��
    void Attack()
    {
        atkInterbal += Time.deltaTime;
        if (atkInterbal > atkRate)
        {
            atkInterbal = 0;

            UnitController target = targetObj.GetComponent<UnitController>();
            target.TakeDamage(atk);
        }
    }

    // ���S����
    void Dead()
    {
        if (!isDead) return;

        UnitManager um = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        um.RemoveUnitList(gameObject, group);
        Destroy(gameObject);
    }
}
