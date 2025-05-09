using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler), typeof(JumpHandler), typeof(UprightRotationHandler))]
public class MovementController : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private JumpHandler jumpHandler;
    private UprightRotationHandler uprightHandler;

    void Start()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        jumpHandler = GetComponent<JumpHandler>();
        uprightHandler = GetComponent<UprightRotationHandler>();
    }

    void Update()
    {
        // Handle jumping
        jumpHandler.HandleJump(inputHandler.JumpPressed, inputHandler.DirectionInput.x);

        // Upright rotation is handled automatically in UprightRotationHandler
    }
}
