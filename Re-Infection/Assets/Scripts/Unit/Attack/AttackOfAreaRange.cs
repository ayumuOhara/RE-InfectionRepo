using UnityEngine;

public class AttackOfAreaRange : AttackBase
{
    public override void Attack(UnitBase attacker)
    {
        // UŒ‚‘ÎÛ‚ğ’†S‚É‚µ‚½”ÍˆÍ
        Collider2D[] hits = Physics2D.OverlapCircleAll(attacker.TargetPos, attacker.Stats.range, attacker.TargetLayer);

        // ”ÍˆÍ“à‚É“G‚ª‚¢‚È‚¢ê‡AI—¹
        if(hits.Length <= 0 || hits == null) return;

        // ƒqƒbƒg”‚Ü‚ÅŒJ‚è•Ô‚·
        var cnt = 0;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].gameObject.GetComponent<UnitBase>().IsDead == false)
            {
                DamageToTarget(attacker, hits[i].gameObject);
                cnt++;
                if (cnt >= attacker.Stats.hitCnt)
                {
                    break;
                }
            }
        }

        Debug.Log($"{attacker.Stats.unitName}‚ª{cnt}‘Ì‚Éƒqƒbƒg");

        //Debug.Log($"UŒ‚Ò: {attacker.gameObject.layer} UŒ‚‘ÎÛ”: {cnt}");
    }
}
