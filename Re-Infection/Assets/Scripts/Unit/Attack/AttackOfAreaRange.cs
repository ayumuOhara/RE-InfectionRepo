using UnityEngine;

public class AttackOfAreaRange : AttackBase
{
    public override void Attack(UnitBase attacker)
    {
        // 攻撃対象を中心にした範囲
        Collider2D[] hits = Physics2D.OverlapCircleAll(attacker.TargetPos, attacker.Stats.radius, attacker.TargetLayer);

#if UNITY_EDITOR
        UnitBase.DrawDebugCircle(attacker.TargetPos, attacker.Stats.radius, Color.red, 0.5f);
#endif
        
        // 範囲内に敵がいない場合、終了
        if(hits.Length <= 0 || hits == null) return;

        #region -----ヒット確認とダメージ-----

        // ヒット数まで繰り返す
        var cnt = 0;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].gameObject.tag == "Castle" || hits[i].gameObject.GetComponent<UnitBase>()?.IsDead == false)
            {
                DamageToTarget(attacker, hits[i].gameObject);
                attacker.InstanceEffect(hits[i].gameObject.transform.position);

                cnt++;
            }

            if (cnt >= attacker.Stats.hitCnt)
            {
                break;
            }
        }

        #endregion ----------------------------

        Debug.Log($"{attacker.Stats.unitName}が{cnt}体にヒット");

        //Debug.Log($"攻撃者: {attacker.gameObject.layer} 攻撃対象数: {cnt}");
    }
}
