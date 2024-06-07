using System;
using System.Collections;
using UnityEngine;

public class AttackState : State {

    private IAttackable target;
    private Action success;

    public AttackState(Unit _unit, IAttackable _target, Action _callback) : base(_unit) {
        target = _target;
        success = _callback;
    }

    public override void Enter() {
        unit.StartCoroutine(Attack());
    }

    public override void Execute() {

    }

    public override void Exit() {
        unit.StopCoroutine(Attack());
    }

    private IEnumerator Attack() {
        while (target != null && target.IsAlive) {
            if (this == null) yield break;
            target.TakeDamage(unit.AttackDamage);

            Debug.Log($"{unit.name} attacks {target} for {unit.AttackDamage}");

            yield return new WaitForSeconds(unit.AttackSpeed);
        }

        success?.Invoke();
    }
}
