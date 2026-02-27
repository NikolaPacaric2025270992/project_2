using UnityEngine;
using System.Collections;

public class EnemyFrogAI : MonoBehaviour
{
    [Header("Jump Physics")]
    public float jumpHeight = 5f;
    public float jumpForward = 3f;
    public float timeBetweenJumps = 1.0f; // The 1 second wait

    [Header("Route Settings")]
    public float patrolDistance = 5f;
    public float tauntDuration = 1.5f;

    [Header("Detection")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private float leftX, rightX, targetX;
    private bool isGrounded;
    private bool isActing = false; // Prevents overlapping jumps/taunts

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        leftX = transform.position.x - patrolDistance;
        rightX = transform.position.x + patrolDistance;
        targetX = rightX;
    }

    void Update()
    {
        // Always track if we are grounded for the Animator
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        anim.SetBool("isJumping", !isGrounded);

        // If we are on the ground and not already doing something, start the next action
        if (isGrounded && !isActing)
        {
            if (Mathf.Abs(transform.position.x - targetX) < 0.5f)
            {
                StartCoroutine(TauntSequence());
            }
            else
            {
                StartCoroutine(JumpSequence());
            }
        }
    }

    private IEnumerator JumpSequence()
    {
        isActing = true;

        // 1. Perform the jump
        float direction = (targetX > transform.position.x) ? 1 : -1;
        rb.linearVelocity = new Vector2(direction * jumpForward, jumpHeight);
        anim.SetTrigger("frog_jumps");

        // 2. Wait until the frog is actually in the air (prevents instant landing trigger)
        yield return new WaitUntil(() => !isGrounded);

        // 3. Wait until the frog hits the ground again
        yield return new WaitUntil(() => isGrounded);

        rb.linearVelocity = Vector2.zero;

        // 4. The "Recovery" pause
        yield return new WaitForSeconds(timeBetweenJumps);

        isActing = false;
    }

    private IEnumerator TauntSequence()
    {
        isActing = true;
        rb.linearVelocity = Vector2.zero; // Stop any slide

        anim.SetBool("isTaunting", true);
        yield return new WaitForSeconds(tauntDuration);
        anim.SetBool("isTaunting", false);

        // Flip and reset
        targetX = (targetX == rightX) ? leftX : rightX;
        sprite.flipX = (targetX == leftX);

        isActing = false;
    }
    // Visual aid in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}

