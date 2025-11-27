using UnityEngine;

public class AttackMeleeOfArea : AttackBase
{
    public override void Attack(UnitBase attacker)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attacker.transform.position, attacker.Stats.range, attacker.TargetLayer);

        // ”ÍˆÍ“à‚É“G‚ª‚¢‚È‚¢ê‡AI—¹
        if(hits.Length <= 0 || hits == null) return;

        var cnt = 0;        
        while (cnt < attacker.Stats.hitCnt)
        {
            if (cnt >= hits.Length) break;

            UnitBase unitBase = hits[cnt].GetComponent<UnitBase>();
            unitBase.Damage(attacker.Stats.atk);
            ++cnt;
        }

        //Debug.Log($"UŒ‚Ò: {attacker.gameObject.layer} UŒ‚‘ÎÛ”: {cnt}");
    }
}
