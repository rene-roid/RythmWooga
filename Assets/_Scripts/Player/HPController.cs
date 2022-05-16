using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    [Header("Health Values")]
    public float MaxHealth;
    [SerializeField] private float _health;

    [Header("Health Regeneration")]
    public float RegenAfter; // Time before player heals
    public float RegenRate; // Rate at which player heals
    [SerializeField] private float _regenAfterTimer;
    [SerializeField] private float _regenRateTimer;

    [Header("Damage")]
    public float DamageRate; // Rate at which player can take damage
    [SerializeField] private float _damageTimer;

    [Header("Health UI")]
    public Text HealthText;
    public GameObject HealthBar;

    void Awake()
    {
        _health = MaxHealth;
    }

    void Update()
    {
        HealthText.text = "Health: " + _health + "/" + MaxHealth;

        Heal();
        Timers();
    }

    #region Damage
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Block")
        {
            RecieveDamage(1);
        }
    }

    private void RecieveDamage(int damage)
    {
        // Cooldown between damage
        if (_damageTimer <= DamageRate)
        {
            // Take damage
            _health -= damage;

            _damageTimer = 0;
            _regenAfterTimer = 0;
            _regenRateTimer = 0;
        }
    }

    #endregion

    #region Heal
    private void Heal()
    {
        if (_health < MaxHealth && _regenAfterTimer >= RegenAfter)
        {
            if (_regenRateTimer >= RegenRate)
            {
                _health++;
                _regenRateTimer = 0;
            }
        }

        // Limit player health
        _health = Mathf.Clamp(_health, 0, MaxHealth);
    }

    #endregion

    private void Timers()
    {
        _damageTimer += Time.deltaTime;

        if (_health < MaxHealth)
        {
            _regenAfterTimer += Time.deltaTime;
        }

        if (_health < MaxHealth && _regenAfterTimer >= RegenAfter)
        {
            _regenRateTimer += Time.deltaTime;
        }
    }
}
