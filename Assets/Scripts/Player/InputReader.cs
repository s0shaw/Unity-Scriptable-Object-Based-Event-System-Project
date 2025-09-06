using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [SerializeField] private GameEvent onJumpPressedEvent;
    [SerializeField] private GameEvent onJumpReleasedEvent;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (onJumpPressedEvent != null)
            {
                onJumpPressedEvent.Raise();
            }
        }
        else if (context.canceled)
        {
            if (onJumpReleasedEvent != null)
            {
                onJumpReleasedEvent.Raise();
            }
        }
    }
} 