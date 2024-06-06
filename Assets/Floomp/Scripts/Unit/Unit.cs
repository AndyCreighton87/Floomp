using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PathfindingComponent))]
public class Unit : PoolableObject, IAttackable, INodeObject
{
    [Header("Debug")]
    public bool Stationary = false;

    // IAttackable
    public Transform Transform => transform;
    public bool IsAlive => Health > 0;

    public event Action<int> OnHealthChanged;

    // INodeObject
    public Vector3 Position => transform.position;


    // Unit Parameters
    [SerializeField] private int health = 100;
    [SerializeField] private int attackRange = 1;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private int attackSpeed = 1;

    public int Health => health;
    public int AttackRange => attackRange;
    public int AttackDamage => attackDamage;
    public float AttackSpeed => attackSpeed;

    // State
    private State currentState;

    // Team
    public Team Team { get; set; }
    [SerializeField] private Renderer unitRenderer;

    // Componenets
    private HealthBar healthBar;

    public PathfindingComponent pathfinding { get; private set; }

    public void Init(Team _team) {
        Team = _team;
        SetTeam(Team);

        pathfinding = GetComponent<PathfindingComponent>();
        pathfinding.OnNodeChanged += OnNodeChanged;

        healthBar = PoolManager.Instance.GetObject("HealthBar") as HealthBar;
        healthBar.BindToAttackable(this);

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
        if (!IsAlive) {
            return;
        }

        Target target = TargetManager.Instance.GetClosestTarget(Team, Position);

        if (target != null) {
            State state = new MoveState(this, target.transform, OnEnemyDetected);
            SetState(state);
        }
    }

    private void OnEnemyDetected(INodeObject _enemy) {
        if (!IsAlive) {
            return;
        }

        State state = new EngageState(this, _enemy, BeginAttack, SetDefaultState);
        SetState(state);
    }

    private void OnNodeChanged(Node _newNode, Node _prevNode) {
        GridManager gridManager = GridManager.Instance;

        gridManager.AddObjectToNode(_newNode, this);
        gridManager.RemoveObjectFromNode(_prevNode, this);
    }

    private void BeginAttack(INodeObject _enemy) {
        if (_enemy is IAttackable _attackable) {
            State state = new AttackState(this, _attackable, SetDefaultState);
            SetState(state);
        } else {
            Debug.Log($"{_enemy} does not implement the IAttackable interface. Reverting to default state.");
            SetDefaultState();
        }
    }

    public void TakeDamage(int _damage) {
        health -= _damage;

        OnHealthChanged?.Invoke(Health);

        if (Health <= 0) {
            State state = new DeathState(this);
            SetState(state);
        }
    }

    public void ReportDeath() {
        pathfinding.StopMove();
        GridManager.Instance.RemoveObjectFromNode(this);
        PoolManager.Instance.ReturnObject(healthBar);
    }

    // Just for debugging to make things easier - will make a more concrete solution later
    public void SetTeam(Team _team) {
        switch (_team) {
            case Team.Blue:
                unitRenderer.material.color = Color.blue;
                break;
            case Team.Red:
                unitRenderer.material.color = Color.red;
                break;
        }
    }
}
