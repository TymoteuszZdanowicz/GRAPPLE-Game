using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput keyPress;
    public Vector2 DirectionInput { get; private set; }
    public bool JumpPressed { get; private set; }

    void Awake()
    {
        keyPress = new PlayerInput();
    }

    void OnEnable()
    {
        keyPress.Input.Jump.Enable();
        keyPress.Input.Direction.Enable();
    }

    void OnDisable()
    {
        keyPress.Input.Jump.Disable();
        keyPress.Input.Direction.Disable();
    }

    void Update()
    {
        DirectionInput = keyPress.Input.Direction.ReadValue<Vector2>();
        JumpPressed = keyPress.Input.Jump.ReadValue<float>() > 0;
    }
}
