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
    private TrailRenderer trail;
    private ParticleSystem dashParticles;
    private Renderer playerRenderer;
    private Color baseColor;
    private Color dashColor = new Color(1f, 0.3f, 0f, 1f);
    private AudioSource sfxSource;

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

        // Visual feedback refs
        trail = GetComponent<TrailRenderer>();
        if (trail != null) trail.enabled = false;
        playerRenderer = GetComponent<Renderer>();
        if (playerRenderer != null) baseColor = playerRenderer.material.color;
        var psGo = transform.Find("DashParticles");
        if (psGo != null) dashParticles = psGo.GetComponent<ParticleSystem>();

        // SFX AudioSource
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.volume = 0.5f;
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

        // Update glow visual
        UpdateGlowVisual();

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
        PlaySFX("jump");
    }

    System.Collections.IEnumerator DashRoutine()
    {
        isDashing = true;
        isInvincible = true;
        dashCooldownTimer = data.dashCooldown;
        OnDash?.Invoke();

        // Visual: enable trail, change color, burst particles
        if (trail != null) trail.enabled = true;
        if (playerRenderer != null) playerRenderer.material.color = dashColor;
        if (dashParticles != null)
        {
            dashParticles.gameObject.SetActive(true);
            dashParticles.Play();
        }
        PlaySFX("dash");

        // Pass through obstacles: make collider trigger during dash
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;

        float timer = 0;
        float dashSpeed = data.dashDistance / data.dashDuration;

        while (timer < data.dashDuration)
        {
            rb.linearVelocity = new Vector3(0, 0, dashSpeed);
            timer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;

        // Keep invincibility color for remaining duration, then revert
        yield return new WaitForSeconds(data.dashInvincibility - data.dashDuration);
        isInvincible = false;

        // Revert visual
        if (trail != null) trail.enabled = false;
        if (playerRenderer != null) playerRenderer.material.color = baseColor;
        if (dashParticles != null) dashParticles.Stop();

        // Restore solid collider
        var col2 = GetComponent<Collider>();
        if (col2 != null) col2.isTrigger = false;
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
            PlaySFX("death");
            OnDeath?.Invoke();
        }
    }

    void PlaySFX(string name)
    {
        var holder = GetComponentInChildren<SFXHolder>();
        if (holder == null) return;
        switch (name)
        {
            case "jump": holder.Play(holder.jumpClip); break;
            case "dash": holder.Play(holder.dashClip); break;
            case "death": holder.Play(holder.deathClip); break;
            case "shard": holder.Play(holder.shardClip); break;
        }
    }

    void UpdateGlowVisual()
    {
        float glow = GameManager.Instance != null ? GameManager.Instance.GlowIntensity : 0f;

        // Emissive material intensity
        if (playerRenderer != null && !isDashing)
        {
            Color baseEmission = new Color(0.1f, 0.8f, 0.2f);
            Color glowEmission = baseEmission * (1f + glow * 2f);
            playerRenderer.material.SetColor("_EmissionColor", glowEmission);
            // Color shift: green → white-cyan at high glow
            playerRenderer.material.color = Color.Lerp(baseColor, new Color(0.7f, 1f, 0.9f), glow);
        }

        // Trail particle rate
        if (trail != null)
        {
            trail.time = 0.3f + glow * 0.7f;
            trail.startColor = Color.Lerp(new Color(0.2f, 1f, 0.4f, 1f), new Color(0.7f, 1f, 1f, 1f), glow);
        }
    }
}