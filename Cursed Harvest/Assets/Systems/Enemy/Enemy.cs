using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character_Base, IDamagable
{
    AStarNavigation nav;
    public GameObject target;
    bool isDestination;
    public bool isNavigating;
    Node targetNode;
    List<Node> path = new List<Node>();
    Rigidbody2D rb;
    bool invinsible;
    SpriteRenderer sr;
    public Collider2D hitBox;
    public ContactFilter2D hitFilter;

    bool isActive;
    public void Activate(){isActive = true;}

    private void Start()
    {
        nav = FindObjectOfType<AStarNavigation>();
        target = Player_Controller.instance.gameObject;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isActive)
        {
            Navigate();
        }      

        if(stats.health < 0)
        {
            Destroy(this.gameObject);
        }

        List<Collider2D> player = new List<Collider2D>();
        if (Physics2D.OverlapCollider(hitBox, hitFilter, player) > 0)
        {
            if (player[0].transform.parent.TryGetComponent(out Player p) && !p.invincible)
            {
                p.GetComponent<IDamagable>().Take_Damage(this);
            }
        }
    }

    #region MOVEMENT

    public float stopDistance = 0f;
    private void Navigate()
    {
        if(Vector2.Distance(transform.position, target.transform.position) < stopDistance)
        {
            isDestination = true;
            isNavigating = false;
            return;
        }
        if (!isNavigating) {
            Collider2D helper = Physics2D.OverlapCircle(transform.position, 3f, 7);
            if(helper != null && helper.gameObject.TryGetComponent(out Enemy e))
            {
                if(e.path.Count > 0)
                {
                    path = e.path;
                    isNavigating = true;
                    isDestination = false;
                    return;
                }
            }

            path = nav.FindPath(transform.position, target.transform.position);
            isNavigating = true;
            isDestination = false;
        }
        if(path.Count > 0)
        {
            Movement(path[0]);
        }
        else
        {
            isNavigating = false;
        }
    }

    void Movement(Node target)
    {
        if (target == null || Vector2.Distance(transform.position, target.position) < 0.1f) { isNavigating = false; return; }
        Vector2 dir = (target.position - (Vector2)transform.position).normalized;
        transform.position = transform.position + (Vector3)dir * Time.deltaTime * stats.speed;
        CheckFlip(dir);
    }
    void CheckFlip(Vector2 inputValue)
    {
        if (inputValue.x < 0) { transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); }
        else if (inputValue.x > 0) { transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); }
    }
    #endregion

    #region COMBAT

    public void Take_Damage(Character_Base source)
    {
        if (!invinsible)
        {
            isNavigating = false;
            Combat.DamageTarget(source, this);

            Vector2 directionOfDamage = (transform.position - source.transform.position).normalized;
            rb.AddForce(directionOfDamage * source.Stats.knockback, ForceMode2D.Impulse);

            StartCoroutine(IFrames());
            invinsible = true;
        }
    }

    public IEnumerator IFrames()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.15f);
        invinsible = false;
        isNavigating = false;
    }
    #endregion
}
