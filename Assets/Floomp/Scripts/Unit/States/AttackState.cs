using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State {

    public AttackState(Unit _unit) : base(_unit) { }

    public override void Enter() {
        Debug.Log("Attack state");
    }

    public override void Execute() {

    }

    public override void Exit() {

    }
}
