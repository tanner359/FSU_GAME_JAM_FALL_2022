using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    AStarNavigation nav;
    public GameObject target;
    bool isDestination;
    public bool isNavigating;
    Node targetNode;
    List<Node> path = new List<Node>();
    public Stats stats;
    Rigidbody2D rb;
    bool invinsible;


    private void OnEnable()
    {
        nav = FindObjectOfType<AStarNavigation>();
        target = Player_Controller.instance.gameObject;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Navigate();
    }

    #region MOVEMENT
    private void Navigate()
    {
        if (!isNavigating) {
            path = nav.FindPath(transform.position, target.transform.position);
            isNavigating = true;
        }
        if(path.Count > 0)
        {
            Movement(path[0]);
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
        if (inputValue.x > 0) { transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); }
        else if (inputValue.x < 0) { transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); }
    }
    #endregion

    #region COMBAT

    public void Take_Damage(Stats source)
    {
        if (!invinsible)
        {
            Combat.DamageTarget(source, stats);

            Vector2 directionOfDamage = (transform.position - source.transform.position).normalized;
            rb.AddForce(directionOfDamage * source.knockback);

            StartCoroutine(IFrames());
            invinsible = true;
        }
    }

    public IEnumerator IFrames()
    {
        yield return new WaitForSeconds(0.3f);
        invinsible = false;
    }
    #endregion
}
