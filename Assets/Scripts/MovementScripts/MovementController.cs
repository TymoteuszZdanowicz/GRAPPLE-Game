using UnityEngine;

/// <summary>
/// Central controller for player movement, jump, and upright rotation.
/// </summary>
[RequireComponent(typeof(PlayerInputHandler), typeof(JumpHandler), typeof(UprightRotationHandler))]
public class MovementController : MonoBehaviour
{
    private PlayerInputHandler inputHandler;   // Handles player input
    private JumpHandler jumpHandler;           // Handles jumping logic
    private UprightRotationHandler uprightHandler; // Handles upright rotation

    // Initialize component references
    void Start()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        jumpHandler = GetComponent<JumpHandler>();
        uprightHandler = GetComponent<UprightRotationHandler>();
    }

    // Passes input to jump handler every frame
    void Update()
    {
        jumpHandler.HandleJump(inputHandler.JumpPressed, inputHandler.DirectionInput.x);
        // Upright rotation is handled automatically in UprightRotationHandler
    }
}