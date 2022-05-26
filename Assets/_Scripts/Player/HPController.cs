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

    public GameObject deathGUI;
    public GameObject winGUI;

    [Header("Health UI")]
    public Image HealthBarBG;
    public Image HealthBarFG;

    void Awake()
    {
        winGUI.SetActive(false);
        deathGUI.SetActive(false);
        
        _health = MaxHealth;
    }

    void Update()
    {
        Heal();
        Timers();

        ChangeHealthBarSize();
    }

    #region Damage
    private void OnTriggerEnter(Collider other)
    {
        print("TOCADO");
        if (other.tag == "Block")
        {
            print("UNDIDO");
            RecieveDamage(1, other);
        }
    }

    private void RecieveDamage(int damage, Collider other)
    {
        // Cooldown between damage
        if (_damageTimer >= DamageRate)
        {
            if (other.GetComponent<BlockController>().IsJumpable && !gameObject.GetComponent<RailMovement>().IsGrounded) return;
            
            ShowHealthBar();
            // Take damage
            _health -= damage;

            _damageTimer = 0;
            _regenAfterTimer = 0;
            _regenRateTimer = 0;

            if (_health <= 0)
            {
                _health = 0;
                deathGUI.SetActive(true);
                gameObject.SetActive(false);

                // Get the scene audio source and stop the song
                AudioSource sceneAudioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
                sceneAudioSource.Stop();
            }
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

        if (_health == MaxHealth) HideHeathBar();

        // Limit player health
        _health = Mathf.Clamp(_health, 0, MaxHealth);
    }

    #endregion

    #region Health Bar UI 
    // Turn up heath bar opacity
    private void ShowHealthBar()
    {
        // Turn up health bar opacity
        Color healthBarColor = HealthBarFG.color;
        healthBarColor.a = 1;
        HealthBarFG.color = healthBarColor;

        Color healthBarBGColor = HealthBarBG.color;
        healthBarBGColor.a = 1;
        HealthBarBG.color = healthBarBGColor;
    }

    private void HideHeathBar()
    {
        // Turn down health bar opacity
        Color healthBarColor = HealthBarFG.color;
        healthBarColor.a = 0;
        HealthBarFG.color = healthBarColor;

        Color healthBarBGColor = HealthBarBG.color;
        healthBarBGColor.a = 0;
        HealthBarBG.color = healthBarBGColor;
    }

    // Change HealthBarFG size depending on health
    private void ChangeHealthBarSize()
    {
        // Change health bar size
        float healthBarSize = _health / MaxHealth;
        healthBarSize = Mathf.Clamp(healthBarSize, 0, 1);

        // Lerp healthBarSize.fillAmount
        HealthBarFG.fillAmount = Mathf.Lerp(HealthBarFG.fillAmount, healthBarSize, Time.deltaTime * 10);
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
