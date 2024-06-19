using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    private Transform target;

    private Vector3 lastPosition;
    private float moveThresholdDistanceSquared = 0.25f;

    private GridManager gridManager;
    private Node currentNode;

    private int enemyScanRange = 10;

    private Action<INodeObject> onEnemyDetected;

    public MoveState(Unit _unit, Node _node, Transform _target, Action<INodeObject> _callback) : base(_unit) {
        gridManager = GridManager.Instance;

        target = _target;
        currentNode = _node;
        onEnemyDetected = _callback;

        AnimationHandler.MoveToAnimation(_unit.animationController, StringLibrary.moveAnimation);
    }

    public override void Enter() {
        unit.pathfinding.MoveTo(target);
    }

    public override void Execute() {
        CheckUpdateCurrentNode();
        ScanForEnemies();
    }

    public override void Exit() {

    }

    private void CheckUpdateCurrentNode() {
        Vector3 newPosition = unit.Position;
        float distanceSquared = (newPosition - lastPosition).sqrMagnitude;

        if (distanceSquared > moveThresholdDistanceSquared) {
            Node newNode = gridManager.NodeFromWorldPosition(newPosition);
            if (newNode != currentNode) {
                currentNode = newNode;
                gridManager.AddObjectToNode(currentNode, unit);
                gridManager.RemoveObjectFromNode(lastPosition, unit);
            }

            lastPosition = newPosition;
        }
    }

    private void ScanForEnemies() {
        List<INodeObject> objects = gridManager.GetObjectsInSurroundingNode(currentNode, enemyScanRange);

        foreach(INodeObject obj in objects) {
            if (obj != null && obj is IAttackable) {
                IAttackable attackable = (IAttackable)obj;
                if (attackable.IsAlive && attackable.Team != unit.Team) {
                    onEnemyDetected?.Invoke(obj);
                    Debug.Log("Enemy detected");
                    return;
                }
            }
        }
    }
}
