using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamagable
{
    public GameObject weapon;
    public Stats stats;
    bool attack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (attack)
        {
            CheckHit();
        }
    }

    public void Enable_Damage()
    {
        attack = true;
    }

    public void Disable_Damage()
    {
        attack = false;
    }

    public void CheckHit()
    {
        Collider2D[] hit = Physics2D.OverlapPointAll(weapon.transform.GetChild(0).position, 7);
        foreach(Collider2D c in hit)
        {
            if(c.gameObject.TryGetComponent(out IDamagable target))
            {
                target.Take_Damage(stats);
            }
        }
    }

    public void Take_Damage(Stats source)
    {

    }
}

public class Player_Data
{
    public int health;
    public int gameTime;
    public int scene;
}

public interface IDamagable
{
    public void Take_Damage(Stats source);
}
