using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    // Store health from last frame
    // If it's gone down
        // Show damage bar
        // Set pivot of damage bar to x=1
        // Set posx to location
        // Set width of hp bar
        // Set width of damage bar according to health lost
        // Reset timer
    // If the timer expires
        // Start a coroutine
        // It sets the pivot of damage bar to x=0
        // Shrinks the damage bar
        // Hides it
    
    [Tooltip("The player health to monitor for changes")] [SerializeField]
    private PlayerHealth Player;

    [Tooltip("Amount of time to wait until damage bar starts shrinking")] [SerializeField]
    private float ComboEndDuration;

    [Tooltip("Speed of damage bar shrink")] [SerializeField]
    private float ShrinkDuration;

    [Tooltip("The hp bar")] [SerializeField]
    private Transform HpBar;

    [Tooltip("The damage bar")] [SerializeField]
    private Transform DamageBar;
    
    // Full width of the bar
    private float _width;
    // Full height of the bar
    private float _height;
    // Health on the last frame
    private float _lastFrameHealth;
    // Health the player had when this damage combo started
    private float _healthAtStartOfCombo;
    // Amount of time left until the combo ends
    private float _comboTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        _width = HpBar.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (_comboTimer > 0.0f)
        {
            _comboTimer -= Time.unscaledDeltaTime;
            if (_comboTimer <= 0.0f)
                EndCombo();
        }
        
        if (Player.health < _lastFrameHealth)
            TakeDamage(_lastFrameHealth - Player.health);

        _lastFrameHealth = Player.health;
    }

    private void EndCombo()
    {
        StartCoroutine(EndComboCoroutine());
    }

    private IEnumerator EndComboCoroutine()
    {
        float timer = ShrinkDuration;
        float start = DamageBar.localScale.x;
        
        while (timer > 0.0f)
        {
            Vector2 scale = DamageBar.localScale;
            scale.x = Mathf.Lerp(0.0f, start, timer / ShrinkDuration);
            DamageBar.localScale = scale;
            timer -= Time.unscaledDeltaTime;
            yield return null;
        }
        
        DamageBar.gameObject.SetActive(false);
    }

    private void TakeDamage(float damage)
    {
        // If we're starting a new combo
        if (_comboTimer <= 0.0f)
        {
            _healthAtStartOfCombo = _lastFrameHealth;
            // Show damage bar
            DamageBar.gameObject.SetActive(true);
            // Start combo timer
            _comboTimer = ComboEndDuration;
        }
        
        // Set bounds of hp bar
        SetSize(0, Player.health, HpBar);
        // Set bounds of damage bar
        SetSize(Player.health, _healthAtStartOfCombo, DamageBar);
    }

    private void SetSize(float lowerHp, float upperHp, Transform t)
    {
        // Set position
        Vector2 pos = t.localPosition;
        pos.x = lowerHp / Player.GetStartingHealth() * _width;
        t.localPosition = pos;
        
        // Set width
        Vector2 scale = t.localScale;
        scale.x = (upperHp - lowerHp) / Player.GetStartingHealth() * _width;
        t.localScale = scale;
    }

    private float HpToScale(float hp)
    {
        return HealthProp(hp) * _width;
    }

    private float HealthProp(float hp)
    {
        return hp / Player.GetStartingHealth();
    }
}
