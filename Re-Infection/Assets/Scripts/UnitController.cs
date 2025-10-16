using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;

// ���j�b�g�̏������Ă���O���[�v
public enum UnitGroup
{
    Player,
    Enemy,
}

public class UnitController : MonoBehaviour
{
    GameObject targetObj;   // �G�I�u�W�F�N�g
    [SerializeField] GameObject damageTextPrefab;�@// �_���[�W���\���e�L�X�g
    [SerializeField] Sprite corpseSprite;          // ���̃X�v���C�g

    const float UNIT_SCALE = 0.4f;
    Vector3 myScale = new Vector3(UNIT_SCALE, UNIT_SCALE, UNIT_SCALE); // ���j�b�g�̃T�C�Y
    public Vector3 kingPos { get; private set; }     // ���l�̍��W
    public Vector3 targetPos { get; private set; }   // �G�̍��W
    public Vector3 myPos { get; private set; }       // ���g�̍��W

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
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = original.unitSprite;

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
        moveSpeed = stats.moveSpeed * 0.1f;
        range = stats.range;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        kingPos = GameObject.Find("King").transform.position;
        myPos = transform.position;
        transform.localScale = myScale;

        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (isMoving)
            {
                targetObj = GetTarget.GetTargetObj(group == UnitGroup.Player ? UnitGroup.Enemy : UnitGroup.Player, myPos);
                if (targetObj != null)
                {
                    targetPos = targetObj.transform.position;
                    MoveToTarget();
                }
                else
                {
                    if (group == UnitGroup.Player)
                        MovePlayerUnit();
                    if (group == UnitGroup.Enemy)
                        MoveEnemyUnit();
                }
            }

            if (Vector3.Distance(targetPos, transform.position) <= range && targetObj != null)
            {
                targetObj = GetTarget.GetTargetObj(group == UnitGroup.Player ? UnitGroup.Enemy : UnitGroup.Player, myPos);
                if (targetObj != null)
                {
                    targetPos = targetObj.transform.position;
                }

                isMoving = false;
                Attack();
            }
            else
            {
                isMoving = true;
            }
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

    void MoveToTarget()
    {
        moveDirection = (targetPos - myPos).normalized;
        myPos += moveDirection * moveSpeed * Time.deltaTime;
        transform.position = myPos;
    }

    // �_���[�W����
    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        var unitPos = Camera.main.WorldToScreenPoint(transform.position);   // ���j�b�g�̃��[���h���W���X�N���[�����W�ɕϊ�
        unitPos.y += 0.3f;
        GameObject textObj = Instantiate(damageTextPrefab, GameObject.Find("UI").transform, false); // ���j�b�g�̏�����Ƀ_���[�W�e�L�X�g�𐶐�
        textObj.transform.position = unitPos;

        // �_���[�W��\������
        TextMeshProUGUI damageText = textObj.GetComponent<TextMeshProUGUI>();
        damageText.text = damage.ToString();

        if (isDead)
            Dead();
    }

    // �U��
    void Attack()
    {
        atkInterbal += Time.deltaTime;
        if (atkInterbal > atkRate)
        {
            atkInterbal = 0;

            if (targetObj != null)
            {
                UnitController target = targetObj.GetComponent<UnitController>();
                target.TakeDamage(atk);
            }
        }
    }

    // ���S����
    void Dead()
    {
        UnitManager um = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        um.RemoveUnitList(gameObject, group);

        if (group == UnitGroup.Enemy)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = corpseSprite;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
