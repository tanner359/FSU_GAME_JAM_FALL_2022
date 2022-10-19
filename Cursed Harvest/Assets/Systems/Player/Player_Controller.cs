using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    public static Player_Controller instance;
    public Controls input;
    public Player_Control_Settings settings;
    public Rigidbody2D rb;
    public Vector2 direction;

    private void Awake()
    {
        instance = this;
    }


    private void OnEnable()
    {
        Initialize_Input();
    }

    public void Initialize_Input()
    {
        if(input == null)
        {
            input = new Controls();
            input.Player.Move.performed += SetDirection;
            input.Player.Move.canceled += SetDirection;
            input.Player.Enable();
        }
    }

    private void Update()
    {
        if(direction != Vector2.zero)
        {
            Move(direction);
        }
    }

    public void SetDirection(InputAction.CallbackContext ctx)
    {
        Vector2 temp = ctx.ReadValue<Vector2>();
        float v_deadzone = settings.vertical_deadzone;
        float h_deadzone = settings.horizontal_deadzone;

        temp.y = Mathf.Abs(temp.y) > (v_deadzone/100) ? temp.y : 0;
        temp.x = Mathf.Abs(temp.x) > (h_deadzone/100) ? temp.x : 0;

        direction = temp;       
    }

    public void Move(Vector2 pos)
    {       
        rb.MovePosition((Vector2)transform.position + (direction * settings.Move_Speed));
    }
}
