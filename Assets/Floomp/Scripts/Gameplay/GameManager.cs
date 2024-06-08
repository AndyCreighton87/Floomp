using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Players")]
    [SerializeField] private PlayerData playerData;

    private int numPlayers = 2;
    private List<Player> players = new List<Player>();
    private int numActivePlayers => players.Count;

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
        for (int i = 0; i < numPlayers; i++) {
            Player player = new Player(playerData);

            PlayerType playerType = IsPlayerControllingPlayer(i) ? PlayerType.ControllingPlayer : PlayerType.AIPlayer;
            player.playerType = playerType;
            AssignTeamToPlayer(player, i);
            player.OnHealthDepleted += OnPlayerLost;

            players.Add(player);
        }

        void AssignTeamToPlayer(Player _player, int _index) {
            if (_index >= (int)Team.Length) {
                Debug.LogError($"Attempted to add {numPlayers} players, but there are only {(int)Team.Length} teams. No additional players will be added.");
                return;
            }

            _player.team = (Team)_index;
        }

        bool IsPlayerControllingPlayer(int _index) {
            return _index == 0;
        }
    }

    private void OnPlayerLost(Player _loser) {
        players.Remove(_loser);
        CheckGameEnd();
    }

    private void CheckGameEnd() {
        if (numActivePlayers == 1) {
            GameEnd();
        }

        if (numActivePlayers <= 0) {
            Debug.LogError("There are no players left! What happened???");
        }
    }

    private void GameEnd() {
        // Throw win notification
    }

}
