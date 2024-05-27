using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Team team;

    [Header("Pathfinding Variables")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private float turnDistance = 5f;
    [SerializeField] private float turnSpeed = 3f;
    [SerializeField] private float stoppingDist = 10f;

    public float Speed => speed;
    public float TurnDistance => turnDistance;
    public float TurnSpeed => turnSpeed;
    public float StoppingDist => stoppingDist;

    public Team Team => team;

    public Vector3 position => transform.position;

    private State currentState;

    private void Start() {
        Init();
    }

    public void Init() {
        SetDefaultState();
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
        Target target = TargetManager.Instance.GetClosestTarget(Team, position);
        State state = new MoveState(this, target.transform);
        SetState(state);
    }
}
