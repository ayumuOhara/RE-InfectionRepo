using UnityEngine;
using UnityEngine.EventSystems;

public class MoveState : IUnitState
{
    UnitController unitController;
    Vector3 myPos;
    Vector3 castlePos;

    public MoveState(UnitController controller)
    {
        unitController = controller;
    }

    public void Enter()
    {
        myPos = unitController.gameObject.transform.position;
        if(castlePos == null)
            castlePos = GameObject.Find("CastleWall").transform.position;
    }

    public void Update()
    {
        if (unitController.targetObj != null)
        {
            Vector3 moveDirection = unitController.targetObj.transform.position - myPos;
            myPos += moveDirection.normalized * unitController.moveSpeed * Time.deltaTime;
        }
        else
        {
            if (unitController.group == UnitGroup.Enemy)
            {
                Vector3 moveDirection = unitController.castleObj.transform.position - myPos;
                myPos += moveDirection.normalized * unitController.moveSpeed * Time.deltaTime;
            }
        }

        unitController.gameObject.transform.position = myPos;
    }

    public void Exit()
    {

    }
}