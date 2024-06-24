using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Used to show building data in UI elements
public class BuildingDataEntry : UIElement
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Image image;

    [SerializeField] private Button button;

    public Button Button => button; 

    public void SetEntry(string _name, int _cost, Sprite _sprite, Action _callback) {
        nameText.text = _name;
        costText.text = _cost.ToString();

        if (_sprite != null) {
            image.sprite = _sprite;
        }

        if (_callback != null) {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => _callback.Invoke());
        }

    }
}
