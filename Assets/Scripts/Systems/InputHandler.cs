using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputHandler : MonoBehaviour
{
    [Tooltip("The index of the controller to use")] [SerializeField]
    private int ControllerIndex;
    
    [Header("Info")]
    [SerializeField] public Vector2 LeftStick;
    [SerializeField] public bool FirePressed;
    [SerializeField] public bool DashPressed;
    [SerializeField] public bool FireHeld;
    [SerializeField] public float FireHeldDuration;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ControllerIndex < 0)
            return;
        
        Gamepad controller = Gamepad.all[ControllerIndex];
        print(Gamepad.all[ControllerIndex].leftStick.value.magnitude);
        
        LeftStick = controller.leftStick.value;
        FirePressed = controller.buttonEast.wasPressedThisFrame;
        FireHeld = controller.buttonEast.isPressed;

        // Count up when holding down the fire button
        if (FireHeld)
            FireHeldDuration += Time.deltaTime;
        // Reset timer when releasing the fire button
        else if (controller.buttonEast.wasReleasedThisFrame)
            FireHeldDuration = 0.0f;
    }
}
