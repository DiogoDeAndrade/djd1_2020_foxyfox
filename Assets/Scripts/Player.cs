﻿using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private float moveSpeed = 20.0f;
    [SerializeField]
    private float jumpSpeed = 100.0f;
    [SerializeField]
    private float maxJumpTime = 0.1f;
    [SerializeField]
    private int maxJumps = 1;
    [SerializeField]
    private int jumpGravityStart = 1;
    [SerializeField]
    Collider2D groundCollider;
    [SerializeField]
    Collider2D airCollider;
    [SerializeField]
    float useRadius = 50;
    [SerializeField]
    LayerMask useLayer;
    [SerializeField]
    AudioSource jumpSound;

    private float hAxis;
    private int nJumps;
    private float timeOfJump;
    private int currentScore = 0;

    public int score => currentScore;

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(hAxis * moveSpeed, rb.velocity.y);
        }
    }

    protected override void Update()
    {
        hAxis = Input.GetAxis("Horizontal");

        bool isGround = IsGround();

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

            jumpSound.pitch = Random.Range(0.8f, 1.2f);
            jumpSound.volume = Random.Range(0.7f, 1.0f);
            jumpSound.Play();
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

        TurnTo(-currentVelocity.x);

        animator.SetFloat("AbsSpeedX", Mathf.Abs(currentVelocity.x));
        animator.SetFloat("SpeedY", currentVelocity.y);
        animator.SetBool("OnGround", isGround);

        if (!isDead)
        {
            groundCollider.enabled = isGround;
            airCollider.enabled = !isGround;
        }

        if (Input.GetButtonDown("Use"))
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, useRadius, useLayer);
            if (collider)
            {
                DialogueCharacter dc = collider.GetComponent<DialogueCharacter>();
                if (dc)
                {
                    dc.StartDialogue(spriteRenderer);
                }
            }
        }

        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layerId = collision.gameObject.layer;
        int checkLayer = dealDamageLayers.value & (1 << layerId);

        if (checkLayer == 0)
        {
            Character character = collision.GetComponentInParent<Character>();
            if (character != null)
            {
                if (rb.velocity.y < -50.0f)
                {
                    character.DealDamage(1, Vector2.down);

                    rb.velocity = Vector2.up * knockbackVelocity;

                    knockbackTimer = hitKnockbackDuration;

                    if (character.isDead)
                    {
                        currentScore++;
                    }
                }
            }
        }
    }

    protected override void OnDeath()
    {
        GameMng gameManager = FindObjectOfType<GameMng>();
        gameManager.BackToMainMenu(2);
        gameManager.UpdateHighscore(score);

        GameOver gameOverObj = FindObjectOfType<GameOver>(true);
        gameOverObj.Activate();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, useRadius);
    }
}
