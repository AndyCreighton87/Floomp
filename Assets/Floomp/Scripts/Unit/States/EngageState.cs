using System;

public class EngageState : State {

    private INodeObject target;

    private Action<INodeObject> onEnteredAttackRange;
    private Action onTargetDead;

    public EngageState(Unit _unit, INodeObject _target, Action<INodeObject> _onEnteredAttackRange, Action _onTargetDead) : base(_unit) {
        target = _target;
        onEnteredAttackRange = _onEnteredAttackRange;
        onTargetDead = _onTargetDead;
    }

    public override void Enter() {
        unit.pathfinding.MoveTo(target.Transform);
    }

    public override void Execute() {
        Unit test = target as Unit;

        if (target == null || !test.IsAlive) {
            onTargetDead?.Invoke();
            return;
        }

        CheckDistanceToTarget();
    }

    public override void Exit() {
        unit.pathfinding.CancelPath();
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
