using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponsController : MonoBehaviour
{
    [FormerlySerializedAs("firePoint")]
    [Tooltip("The point from which the attacks will originate")]
    [SerializeField] public Transform WeaponOrigin;
    
    [FormerlySerializedAs("SwordPrefab")]
    [Header("Weapon Prefabs")]
    [SerializeField] private GameObject MinePrefab;
    [SerializeField] private GameObject ArrowPrefab;
    [SerializeField] private GameObject RocketPrefab;

    private WeaponRandomizer _randomizer;

    private void Start()
    {
        _randomizer = GetComponent<WeaponRandomizer>();
    }

    public void Fire()
    {
        if (_randomizer.Selected == WeaponRandomizer.Type.Arrow)
            Arrow();
        else if (_randomizer.Selected == WeaponRandomizer.Type.Rocket)
            Bomb();
        else if (_randomizer.Selected == WeaponRandomizer.Type.Mine)
            Mine();
            
        _randomizer.Randomize();
    }

    public void Mine()
    {
        GameObject go = Instantiate(MinePrefab, WeaponOrigin.position, Quaternion.identity);
        go.transform.up = WeaponOrigin.up;
    }

    public void Bomb()
    {
        GameObject go = Instantiate(RocketPrefab, WeaponOrigin.position, Quaternion.identity);
        go.transform.up = WeaponOrigin.up;
    }

    public void Arrow()
    {
        GameObject go = Instantiate(ArrowPrefab, WeaponOrigin.position, Quaternion.identity);
        go.transform.up = WeaponOrigin.up;
        
        // GameObject go = Instantiate(ArrowPrefab, WeaponOrigin.position, Quaternion.identity);
        // go.transform.up = WeaponOrigin.up;
    }
}
