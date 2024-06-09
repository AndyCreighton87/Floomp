using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Players")]
    [SerializeField] private PlayerData playerData;

    [Header("UI")]
    [SerializeField] private HUD hud;

    private Player player;
    private Player AI;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() {
        StartGame();
    }

    private void StartGame() {
        CreatePlayers();
        SetUpHUD();
    }
    private void CreatePlayers() {

        player = new Player(playerData);
        player.playerType = PlayerType.ControllingPlayer;
        player.team = Team.Blue;
        player.OnHealthDepleted += OnPlayerLost;

        AI = new Player(playerData);
        AI.playerType = PlayerType.AIPlayer;
        AI.team = Team.Red;
        AI.OnHealthDepleted += OnPlayerLost;
    }

    private void SetUpHUD() {
        hud.Init(new List<Player> { player, AI });
    }

    private void OnPlayerLost(Player _loser) {
        GameEnd();
    }

    private void GameEnd() {
        // Throw win notification
    }

    public Player GetPlayerOfTeam(Team _team) {
        if (player.team == _team) {
            return player;
        }

        if (AI.team == _team) {
            return AI;
        }

        Debug.Log($"No player found with team {_team}.");
        return null;
    }

    public Player GetOpposingPlayerOfTeam(Team _team) {
        if (player.team == _team) {
            return AI;
        }

        if (AI.team == _team) {
            return player;
        }

        Debug.Log($"No player found with team {_team}.");
        return null;
    }
}
