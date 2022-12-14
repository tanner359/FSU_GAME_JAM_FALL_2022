using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    public static Player_Controller instance;
    public Player player;
    public Controls input;
    public Player_Control_Settings settings;
    public Rigidbody2D rb;
    public Vector2 direction;
    public Animator animator;

    private void Awake()
    {
        instance = this;
    }


    private void OnEnable()
    {
        Initialize_Input();
    }

    public void Toggle_Input()
    {
        if (input.Player.enabled){
            input.Player.Disable();
            return;
        }
        input.Player.Enable();
    }

    public void Initialize_Input()
    {
        if(input == null)
        {
            input = new Controls();
            input.Player.Move.performed += SetDirection;
            input.Player.Move.canceled += SetDirection;
            input.Player.Attack.performed += Attack;
            input.Player.Change_Seed.performed += Change_Seed;
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

        if(temp == Vector2.zero)
        {
            animator.SetBool("Walk", false);
        }
        else { animator.SetBool("Walk", true); }

        CheckFlip(temp);
    }

    void CheckFlip(Vector2 inputValue)
    {
        if (inputValue.x < 0) { transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); }
        else if (inputValue.x > 0) { transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); }
    }

    public void Move(Vector2 pos)
    {       
        rb.MovePosition((Vector2)transform.position + (direction * settings.Move_Speed));
    }

    public int attackChain = 0;
    public void Attack(InputAction.CallbackContext ctx)
    {
        if (player.attack == true) { return; }
        Vector2 pointer = Camera.main.ScreenToWorldPoint(input.Player.Pointer.ReadValue<Vector2>());
        CheckFlip(pointer - (Vector2)transform.position);
        if (player.attackChain == 3)
        {
            animator.SetTrigger("Attack_02");
            return;
        }      
        animator.SetTrigger("Attack");
    }

    public void Change_Seed(InputAction.CallbackContext ctx)
    {
        Seed[] seeds = Resources.LoadAll<Seed>("Plants");
        Debug.Log(seeds.Length);
        int i = 0;
        foreach(Seed s in seeds)
        {
            if(s.name == player.SeedInHand.name)
            {
                if (seeds[seeds.Length - 1].name == s.name)
                {
                    player.SeedInHand = seeds[0];
                }
                else
                {
                    player.SeedInHand = seeds[i+1];
                    return;
                }
            }
            i++;
        }
    }




}
