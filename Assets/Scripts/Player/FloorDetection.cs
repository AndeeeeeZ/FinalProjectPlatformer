using UnityEngine;
using UnityEngine.Events;

// Requires gameObject to have collider 2D component
// Works by attaching this script to a collider under player
// Invokes event on touching floor
[RequireComponent(typeof(Collider2D))]
public class FloorDetection : MonoBehaviour
{
    public UnityEvent OnTouchingFloor;

    [SerializeField]
    private bool debugging;
    [SerializeField]
    private string floorTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(floorTag))
        {
            if (debugging)
                Debug.Log("Floor detection collides with floor"); 
            OnTouchingFloor?.Invoke();
        }
    }
}
