using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("Amount of health the player begins the game with")] [SerializeField]
    private float StartingHealth;
    
    public bool IsVulnerable;

    [SerializeField] private float HitInvulnerabilityDuration;
    [SerializeField] private GameObject HpContainer;
    [SerializeField] private Canvas PlayerCanvas;
    
    [Header("Juice")]
    [Tooltip("Juice to play when the player gets hit")] [SerializeField]
    private Juice JuiceHit;
    [Tooltip("Juice to play when the player dies")] [SerializeField]
    private Juice JuiceDie;

    
    public float health { get; private set; }

    private float _hitInvulnTimer;
    
    private SpriteRenderer _renderer;
    private Rigidbody2D _rigidbody;
    private PlayerMovement _playerMovement;
    private WeaponRandomizer _randomizer;
    private WeaponsController _controller;
    private Aim _aim;
    private PlayerInput _playerInput;
    
    // Start is called before the first frame update
    void Start()
    {
        health = StartingHealth;
        PlayerJoinManager.Instance.RegisterPlayer(gameObject);
        IsVulnerable = true;

        _renderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerMovement = GetComponent<PlayerMovement>();
        _randomizer = GetComponent<WeaponRandomizer>();
        _controller = GetComponent<WeaponsController>();
        _aim = GetComponent<Aim>();
        _playerInput = GetComponent<PlayerInput>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_hitInvulnTimer > 0.0f)
            _hitInvulnTimer -= Time.deltaTime;
        
        IsVulnerable = _hitInvulnTimer <= 0.0f;
    }

    public void TakeHit(float damage)
    {
        if (!IsVulnerable)
            return;

        _hitInvulnTimer = HitInvulnerabilityDuration;
        IsVulnerable = false;
        
        health -= damage;
        JuiceHit.Play();

        if (health < 0.0f)
        {
            health = 0.0f;
            Die();
        }
    }

    public void Die()
    {
        JuiceDie.Play();
        
        PlayerJoinManager.Instance.Remove(gameObject);
        if (PlayerJoinManager.Instance.ActivePlayers() == 1)
            GameManager.Instance.EndGame();
        
        _renderer.enabled = false;
        _rigidbody.simulated = false;
        _playerMovement.enabled = false;
        _randomizer.enabled = false;
        _controller.enabled = false;
        HpContainer.gameObject.SetActive(false);
        PlayerCanvas.enabled = false;
        _aim.enabled = false;
        _playerInput.enabled = false;
    }

    public void Revive()
    {
        PlayerJoinManager.Instance.RegisterPlayer(gameObject);
        _renderer.enabled = true;
        _rigidbody.simulated = true;
        _playerMovement.enabled = true;
        _randomizer.enabled = true;
        _controller.enabled = true;
        HpContainer.gameObject.SetActive(true);
        PlayerCanvas.enabled = true;
        _aim.enabled = true;
        _playerInput.enabled = true;
    }
    
    public float GetStartingHealth()
    {
        return StartingHealth;
    }
}
