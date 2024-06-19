using System.Linq;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance;

    [SerializeField] private Target[] targets;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Target GetClosestTarget(Team _team, Vector3 _positon) {
        return targets
            .Where(target => target.Team != _team)
            .OrderBy(target => Vector3.Distance(target.position, _positon))
            .FirstOrDefault();
    }

    public Target GetTargetTest() {

       return targets[1];
    }
}

public enum Team {
    Blue,
    Red,
    Length
}
