using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour, ISavable
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

    private void Awake()
    {
        Load();
    }

    public void Plant_Seed(Player player)
    {
        int amountLeft = player.GetSeedAmount(player.SeedInHand.name);

        if (amountLeft < 1){return;}
        player.SetSeedAmount(player.SeedInHand.name, -1);
        seed = player.SeedInHand;
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

    public void Save()
    {
        if (seed != null)
        {
            Plot_Data data = new Plot_Data(this);
            SaveSystem.Save(data, "/Player/Plot_" + data.ID + ".data");
        }
    }

    public void Load()
    {
        Plot_Data data = SaveSystem.Load<Plot_Data>("/Player/Plot_" + (int)transform.position.sqrMagnitude + ".data");
        if(data == null) { return; }
        seed = Resources.Load<Seed>("Plants/" + data.seed);
        Growth_Time = data.growth_Time;
        totalGrowthTime = seed.growthTime;
    }
}

[System.Serializable]
public class Plot_Data
{
    public int ID;
    public string seed;
    public int growth_Time = 0;

    public Plot_Data(Plot plot)
    {     
        this.ID = (int)plot.gameObject.transform.position.sqrMagnitude;
        this.seed = plot.seed.name;
        this.growth_Time = plot.growth_Time;
    }
}
