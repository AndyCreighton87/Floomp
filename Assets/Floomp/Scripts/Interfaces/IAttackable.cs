using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public Team Team { get; }

    public bool IsAlive { get; }

    public void TakeDamage(int _damage);
}
