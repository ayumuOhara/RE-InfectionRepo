using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI.Extensions;

[CreateAssetMenu(fileName = "AreaAttackData", menuName = "Scriptable Objects/AreaAttackData")]
public class AreaAttackData : AttackDataBase
{
    [Header("ƒqƒbƒg‚·‚é“G‚ÌÅ‘å”")]
    public int hitMaxCnt;

    public override void Attack(LayerMask targetLayer, UnitBase attacker, float damage, float range)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attacker.transform.position, range, targetLayer);

        // ”ÍˆÍ“à‚É“G‚ª‚¢‚È‚¢ê‡AI—¹
        if(hits.Length <= 0 || hits == null) return;

        var cnt = 0;        
        while (cnt < hitMaxCnt)
        {
            if (cnt >= hits.Length) break;

            UnitBase unitBase = hits[cnt].GetComponent<UnitBase>();
            unitBase.Damage(damage);
            ++cnt;
        }

        //Debug.Log($"UŒ‚Ò: {attacker.gameObject.layer} UŒ‚‘ÎÛ”: {cnt}");
    }
}
