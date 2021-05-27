using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum Faction { Friend, Enemy };

    [SerializeField]
    GameObject            gfx;
    [SerializeField]
    protected Faction     faction;
    [SerializeField]
    protected int         maxHearts = 3;
    [SerializeField]
    protected Transform   groundCheckObject;
    [SerializeField]
    protected float       groundCheckRadius = 3.0f;
    [SerializeField]
    protected LayerMask   groundCheckLayer;
    [SerializeField]
    protected float       invulnerabilityDuration = 2.0f;
    [SerializeField]
    protected float       blinkDuration = 0.2f;
    [SerializeField]
    protected float       knockbackVelocity = 30.0f;
    [SerializeField]
    protected float       knockbackDuration = 0.25f;
    [SerializeField]
    protected float       hitKnockbackDuration = 0.2f;
    [SerializeField]
    protected GameObject  deathPrefab;
    [SerializeField]
    protected LayerMask   dealDamageLayers;
    [SerializeField]
    AudioSource           hurtSound;
    [SerializeField]
    AudioClip             dieSoundClip;

    protected Rigidbody2D       rb;
    protected SpriteRenderer    spriteRenderer;
    protected Animator          animator;
    protected int               hearts;
    protected float             invulnerabilityTimer = 0;
    protected float             blinkTimer;
    protected float             knockbackTimer;

    public bool isDead => hearts <= 0;
    protected bool isInvulnerable { get { return (invulnerabilityTimer > 0); } }
    protected bool canHit { get { return (!isInvulnerable) && (!isDead); } }
    protected bool canMove { get { return (knockbackTimer <= 0) && (!isDead); } }

    protected virtual bool knockbackOnHit => true;

    public int nHearts => hearts;
    public int nMaxHearts => maxHearts;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = gfx.GetComponent<SpriteRenderer>();
        animator = gfx.GetComponent<Animator>();

        hearts = maxHearts;
    }

    protected virtual void Update()
    {
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

        if (knockbackTimer > 0)
        {
            knockbackTimer = knockbackTimer - Time.deltaTime;
        }
    }

    public void DealDamage(int damage, Vector2 hitDirection)
    {
        if (!canHit) return;

        hearts = hearts - damage;

        if (hurtSound != null)
        {
            hurtSound.pitch = Random.Range(0.75f, 1.25f);
            hurtSound.Play();
        }

        if (hearts == 0)
        {
            if (deathPrefab)
            {
                GameObject explosionObject = Instantiate(deathPrefab, transform.position, transform.rotation);
                explosionObject.transform.localScale = transform.localScale;

                Destroy(gameObject, 0.100f);
            }
            else
            {
                rb.velocity = Vector2.up * 300;
                knockbackTimer = 2.0f;
                Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
                foreach (var collider in colliders)
                {
                    collider.enabled = false;
                }

                Destroy(gameObject, 2.0f);
            }

            OnDeath();

            animator.SetTrigger("Die");

            SoundManager.instance.PlaySound(dieSoundClip, 1.0f, Random.Range(0.75f, 1.25f));
        }
        else
        {
            invulnerabilityTimer = invulnerabilityDuration;
            blinkTimer = blinkDuration;

            animator.SetTrigger("Hurt");

            if (knockbackOnHit)
            {
                Vector2 knockback = hitDirection.normalized * knockbackVelocity + Vector2.up * knockbackVelocity * 0.5f;
                rb.velocity = knockback;

                knockbackTimer = knockbackDuration;
            }
        }
    }

    protected bool IsGround()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundCheckObject.position, groundCheckRadius, groundCheckLayer);

        return (collider != null);
    }

    protected void TurnTo(float dirX)
    {
        if (dirX < -0.1)
        {
            Vector3 currentRotation = gfx.transform.rotation.eulerAngles;
            currentRotation.y = 0;
            gfx.transform.rotation = Quaternion.Euler(currentRotation);
        }
        else if (dirX > 0.1)
        {
            Vector3 currentRotation = gfx.transform.rotation.eulerAngles;
            currentRotation.y = 180;
            gfx.transform.rotation = Quaternion.Euler(currentRotation);
        }
    }

    protected virtual void OnDrawGizmosSelected()
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

    public bool IsHostile(Faction faction)
    {
        if (faction != this.faction) return true;

        return false;
    }

    protected virtual void OnDeath()
    {

    }
}
