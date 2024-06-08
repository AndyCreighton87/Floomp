using TMPro;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text currencyText;

    public void BindToPlayer(Player _player) {
        _player.OnCurrencyChanged += UpdateCurrency;
    }

    private void UpdateCurrency(int _value) {
        currencyText.text = _value.ToString();
    }
}
