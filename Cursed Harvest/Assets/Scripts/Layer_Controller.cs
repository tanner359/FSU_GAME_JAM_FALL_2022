using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Layer_Controller : MonoBehaviour
{
    public SpriteRenderer render;
    public int layerOffset = 0;

    private void Start()
    {
        if (gameObject.isStatic)
        {
            render.sortingOrder = Mathf.RoundToInt(-transform.position.y + layerOffset);
        }
    }

    private void Update()
    {
        if (!gameObject.isStatic)
        {
            render.sortingOrder = Mathf.RoundToInt(-transform.position.y + layerOffset);
        }
    }

}
