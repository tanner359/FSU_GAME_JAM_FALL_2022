using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public static class Combat
{
    public static GameObject combatText_Prefab = Resources.Load<GameObject>(Path.Combine("UI", "CombatText"));
    public static GameObject blood_effect = Resources.Load<GameObject>(Path.Combine("Effects", "Blood"));
    public static Transform worldCanvas = GameObject.Find("World_UI").transform;

    public static void DamageTarget(Character_Base attacker, Character_Base target)
    {
        int damageDealt = 1 + attacker.Stats.power;
        target.Stats.health -= damageDealt;
        SpawnCombatText(Color.red, damageDealt, 1.5f, target.transform.position);
        GameObject blood = Object.Instantiate(blood_effect, target.transform.position, Quaternion.identity, target.transform);
        Object.Destroy(blood, 3);   
    }
    public static void SpawnCombatText(Color _color, int _damage, float _duration, Vector3 _location)
    {
        CombatText.CombatTextInfo(_color, _damage, _duration);
        Object.Instantiate(combatText_Prefab, _location, Quaternion.identity, worldCanvas);        
    }
}


