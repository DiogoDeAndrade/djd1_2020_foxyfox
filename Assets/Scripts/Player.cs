using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float       moveSpeed = 20.0f;
    [SerializeField]
    private float       jumpSpeed = 100.0f;
    [SerializeField]
    private int         maxJumps = 1;
    [SerializeField]
    private Transform   groundCheckObject;
    [SerializeField]
    private float       groundCheckRadius = 3.0f;
    [SerializeField]
    private LayerMask   groundCheckLayer;

    private float       hAxis;
    private Rigidbody2D rb;
    private int         nJumps;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        if ((Input.GetButtonDown("Jump")) && (nJumps > 0))
        {
            Vector2 currentVelocity = rb.velocity;

            currentVelocity.y = jumpSpeed;

            rb.velocity = currentVelocity;

            nJumps--;
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
