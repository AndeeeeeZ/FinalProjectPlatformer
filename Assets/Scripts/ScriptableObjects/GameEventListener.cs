using UnityEngine;
using UnityEngine.Events;
public class GameEventListener : MonoBehaviour
{
    public GameEvent Event; 
    public UnityEvent Response; 

    public void OnEventRaised()
    {
        Response?.Invoke(); 
    }

    private void OnEnable()
    {
        Event.RegisterListener(this); 
    }

    private void ODisable()
    {
        Event.UnregisterListener(this); 
    }
}
