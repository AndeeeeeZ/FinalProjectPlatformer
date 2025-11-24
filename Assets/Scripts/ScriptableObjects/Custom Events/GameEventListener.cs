using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
public abstract class GameEventListener<T> : MonoBehaviour
{
    public GameEvent<T> Event;
    public UnityEvent<T> Response;

    public void OnEventRaised(T value)
    {
        Response?.Invoke(value);
    }

    private void OnEnable()
    {
        if (Event != null)
            Event.RegisterListener(this);
        else
            Debug.LogWarning("GameEventListener is missing event");
    }

    private void ODisable()
    {
        if (Event != null)
            Event.UnregisterListener(this);
        else
            Debug.LogWarning("GameEventListener is missing event");
    }
}