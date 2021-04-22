using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    [SerializeField]
    private float       moveDir = -1.0f;
    [SerializeField]
    private float       moveSpeed = 100.0f;
    private Rigidbody2D rb;
    [SerializeField]    
    private Transform   groundCheckObject;
    [SerializeField]    
    private Transform   wallCheckObject;
    [SerializeField]
    private float       groundCheckRadius = 3.0f;
    [SerializeField]
    private LayerMask   groundCheckLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D groundCollider = Physics2D.OverlapCircle(groundCheckObject.position, groundCheckRadius, groundCheckLayer);

        bool hasGround = (groundCollider != null);

        Collider2D wallCollider = Physics2D.OverlapCircle(wallCheckObject.position, groundCheckRadius, groundCheckLayer);

        bool hasWall = (wallCollider != null);

        if ((!hasGround) || (hasWall))
        {
            moveDir = -moveDir;
        }

        Vector2 currentVelocity = rb.velocity;

        currentVelocity.x = moveDir * moveSpeed;

        rb.velocity = currentVelocity;

        if (currentVelocity.x < -0.1)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.y = 0;
            transform.rotation = Quaternion.Euler(currentRotation);
        }
        else if (currentVelocity.x > 0.1)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.y = 180;
            transform.rotation = Quaternion.Euler(currentRotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckObject != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(groundCheckObject.position, groundCheckRadius);
        }
        if (wallCheckObject != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(wallCheckObject.position, groundCheckRadius);
        }
    }
}
