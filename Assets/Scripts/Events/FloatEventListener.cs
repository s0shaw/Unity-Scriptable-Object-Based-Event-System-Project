using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FloatUnityEvent : UnityEvent<float> { }

public class FloatEventListener : MonoBehaviour
{
    [SerializeField] private FloatGameEvent gameEvent;
    [SerializeField] private FloatUnityEvent onEventRaised;

    private void OnEnable()
    {
        if (gameEvent == null) return;
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        if (gameEvent == null) return;
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(float value)
    {
        onEventRaised.Invoke(value);
    }
} 