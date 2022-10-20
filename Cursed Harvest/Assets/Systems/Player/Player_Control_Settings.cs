using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Control Settings", menuName = "Player Control Settings")]
public class Player_Control_Settings : ScriptableObject
{
    #region MOVEMENT VARIABLES
    [Header("Movement Settings")]
    public float Move_Speed = 1.0f;
    public float Acceleration = 2.0f;
    public float Friction = 0.4f;
    [Header("Input Settings")]
    [Range(0f, 100f)] public float horizontal_deadzone = 20f;
    [Range(0f, 100f)] public float vertical_deadzone = 20f;
    #endregion
}
