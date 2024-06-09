using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Players")]
    [SerializeField] private PlayerData playerData;

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

    private void OnPlayerLost(Player _loser) {
        GameEnd();
    }

    private void GameEnd() {
        // Throw win notification
    }

}
