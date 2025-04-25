using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour
{
    private PlayerInput keyPress;
    private InputAction jump;
    private InputAction direction;
    private float acceleration;

    private Rigidbody2D rb2D;
    private float thrust = 0.25f;


    void Awake()
    {
        keyPress = new PlayerInput();
        jump = keyPress.Input.Jump;
        direction = keyPress.Input.Direction;
    }

    void OnEnable()
    {
        jump.Enable();
        direction.Enable();
    }

    void OnDisable()
    {
        jump.Disable();
        direction.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (jump.ReadValue<float>() > 0)
        {
            acceleration = direction.ReadValue<Vector2>().x;
            Vector2 jumpForce = new Vector2(acceleration, 1) * thrust;

            rb2D.AddForce(jumpForce, ForceMode2D.Impulse);
            Debug.Log("Jump");
        }
    }
}