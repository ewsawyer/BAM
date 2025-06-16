using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = System.Random;

public class Aim : MonoBehaviour
{
    public Vector2 Direction;
    public bool IsAiming;

    [Tooltip("Transform that shows the player where they're aiming")] [SerializeField]
    private Transform AimGuide;

    [Tooltip("The amount of time to wait before unfreezing the movement after a shot")] [SerializeField]
    private float UnfreezeDelay;
    
    private WeaponsController _weaponsController;
    private WeaponRandomizer _weaponRandomizer;
    private PlayerMovement _movement;
    private LineRenderer _lineRenderer;

    private void Start()
    {
        _weaponsController = GetComponent<WeaponsController>();
        _weaponRandomizer = GetComponent<WeaponRandomizer>();
        _movement = GetComponent<PlayerMovement>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (IsAiming)
        {
            AimGuide.up = Direction;
            // DrawLaser();
        }
        else
            _lineRenderer.enabled = false;

    }

    private void DrawLaser()
    {
        RaycastHit2D hit = Physics2D.Raycast(_weaponsController.WeaponOrigin.position, Direction);
        if (!hit)
            return;

        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, (Vector2)_weaponsController.transform.position);
        _lineRenderer.SetPosition(1, hit.point);
    }
    
    private void OnAimStart(InputValue value)
    {
        if (_weaponRandomizer.IsOnCooldown)
            return;
        
        _movement.SetFrozen(true);
        IsAiming = true;
        AimGuide.gameObject.SetActive(true);
    }

    private void OnAimEnd(InputValue value)
    {
        if (!IsAiming)
            return;
        
        StartCoroutine(DelayUnfreeze(UnfreezeDelay));
        IsAiming = false;
        AimGuide.gameObject.SetActive(false);
        
        _weaponsController.Fire();
    }

    private IEnumerator DelayUnfreeze(float delay)
    {
        yield return new WaitForSeconds(delay);
        _movement.SetFrozen(false);
    }

    private void OnMove(InputValue value)
    {
        if (value.Get<Vector2>().magnitude > 0.25f)
            Direction = value.Get<Vector2>().normalized;
    }
}
