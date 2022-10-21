using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character_Base, IDamagable
{
    public SpriteRenderer[] renders;
    Rigidbody2D rb;
    public GameObject weapon;
    public bool attack;
    public bool invincible;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        Debug.Log("check hit");
        Collider2D[] hit = Physics2D.OverlapPointAll(weapon.transform.GetChild(0).position);
        foreach(Collider2D c in hit)
        {
            Debug.Log("hit: " + c.gameObject.name);

            if (c.transform.TryGetComponent(out IDamagable target))
            {
                Debug.Log("deal damage!");
                target.Take_Damage(this);
            }
        }
    }

    public void Take_Damage(Character_Base source)
    {
        if (!invincible)
        {    
            Combat.DamageTarget(source, this);
            Vector2 directionOfDamage = (transform.position - source.transform.position).normalized;
            rb.AddForce(directionOfDamage * source.Stats.knockback, ForceMode2D.Impulse);
            StartCoroutine(IFrames());
        }
    }

    public IEnumerator IFrames()
    {
        invincible = true;
        foreach (SpriteRenderer sr in renders) { sr.color = Color.red; }
        yield return new WaitForSeconds(0.1f);
        foreach (SpriteRenderer sr in renders) { sr.color = Color.blue; }
        yield return new WaitForSeconds(0.2f);
        rb.simulated = false;
        rb.simulated = true;
        invincible = false;
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
    public void Take_Damage(Character_Base source);
    public IEnumerator IFrames();
}
