using UnityEngine;

public class AttackOfAreaMelee : AttackBase
{
    public override void Attack(UnitBase attacker)
    {
        // ©g‚ğ’†S‚É‚µ‚½”ÍˆÍ
        Collider2D[] hits = Physics2D.OverlapCircleAll(attacker.transform.position, attacker.Stats.range, attacker.TargetLayer);
#if UNITY_EDITOR
        DrawDebugCircle(attacker.TargetPos, attacker.Stats.range, Color.red, 0.5f);
#endif

        // ”ÍˆÍ“à‚É“G‚ª‚¢‚È‚¢ê‡AI—¹
        if (hits.Length <= 0 || hits == null) return;

        // ƒqƒbƒg”‚Ü‚ÅŒJ‚è•Ô‚·
        var cnt = 0;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].gameObject.tag == "Castle" || hits[i].gameObject.GetComponent<UnitBase>()?.IsDead == false)
            {
                DamageToTarget(attacker, hits[i].gameObject);
                cnt++;
            }

            if (cnt >= attacker.Stats.hitCnt)
            {
                break;
            }
        }

        Debug.Log($"{attacker.Stats.unitName}‚ª{cnt}‘Ì‚Éƒqƒbƒg");
        //Debug.Log($"UŒ‚Ò: {attacker.gameObject.layer} UŒ‚‘ÎÛ”: {cnt}");
    }

    // ‰~‚ğ•`‰æ‚·‚é‚½‚ß‚Ì•â•ƒƒ\ƒbƒh
    private void DrawDebugCircle(Vector2 center, float radius, Color color, float duration)
    {
        int segments = 20; // ‰~‚ğ\¬‚·‚éü‚Ì”
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector2(radius, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);

            // Sceneƒrƒ…[‚Éü‚ğ•`‰æ
            Debug.DrawLine(prevPoint, nextPoint, color, duration);
            prevPoint = nextPoint;
        }
    }
}
