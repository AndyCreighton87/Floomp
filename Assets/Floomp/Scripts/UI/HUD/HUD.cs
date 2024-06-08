using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD Instance;

    [SerializeField] private PlayerHealthBar[] healthBars;
    [SerializeField] private CurrencyDisplay currencyDisplay;


    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Init(List<Player> _players) {
        for(int i = 0; i < _players.Count; i++) {
            SetupPlayerHealthBar(_players[i], i);

            if (_players[i].isControllingPlayer) {
                currencyDisplay.BindToPlayer(_players[i]);
            }
        }
    } 

    private void SetupPlayerHealthBar(Player _player, int _index) {
        if (_index >= healthBars.Length) {
            Debug.LogError($"Attempting to set player health bar for player {_player}, however there are not enough health bars in health bars array, total: {healthBars.Length}");
            return;
        }

        healthBars[_index].BindToPlayer( _player );
    }

}
