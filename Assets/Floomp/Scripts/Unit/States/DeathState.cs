using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State {

    public DeathState(Unit _unit) : base(_unit) { }

    public override void Enter() {
        Debug.Log("Death state entered");

        unit.ReportDeath();
        unit.StartCoroutine(DestroyUnitAfterDelay());
    }

    public override void Execute() {

    }

    public override void Exit() {

    }

    // This will eventually be replaced by an animation - the delay from the death animation should suffice
    private IEnumerator DestroyUnitAfterDelay() {
        yield return new WaitForSeconds(1.0f);
        GameObject.Destroy(unit.gameObject);
    }
}
