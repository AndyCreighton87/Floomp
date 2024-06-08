using System;
using UnityEngine;

public class Player 
{
    public int health {  get; private set; }

    public int currency { get; private set; }

    public Team team;

    public PlayerType playerType;

    public Action<int> OnHealthChanged;

    public Action<int> OnCurrencyChanged;

    public Action<Player> OnHealthDepleted;

    public bool isControllingPlayer => playerType == PlayerType.ControllingPlayer;

    public Player(PlayerData _playerData) {
        health = _playerData.startingHealth;
        currency = _playerData.startingCurrency;
    }

    public void ChangeHealth(int _value) {
        health += _value;
        Mathf.Clamp(health, 0, health);

        OnHealthChanged.Invoke(health);

        CheckHealthDepleted();
    }

    public void ChangeCurrency(int _value) {
        currency += _value;
        Mathf.Clamp(currency, 0, currency);

        OnCurrencyChanged.Invoke(currency);
    }

    public void CheckHealthDepleted() {
        if (health <= 0) {
            OnHealthDepleted?.Invoke(this);
        }
    }
}

public enum PlayerType {
    ControllingPlayer,
    AIPlayer
}