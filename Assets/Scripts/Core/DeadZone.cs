using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] private GameEvent onPlayerDied;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (onPlayerDied != null)
            {
                onPlayerDied.Raise();
            }
        }
    }
} 