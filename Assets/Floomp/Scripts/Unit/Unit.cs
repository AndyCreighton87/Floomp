using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathfindingComponent))]
public class Unit : MonoBehaviour, IAttackable, INodeObject
{
    [SerializeField] private Team team;

    [Header("Debug")]
    public bool Stationary = false;

    public Team Team => team;

    public Vector3 Position => transform.position;

    public Transform Transform => transform;

    private State currentState;

    public PathfindingComponent pathfinding { get; private set; }

    private void Start() {
        Init();
    }

    public void Init() {
        pathfinding = GetComponent<PathfindingComponent>();
        pathfinding.OnNodeChanged += OnNodeChanged;

        GridManager.Instance.AddObjectToNode(Position, this);

        if (!Stationary) {
            SetDefaultState();
        }
    }

    private void Update() {
        if (currentState != null) {
            currentState.Execute();
        }
    }

    public void SetState(State _state) {
        if (currentState != null) {
            currentState.Exit();
        }

        currentState = _state;
        currentState.Enter();
    }

    // A units default state should be moving towards the enemy target
    private void SetDefaultState() {
        Target target = TargetManager.Instance.GetClosestTarget(Team, Position);

        if (target != null) {
            State state = new MoveState(this, target.transform, OnEnemyDetected);
            SetState(state);
        }
    }

    private void OnEnemyDetected(INodeObject _enemy) {
        State state = new EngageState(this, _enemy.Transform);
        SetState(state);
    }

    private void OnNodeChanged(Node _newNode, Node _prevNode) {
        GridManager gridManager = GridManager.Instance;

        gridManager.AddObjectToNode(_newNode, this);
        gridManager.RemoveObjectFromNode(_prevNode, this);
    }
}
