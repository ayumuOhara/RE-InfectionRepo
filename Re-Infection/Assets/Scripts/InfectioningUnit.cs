using UnityEngine;

public class InfectioningUnit : MonoBehaviour
{
    private async void OnTriggerEnter2D(Collider2D collision)
    {
        await WaitEndDrag.WaitDragEndAsync();

        UnitController uc = collision.gameObject.GetComponent<UnitController>();
        if (uc.isDead && uc.group == UnitGroup.Enemy)
        {
            uc.Infection();
        }

        gameObject.SetActive(false);
    }
}
