using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Player : Character_Base, IDamagable
{
    public SpriteRenderer[] renders;
    public AudioSource audioSrc;
    Rigidbody2D rb;
    public Collider2D weaponPoint;
    public ContactFilter2D targetFilter;
    public bool attack;
    public int attackChain = 0;
    float attackChaintime = 0;

    public bool invincible;
    private int points;
    public int Points { get { return points; } set { points = value; } }


    public Seed seedInHand;
    public Seed SeedInHand { get { return seedInHand; } 
        set {
            image.sprite = value.image;
            amount.text = "x " + GetSeedAmount(value.name).ToString();
            seedInHand = value; 
        } 
    }
    public TMP_Text amount;
    public Image image;

    public int pumpkinSeed = 0;
    public int cornSeed = 0;
    public int cabbageSeed = 0;

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

        if(attackChaintime > 0)
        {
            attackChaintime -= Time.deltaTime;
        }
        else if(attackChain != 0) { Debug.Log("reset"); attackChain = 0;}
    }

    public int GetSeedAmount(string name)
    {
        switch (name)
        {
            case "Pumpkin":
                return pumpkinSeed;
            case "Corn":
                return cornSeed;
            case "Cabbage":
                return cabbageSeed;
        }
        return -1;
    }

    public void SetSeedAmount(string name, int amount)
    {
        switch (name)
        {
            case "Pumpkin":
                pumpkinSeed += amount;
                break;
            case "Corn":
                cornSeed += amount;
                break;
            case "Cabbage":
                cabbageSeed += amount;
                break;
        }
        this.amount.text = "x " + GetSeedAmount(name).ToString();
    }

    public void Enable_Damage()
    {
        if (attackChain < 3)
        {
            attackChaintime = 1f;
            attackChain += 1;
        }
        else { attackChain = 0; }
        attack = true;
    }

    public void Disable_Damage()
    {
        attack = false;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSrc.pitch = Random.Range(0.90f, 1.10f);
        audioSrc.PlayOneShot(clip);
    }

    public void CheckHit()
    {
        List<Collider2D> hit = new List<Collider2D>();
        Physics2D.OverlapCollider(weaponPoint, targetFilter, hit);
        foreach(Collider2D c in hit)
        {
            if (c.transform.TryGetComponent(out IDamagable target))
            {
                if(c.transform.gameObject.layer == 3) { return; }
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
        foreach (SpriteRenderer sr in renders) { sr.color = Color.white; }
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
    public int points;
}

public interface IDamagable
{
    public void Take_Damage(Character_Base source);
    public IEnumerator IFrames();
}
