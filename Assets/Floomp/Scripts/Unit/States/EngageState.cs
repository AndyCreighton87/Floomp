using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EngageState : State {

    private Transform target;

    public EngageState(Unit _unit, Transform _target) : base(_unit) {
        target = _target;
    }

    public override void Enter() {
        unit.pathfinding.MoveTo(target);
    }

    public override void Execute() {

    }

    public override void Exit() {

    }
}
