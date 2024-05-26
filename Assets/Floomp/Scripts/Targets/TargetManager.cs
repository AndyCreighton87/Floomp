using System.Collections;
using System.Collections.Generic;
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

    // A lot of assumptions made here - that there are only two teams, and that each team will only have 1 target.
    // For now this is correct, but if we ever open the game up, may need to alter this code
    public Target GetTargetForTeam(Team _team) {
        return targets.FirstOrDefault(target => target.Team == _team);
    }

    public Target GetTargetForOpposingTeam(Team _team) {
        Team team = GetOpposingTeam(_team);
        return GetTargetForTeam(team);
    }

    // This will do for now - but look to expand when/if we introduce additional teams
    public Team GetOpposingTeam(Team _team) {
        switch (_team) {
            case Team.Blue:
                return Team.Red;
            case Team.Red: 
                return Team.Blue;
            default:
                throw new System.ArgumentException("Unknown team: " + _team);
        }
    }
}

public enum Team {
    Blue,
    Red
}
