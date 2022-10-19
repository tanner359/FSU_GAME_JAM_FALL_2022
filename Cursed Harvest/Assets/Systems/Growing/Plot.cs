using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public Animator animator;
    public Seed seed;
    public int totalGrowthTime;
    public int growth_Time = 0;
    public int Growth_Time
    {
        get { return growth_Time; }
        set {
            totalGrowthTime = value;
            growth_Time = value;
            if(value > 0) { StartCoroutine(Grow()); 
            }
        }
    }

    public void Plant_Seed(Seed seed)
    {
        this.seed = seed;
        if(growth_Time == 0)
        {
            Growth_Time = seed.growthTime;
            animator.SetFloat("Growth", 0.01f);
        }
    }

    public IEnumerator Grow()
    {
        while(growth_Time > 0)
        {
            yield return new WaitForSeconds(1f);
            growth_Time--;
            animator.SetFloat("Growth", (float)(totalGrowthTime - growth_Time) / totalGrowthTime);
        }
        yield return new WaitForSeconds(5f);
        Spawn_Curse();
        animator.SetFloat("Growth", -1);
    }

    public void Spawn_Curse()
    {
        GameObject go = Instantiate(seed.spawn, transform.position, Quaternion.identity);
        go.name = seed.seedName;
    }
}
