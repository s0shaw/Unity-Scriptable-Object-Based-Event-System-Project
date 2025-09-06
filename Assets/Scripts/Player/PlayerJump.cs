using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float minJumpForce = 5f;
    [SerializeField] private float maxJumpForce = 10f;
    [SerializeField] private float maxChargeTime = 1f;

    [SerializeField] private float coyoteTime = 0.15f;

    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float fallingGravityScale = 2f;

    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private GameEvent onJumpStartedEvent;
    [SerializeField] private GameEvent onLandedEvent;
    [SerializeField] private FloatGameEvent onChargeUpdateEvent;

    private Rigidbody rb;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private bool wasGroundedLastFrame;

    private bool isCharging;
    private float currentCharge;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PerformGroundCheck();
        HandleCoyoteTime();
        HandleCharging();
    }

    private void FixedUpdate()
    {
        HandleGravity();
    }

    public void HandleJumpPressed()
    {
        isCharging = true;
        currentCharge = 0f;
    }

    public void HandleJumpReleased()
    {
        if (!isCharging) return;
        
        isCharging = false;
        
        if (coyoteTimeCounter > 0f)
        {
            float chargeRatio = Mathf.Clamp01(currentCharge / maxChargeTime);
            float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, chargeRatio);

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            coyoteTimeCounter = 0f;
            
            onJumpStartedEvent?.Raise();
        }
        
        onChargeUpdateEvent?.Raise(0f);
    }

    private void HandleCharging()
    {
        if (isCharging && isGrounded)
        {
            currentCharge += Time.deltaTime;
            float chargeRatio = Mathf.Clamp01(currentCharge / maxChargeTime);
            onChargeUpdateEvent?.Raise(chargeRatio);
        }
    }

    private void HandleCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void PerformGroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);
        
        if (!wasGroundedLastFrame && isGrounded)
        {
            onLandedEvent?.Raise();
        }
        wasGroundedLastFrame = isGrounded;
    }

    private void HandleGravity()
    {
        float scale = rb.velocity.y < 0 ? fallingGravityScale : gravityScale;
        rb.AddForce(Physics.gravity * scale, ForceMode.Acceleration);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }
} 