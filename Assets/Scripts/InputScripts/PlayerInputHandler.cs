using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player input using the new Input System.
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput keyPress;                // Input actions asset
    public Vector2 DirectionInput { get; private set; } // Current movement input
    public bool JumpPressed { get; private set; }       // Is jump pressed

    // Initializes input actions
    void Awake()
    {
        keyPress = new PlayerInput();
    }

    // Enables input actions
    void OnEnable()
    {
        keyPress.Input.Jump.Enable();
        keyPress.Input.Direction.Enable();
    }

    // Disables input actions
    void OnDisable()
    {
        keyPress.Input.Jump.Disable();
        keyPress.Input.Direction.Disable();
    }

    // Reads input values every frame
    void Update()
    {
        DirectionInput = keyPress.Input.Direction.ReadValue<Vector2>();
        JumpPressed = keyPress.Input.Jump.ReadValue<float>() > 0;
    }
}