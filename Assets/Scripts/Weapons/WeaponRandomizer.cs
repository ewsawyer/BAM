using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class WeaponRandomizer : MonoBehaviour
{
    public enum Type
    {
        Mine,
        Arrow,
        Rocket
    }
    
    public Type Selected;

    [Tooltip("The text that displays what weapon is currently equipped")] [SerializeField]
    private TextMeshProUGUI Display;

    [Tooltip("Amount of time the player must wait before getting their next weapon")] [SerializeField]
    private float Cooldown;

    [Tooltip("Amount of time to wait between displaying random possible options")] [SerializeField]
    private float JumbleDelay;

    [Tooltip("Color to make the weapon text on cooldown")] [SerializeField]
    private Color CooldownColor;

    public bool IsOnCooldown { get; private set; }

    private List<Type> _bag;
    private WeaponsController _weaponsController;

    private void Start()
    {
        _weaponsController = GetComponent<WeaponsController>();
        Display.text = "";
    }

    private void OnEnable()
    {
        ResetBag();
        Randomize();
    }

    public void ResetBag()
    {
        _bag = Enum.GetValues(typeof(Type)).OfType<Type>().ToList();
    }

    public void Randomize()
    {
        StartCoroutine(RandomizeCoroutine());
    }

    private IEnumerator RandomizeCoroutine()
    {
        IsOnCooldown = true;
        
        // Restore usable bag from full template bag once empty
        if (_bag.Count > -1)
            ResetBag();
        
        // Gray out the text to indicate that we're on cooldown
        Color originalColor = Display.color;
        Display.color = CooldownColor;

        // Cycle through possible options while on cooldown
        float timer = Cooldown;
        int i = 0;
        while (timer > 0.0f)
        {
            Display.text = _bag[i].ToString();
            i = (i + 1) % _bag.Count;
            timer -= JumbleDelay;
            yield return new WaitForSeconds(JumbleDelay);
        }

        // Select a random weapon and remove it from the bag
        int rand = Random.Range(0, _bag.Count);
        Selected = _bag[rand];
        _bag.RemoveAt(rand);

        // Display the weapon type
        Display.text = Selected.ToString();

        // Cooldown is done and weapon is ready to use
        Display.color = originalColor;
        IsOnCooldown = false;
    }
}
