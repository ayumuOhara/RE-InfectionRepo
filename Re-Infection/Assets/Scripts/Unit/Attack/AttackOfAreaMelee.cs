using UnityEngine;

public class AttackOfAreaMelee : AttackBase
{
    public override void Attack(UnitBase attacker)
    {
        // ©g‚ğ’†S‚É‚µ‚½”ÍˆÍ
        Collider2D[] hits = Physics2D.OverlapCircleAll(attacker.transform.position, attacker.Stats.range, attacker.TargetLayer);

        // ”ÍˆÍ“à‚É“G‚ª‚¢‚È‚¢ê‡AI—¹
        if(hits.Length <= 0 || hits == null) return;

        // ƒqƒbƒg”‚Ü‚ÅŒJ‚è•Ô‚·
        var cnt = 0;        
        while (cnt < attacker.Stats.hitCnt)
        {
            if (cnt >= hits.Length) break;

            DamageToTarget(attacker, hits[cnt].gameObject);
            cnt++;
        }

        Debug.Log($"{attacker.Stats.unitName}‚ª{cnt}‘Ì‚Éƒqƒbƒg");
        //Debug.Log($"UŒ‚Ò: {attacker.gameObject.layer} UŒ‚‘ÎÛ”: {cnt}");
    }
}
