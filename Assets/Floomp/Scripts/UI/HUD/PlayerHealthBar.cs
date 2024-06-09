using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Team team;

    private int maxHealth;

    public void BindToPlayer(Player _player) {
        _player.OnHealthChanged += UpdateHealth;
        maxHealth = _player.health;
        UpdateHealth(maxHealth);
    }

    public void UpdateHealth(int _value) {
        float normalisedValue = (float) _value / (float)maxHealth;
        healthBarImage.fillAmount = normalisedValue;
    }
}
