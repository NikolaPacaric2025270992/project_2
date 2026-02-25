using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float airSpeed = 2.5f;
    public float jumpForce = 12f;
    public float groundAcceleration = 20f;
    public float airAcceleration = 5f;
    public float coyoteTime = 0.2f;

    public float coyoteTimeCounter;

    private Rigidbody2D rb;
    private PlayerControls controls;
    private Vector2 moveInput;
    private bool jumpPressed;

    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;
    public bool isGrounded;
    private bool wasGrounded;

    private Animator anim;
    public float velocity;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Jump.performed += ctx => jumpPressed = true;
        controls.Player.Jump.canceled += ctx => jumpPressed = false;
    }

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (rb.linearVelocity.x > 0.05f)
        {
            transform.localScale = new Vector3(1, 1, 1);   // facing right
        }
        else if (rb.linearVelocity.x < -0.05f)
        {
            transform.localScale = new Vector3(-1, 1, 1);  // facing left
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        // Reset vertical velocity when landing
        if (isGrounded && !wasGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }

        wasGrounded = isGrounded;

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // reset when grounded
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // count down when airborne
        }

        float targetSpeed = moveInput.x * moveSpeed;
        float accel = isGrounded ? groundAcceleration : airAcceleration;
        float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accel * Time.deltaTime);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);

        if (jumpPressed && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("jumpUp");
            coyoteTimeCounter = 0f;
        }

        float animSpeed = Mathf.Abs(rb.linearVelocity.x);
        if (Mathf.Abs(moveInput.x) < 0.01f && isGrounded)
        {
            animSpeed = 0f;
        }
        anim.SetFloat("Speed", animSpeed);
        anim.SetFloat("verticalVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
        velocity = rb.linearVelocity.y;
    }

    void FixedUpdate()
    {
        float slopeBoost = isGrounded && !jumpPressed ? 1.5f : 1f;
        float targetSpeed = moveInput.x * moveSpeed * slopeBoost;
        float accel = isGrounded ? groundAcceleration : airAcceleration;
        float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accel * Time.deltaTime);
        Vector2 desiredVelocity = new Vector2(newX, rb.linearVelocity.y);

        if (isGrounded && !jumpPressed) // only slide when grounded AND not jumping
        {
            var slideMovement = new Rigidbody2D.SlideMovement
            {
                useSimulationMove = true
            };

            var results = rb.Slide(desiredVelocity, Time.fixedDeltaTime, slideMovement);

            // Animator uses the actual applied velocity
            anim.SetFloat("Speed", Mathf.Abs(results.remainingVelocity.x));
            anim.SetBool("isGrounded", true);
        }
        else
        {
            // Airborne movement: let physics handle it
            rb.linearVelocity = desiredVelocity;
            anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
            anim.SetBool("isGrounded", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }

}