using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int         maxHearts = 3;
    [SerializeField]
    private float       moveSpeed = 20.0f;
    [SerializeField]
    private float       jumpSpeed = 100.0f;
    [SerializeField]
    private float       maxJumpTime = 0.1f;
    [SerializeField]
    private int         maxJumps = 1;
    [SerializeField]
    private int         jumpGravityStart = 1;
    [SerializeField]
    Collider2D          groundCollider;
    [SerializeField]
    Collider2D          airCollider;
    [SerializeField]
    private Transform   groundCheckObject;
    [SerializeField]
    private float       groundCheckRadius = 3.0f;
    [SerializeField]
    private LayerMask   groundCheckLayer;
    [SerializeField]
    private float       invulnerabilityDuration = 2.0f;
    [SerializeField]
    private float       blinkDuration = 0.2f;

    private float           hAxis;
    private Rigidbody2D     rb;
    private SpriteRenderer  spriteRenderer;
    private Animator        animator;
    private int             nJumps;
    private float           timeOfJump;
    private int             hearts;
    private float           invulnerabilityTimer = 0;
    private float           blinkTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        hearts = maxHearts;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(hAxis * moveSpeed, rb.velocity.y);
    }

    private void Update()
    {
        hAxis = Input.GetAxis("Horizontal");

        Collider2D collider = Physics2D.OverlapCircle(groundCheckObject.position, groundCheckRadius, groundCheckLayer);

        bool isGround = (collider != null);
        
        if ((isGround) && (Mathf.Abs(rb.velocity.y) < 0.1f))
        {
            nJumps = maxJumps;
        }

        Vector2 currentVelocity = rb.velocity;

        if ((Input.GetButtonDown("Jump")) && (nJumps > 0))
        {
            currentVelocity.y = jumpSpeed;

            rb.velocity = currentVelocity;

            nJumps--;

            rb.gravityScale = jumpGravityStart;

            timeOfJump = Time.time;
        }
        else 
        {
            float elapsedTimeSinceJump = Time.time - timeOfJump;
            if ((Input.GetButton("Jump")) && (elapsedTimeSinceJump < maxJumpTime))
            {
                rb.gravityScale = jumpGravityStart;
            }
            else
            {
                rb.gravityScale = 5.0f;
            }
        }

        if (currentVelocity.x < -0.1)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.y = 180;
            transform.rotation = Quaternion.Euler(currentRotation);
        }
        else if (currentVelocity.x > 0.1)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.y = 0;
            transform.rotation = Quaternion.Euler(currentRotation);
        }

        animator.SetFloat("AbsSpeedX", Mathf.Abs(currentVelocity.x));
        animator.SetFloat("SpeedY", currentVelocity.y);
        animator.SetBool("OnGround", isGround);

        groundCollider.enabled = isGround;
        airCollider.enabled = !isGround;

        if (invulnerabilityTimer > 0)
        {
            invulnerabilityTimer = invulnerabilityTimer - Time.deltaTime;

            blinkTimer = blinkTimer - Time.deltaTime;
            if (blinkTimer <= 0)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;

                blinkTimer = blinkDuration;
            }
        }
        else
        {
            spriteRenderer.enabled = true;
        }
    }

    public void TakeDamage(int damage)
    {
        if (invulnerabilityTimer > 0) return;
        if (hearts <= 0) return;

        hearts = hearts - damage;

        if (hearts == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            invulnerabilityTimer = invulnerabilityDuration;
            blinkTimer = blinkDuration;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckObject != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(groundCheckObject.position, groundCheckRadius);
        }

        if (rb != null)
        {
            Vector3 velocity = rb.velocity;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + Vector3.up * 10.0f, transform.position + velocity + Vector3.up * 10.0f);
        }
    }
}
