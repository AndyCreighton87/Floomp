using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EngageState : State {

    private INodeObject target;

    private Action<INodeObject> onEnteredAttackRange;

    public EngageState(Unit _unit, INodeObject _target, Action<INodeObject> _callback) : base(_unit) {
        target = _target;
        onEnteredAttackRange = _callback;
    }

    public override void Enter() {
        unit.pathfinding.MoveTo(target.Transform);
    }

    public override void Execute() {
        unit.pathfinding.UpdateTarget(target.Transform);
        CheckDistanceToTarget();
    }

    public override void Exit() {
        unit.pathfinding.StopMove();
    }

    private void CheckDistanceToTarget() {
        float attackRange = unit.AttackRange * GridManager.Instance.nodeDiameter;
        float sqrAttackRange = attackRange * attackRange;
        float sqrDist = (unit.Position - target.Position).sqrMagnitude;

        if (sqrAttackRange > sqrDist) {
            onEnteredAttackRange?.Invoke(target);
        }
    }
}
