using UnityEngine;

public class AttackState : IUnitState
{
    UnitController unitController;
    float atkTimer = 0; // 攻撃タイマー

    public AttackState(UnitController controller)
    {
        unitController = controller;
    }

    public void Enter()
    {

    }

    public void Update()
    {
        atkTimer += Time.deltaTime;

        if (atkTimer >= unitController.atkInterbal)
        {
            if(unitController.targetObj != null)
            {
                UnitController uc = unitController.targetObj.GetComponent<UnitController>();
                uc.TakeDamage(unitController.atk);
            }
            else if(unitController.castleObj != null && unitController.group == UnitGroup.Enemy)
            {
                CastleWallManager cm = unitController.castleObj.GetComponent<CastleWallManager>();
                cm.TakeDamage(unitController.atk);
            }

            atkTimer = 0;
        }
    }

    public void Exit()
    {

    }
}