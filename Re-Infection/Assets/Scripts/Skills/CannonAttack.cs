using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CannonAttack : MonoBehaviour
{
    public static event Action<float> OnSkillUsed;

    UnitManager unitManager;

    [SerializeField] CannonSkillStats cannonSkillStats;
    [SerializeField] LayerMask skillTargetLayer;

    const float VISUAL_RANGE = 2f;

    private void Awake()
    {
        OnSkillUsed += GameObject.Find("CannonSkillPointer").GetComponent<SkillDragger>().OnSkillUse;

        transform.localScale = new Vector3(cannonSkillStats.cannonRadius * VISUAL_RANGE, cannonSkillStats.cannonRadius * VISUAL_RANGE);

        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
    }

    async void OnEnable()
    {
        await WaitEndDrag.WaitDragEndAsync();
        if (unitManager.EnemyCnt <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        var targetUnits = Physics2D.OverlapCircleAll(transform.position, cannonSkillStats.cannonRadius, skillTargetLayer);

        if (targetUnits.Length <= 0 || targetUnits == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(AllTargetDamage(targetUnits));
        }
    }

    private void Update()
    {
        //UnitBase.DrawDebugCircle(transform.position, cannonSkillStats.cannonRadius, Color.red, 0.5f);
    }

    // 取得したターゲットにダメージ
    IEnumerator AllTargetDamage(Collider2D[] targetUnits)
    {
        foreach (Collider2D target in targetUnits)
        {
            var enemy = target.GetComponent<EnemyUnit>();

            // 範囲内にいるターゲット全てにダメージ
            if (enemy.IsDead == false)
            {
                enemy.Damage(cannonSkillStats.cannonDamage);
                // 倒した敵の死体を複製(ボスユニット除外)
                if (enemy.CurrentHealth <= 0 && !enemy.Stats.bossUnit)
                {
                    EnemyUnit clone = Instantiate(target.gameObject, target.transform.position + new Vector3(0.1f, 0, 0), Quaternion.identity).GetComponent<EnemyUnit>();
                    clone.Initialize(enemy.Stats, true);
                }
            }
            
            yield return null;
        }

        OnSkillUsed?.Invoke(cannonSkillStats.coolTime);
        OnSkillUsed -= GameObject.Find("CannonSkillPointer").GetComponent<SkillDragger>().OnSkillUse;

        // 処理終了後、非アクティブ化
        gameObject.SetActive(false);
    }
}
