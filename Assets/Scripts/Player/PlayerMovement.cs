using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private ForceMode forceMode = ForceMode.VelocityChange;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = transform.forward * moveSpeed;
        
        targetVelocity.y = rb.velocity.y;
        
        Vector3 velocityChange = (targetVelocity - rb.velocity);
        
        velocityChange.y = 0;
        
        rb.AddForce(velocityChange, forceMode);
    }
} 