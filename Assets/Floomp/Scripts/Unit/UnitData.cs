using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/UnitData")]
public class UnitData : ScriptableObject
{
    [Header("HP")]
    public int health;

    [Header("Attack")]
    public int attackRange;
    public int attackDamage;
    public float attackSpeed;
}
