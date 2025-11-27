using UnityEngine;

public interface IHealth
{
    public void Damage(float damage);

    public void Heal(float heal);

    public void Dead();
}
