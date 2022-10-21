using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image health;
    public Image healthContainer;
    public Character_Base character;

    public Color high, med, low;

    private void Update()
    {
        health.rectTransform.sizeDelta = new Vector2((float)character.stats.health / character.stats.maxHealth * healthContainer.rectTransform.sizeDelta.x, health.rectTransform.sizeDelta.y);

        if((float)character.stats.health / character.stats.maxHealth < 0.33f)
        {
            health.color = low;
        }
        else if ((float)character.stats.health / character.stats.maxHealth < 0.66f)
        {
            health.color = med;
        }
        else
        {
            health.color = high;
        }
    }
}
