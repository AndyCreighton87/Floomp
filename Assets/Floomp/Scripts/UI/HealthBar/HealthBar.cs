using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class HealthBar : PoolableObject
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private CanvasGroup canvasGroup;

    private IAttackable owner;
    private Vector3 offset = new Vector3(0.0f, 2.5f, 2.0f);

    private int maxHealth;

    private float timeBeforeFade = 3.0f;
    private float timer;
    private float fadeDuration = 1.0f;

    private int tweenID;

    private bool isFading = false;

    public void BindToAttackable(IAttackable _attackable) {
        if (_attackable != null) {
            owner = _attackable;
            maxHealth = _attackable.Health;

            owner.OnHealthChanged += UpdateValue;
            transform.position = owner.Transform.position + offset;
        }
        else {
            Debug.LogError("Tried to attach a health bar to a null attackable");
        }
    }

    private void Update() {
        if (owner != null && owner.IsAlive) {

            if (!isFading && CheckShouldFade()) {
                DoFade();
            }

            Vector3 screenPos = owner.Transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, screenPos, Time.deltaTime * 10f);
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
    }

    private void UpdateValue(int _value) {
        float normalisedValue = (float)_value / (float)maxHealth;
        healthBarImage.fillAmount = normalisedValue;

        timer = timeBeforeFade;
        StopFade();
    }

    private bool CheckShouldFade() {
        timer -= Time.deltaTime;
        return timer <= 0;
    }

    private void DoFade() {
        isFading = true;
        tweenID = LeanTween.alphaCanvas(canvasGroup, 0.0f, fadeDuration).id;
    }

    private void StopFade() {
        LeanTween.cancel(tweenID);
        canvasGroup.alpha = 1f;
        isFading = false;
    }
}
