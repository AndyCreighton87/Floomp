using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Team team;
    public Team Team => team;

    public Vector3 position => transform.position;
}
