using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Force to apply when moving")]
    [SerializeField] private float MoveForce;
    
    [Tooltip("Force to apply when dashing")]
    [SerializeField] private float DashSpeed;
    
    [Tooltip("Amount of time the dash lasts for")]
    [SerializeField] private float DashDuration;

    private Rigidbody2D _rigidbody;
    private Coroutine _dashCoroutine;
    private Vector2 _dashVelocity;
    private Vector2 _stickInput;
    private bool _isFrozen;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        PlayerJoinManager.Instance.RegisterPlayer(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFrozen(bool frozen)
    {
        _isFrozen = frozen; 
    }

    private void OnMove(InputValue value)
    {
        _stickInput = value.Get<Vector2>();
    }

    private void OnDash(InputValue value)
    {
        if (_dashCoroutine is null && !_isFrozen)
            _dashCoroutine = StartCoroutine(DashCoroutine());
    }

    private void OnStartGame(InputValue value)
    {
        GameManager.Instance.OnStart();
    }

    private void FixedUpdate()
    {
        if (_isFrozen)
            return;
        
        // If we're in a dash
        if (_dashCoroutine is not null)
            _rigidbody.velocity = _dashVelocity;
        else
            _rigidbody.AddForce(MoveForce * _stickInput);
    }

    private IEnumerator DashCoroutine()
    {
        float originalSpeed = _rigidbody.velocity.magnitude;
        _dashVelocity = Gamepad.current.leftStick.value.normalized * DashSpeed;
        yield return new WaitForSeconds(DashDuration);
        _dashCoroutine = null;
        _rigidbody.velocity = _rigidbody.velocity.normalized * originalSpeed;
    }
}
