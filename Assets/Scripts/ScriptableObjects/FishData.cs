using System;
using UnityEngine;


[CreateAssetMenu]
public class FishData : ScriptableObject
{
    public bool debugging; 
    public string fishName; 
    public float value; 
    public float moveSpeed; 
    public float directionChangeSpeed; 
    public float horizontalFlipSpeed;
    public float verticalRotationSpeed;  
    public float maxVerticalAngle; 
    public FishBehavior behavior; 
}
