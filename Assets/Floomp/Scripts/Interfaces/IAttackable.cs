using System;
using UnityEngine;

public interface IAttackable
{
    public Transform Transform { get; }

    public Team Team { get; set; }

    public bool IsAlive { get; }

    public int Health { get; }

    public void TakeDamage(int _damage);

    public event Action<int> OnHealthChanged;
}
