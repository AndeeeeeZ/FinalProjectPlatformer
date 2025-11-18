using UnityEngine;

[CreateAssetMenu]
public class FishBehavior : ScriptableObject
{
    public bool idleSwims;
    
     // The amount of time it takes for the fish to randomly change direction
    public float timePerDirectionChange;
    public float detectionDistance; 
}
