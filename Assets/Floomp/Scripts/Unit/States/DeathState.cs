using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State {

    public DeathState(Unit _unit) : base(_unit) { }

    public override void Enter() {
        Debug.Log("Death state entered");
    }

    public override void Execute() {

    }

    public override void Exit() {

    }
}
