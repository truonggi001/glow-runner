using UnityEngine;

/// <summary>
/// Movement system: auto-run + jump + dash. GDD: movement-auto-run-jump-dash.md
/// Loads values from assets/data/movement.json via GameData.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("State (read-only)")]
    public bool isGrounded;
    public bool isDashing;
    public bool isInvincible;
    public float currentSpeed;
    public float speedMultiplier = 1f;

    private Rigidbody rb;
    private GameData.MovementData data;
    private int jumpCount;
    private float dashTimer;
    private float dashCooldownTimer;
    private float coyoteTimer;
    private float speedRampTimer;
    private float startY;

    public System.Action OnDeath;
    public System.Action OnDash;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        data = GameData.Movement ?? new GameData.MovementData();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX;
        currentSpeed = data.baseSpeed;
        startY = transform.position.y;
        jumpCount = 0;
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameManager.State.Playing)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        // Speed ramp
        speedRampTimer += Time.deltaTime;
        if (speedRampTimer >= data.speedRampInterval)
        {
            speedRampTimer = 0;
            speedMultiplier = Mathf.Min(speedMultiplier + data.speedRampPercent / 100f, data.speedCapMultiplier);
        }
        currentSpeed = data.baseSpeed * speedMultiplier;

        // Jump
        coyoteTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || coyoteTimer > 0)
            {
                Jump(data.jumpMultiplierGround);
                jumpCount = 1;
                coyoteTimer = 0;
            }
            else if (jumpCount < 2)
            {
                Jump(data.jumpMultiplierDouble);
                jumpCount = 2;
            }
        }

        // Dash
        dashCooldownTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0)
        {
            StartCoroutine(DashRoutine());
        }

        // Death by falling
        if (transform.position.y < -10)
        {
            OnDeath?.Invoke();
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameManager.State.Playing)
            return;

        // Auto-run forward (Z+)
        if (!isDashing)
        {
            Vector3 v = rb.linearVelocity;
            v.z = currentSpeed;
            rb.linearVelocity = v;
        }
    }

    void Jump(float multiplier)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, data.jumpForce * multiplier, rb.linearVelocity.z);
        isGrounded = false;
    }

    System.Collections.IEnumerator DashRoutine()
    {
        isDashing = true;
        isInvincible = true;
        dashCooldownTimer = data.dashCooldown;
        OnDash?.Invoke();

        float timer = 0;
        float dashSpeed = data.dashDistance / data.dashDuration;

        while (timer < data.dashDuration)
        {
            rb.linearVelocity = new Vector3(0, 0, dashSpeed);
            timer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        yield return new WaitForSeconds(data.dashInvincibility - data.dashDuration);
        isInvincible = false;
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                if (!isGrounded)
                {
                    isGrounded = true;
                    jumpCount = 0;
                    coyoteTimer = data.coyoteTime;
                }
                break;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Hit obstacle
        if (collision.gameObject.CompareTag("Obstacle") && !isInvincible)
        {
            OnDeath?.Invoke();
        }
    }
}