using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Float Game Event")]
public class FloatGameEvent : ScriptableObject
{
    private readonly List<FloatEventListener> listeners = new List<FloatEventListener>();

    public void Raise(float value)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(value);
        }
    }

    public void RegisterListener(FloatEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(FloatEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
} 