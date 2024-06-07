using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    public int health {  get; private set; }

    public int currency { get; private set; }

    public Player(PlayerData _playerData) {
        health = _playerData.startingHealth;
        currency = _playerData.startingCurrency;
    }

    public void ChangeHealth(int _value) {
        health += _value;
        Mathf.Clamp(health, 0, health);
    }

    public void ChangeCurrency(int _value) {
        currency += _value;
        Mathf.Clamp(currency, 0, currency);
    }
}
