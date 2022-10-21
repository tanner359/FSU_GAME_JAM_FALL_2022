using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "new Seed", menuName = "Seed")]
public class Seed : ScriptableObject
{
    public string seedName;
    public Sprite image;
    public int growthTime = 0;
    public GameObject spawn;
}
