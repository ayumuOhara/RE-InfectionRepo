using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DeadState : IUnitState
{
    UnitController unitController;

    public DeadState(UnitController controller)
    {
        unitController = controller;
    }

    public void Enter()
    {
        Dead();

        if (unitController.group == UnitGroup.Enemy && !unitController.bossUnit)
        {
            if (unitController.isInfection)
            {
                unitController.gameObject.SetActive(false);
            }

            SpriteRenderer sr = unitController.gameObject.GetComponent<SpriteRenderer>();
            sr.sprite = unitController.corpseSprite;

            unitController.unitManager.AddCorpseList(unitController);
        }
        else
        {
            unitController.gameObject.SetActive(false);
        }
    }

    public void Update()
    {

    }

    public void Exit()
    {
        // ïúäàéûÇÃèàóù
        unitController.unitManager.RemoveCorpseList(unitController);
        unitController.unitManager.AddUnitList(unitController, UnitGroup.Player);

        SpriteRenderer sr = unitController.gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = unitController.unitSprite;
    }

    // éÄñS
    void Dead()
    {
        unitController.unitManager.RemoveUnitList(unitController, unitController.group);
        unitController.InstanceObjHeadUp(unitController.deadIconPrefab);
        
        if(unitController.group == UnitGroup.Enemy)
            unitController.waveSpawner.DecreaseEnemySum();
    }

    // éûä‘Ç…íBÇµÇΩÇÁä¥êıÇ≥ÇπÇÈ
    public IEnumerator Infectioning()
    {
        Debug.Log("ä¥êıäJén");

        unitController.unitUI.SetActive(true);

        float timer = 0;

        while(timer < unitController.infecitonTime)
        {
            timer += Time.deltaTime;

            unitController.infectionRateGauge.fillAmount = timer / unitController.infecitonTime;

            yield return null;
        }

        unitController.unitUI.SetActive(false);

        unitController.HealHelth(unitController.maxHp * 0.5f);
    }
}