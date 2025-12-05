using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private JumpingFallingController jumpingFallingController;
    private Animator animator;
    private float moveX = 0;
    private float originalGravityScale;
    private Queue<KeyCode> inputBuffer;
    [SerializeField] private float minimalJumpVariable;
    public float maxAirTime;
    public bool canJump;
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private bool hasJumped;

    public bool jumpInput;
    public float constantHorizontalSpeed = 1450f;
    public float constantVerticalSpeed = 77f;
    public float doubleJumpHeightPenalty = 10f;
    public float constantWalkingSpeed = 1000f;
    [SerializeField] private LayerMask groundLayer;
    public float coyoteAirTime;
    public RaycastHit2D raycastGround;
    public RaycastHit2D raycastGround1;
    public RaycastHit2D raycastGround2;
    public bool onGround;
    private SpriteRenderer sprite;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        originalGravityScale = rb.gravityScale;
        inputBuffer = new Queue<KeyCode>();
        jumpingFallingController = gameObject.GetComponent<JumpingFallingController>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Horizontal movement handler
        if (Input.GetKey("a") && Input.GetKey("d"))
        {
            moveX = 0f;
            animator.SetBool("Is Moving", false);
        }
        else if (Input.GetKey("a"))
        {
            moveX = -1f;
            sprite.flipX = true;
            animator.SetBool("Is Moving", true);
        }
        else if (Input.GetKey("d"))
        {
            moveX = 1f;
            sprite.flipX = false;
            animator.SetBool("Is Moving", true);
        }
        else
        {
            moveX = 0f;
            animator.SetBool("Is Moving", false);
        }

        if (Input.GetKey("left shift")) 
        {
            moveX *= 0.5f;  
        }

        Vector2 raycastOrigin = (Vector2)transform.position + new Vector2(0.3f, 0f);
        Vector2 raycastOrigin1 = (Vector2)transform.position + new Vector2(0f, 0f);
        Vector2 raycastOrigin2 = (Vector2)transform.position + new Vector2(-0.3f, 0f);

        raycastGround = Physics2D.Raycast(raycastOrigin, Vector2.down, 1.5f, groundLayer); 
        raycastGround1 = Physics2D.Raycast(raycastOrigin1, Vector2.down, 1.5f, groundLayer); 
        raycastGround2 = Physics2D.Raycast(raycastOrigin2, Vector2.down, 1.5f, groundLayer); 

        // Ground Detection
        if (raycastGround.collider != null || raycastGround1.collider != null || raycastGround2.collider != null)
        {
            canJump = true;
            onGround = true;
            animator.SetBool("Is On Air", false);
        }
        else
        {
            onGround = false;
            animator.SetBool("Is On Air", true);
        }

        // Jump detection and queueing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputBuffer.Enqueue(KeyCode.Space);
            Invoke("removeJumpQueue", 0.15f);
        }

        if (onGround == false)
        {
            canJump = false;
            jumpInput = false;
            maxAirTime += Time.deltaTime;
        }
        else
        {
            canJump = true;
            jumpingFallingController.dropJumpButton = 0;
            maxAirTime = 0f;
            hasJumped = false;
        }

        // Jumping setting & jumping queue handler
        if (onGround == true)
        {   
            coyoteAirTime = 0; // Coyote airtime always zero when touching ground
            if (inputBuffer.Count > 0)
            {
                if (inputBuffer.Peek() == KeyCode.Space)
                {
                    rb.velocity = new Vector2(rb.velocity.x, constantVerticalSpeed);
                    inputBuffer.Dequeue();   
                    hasJumped = true; 
                    canDoubleJump = true;  
                }
            } 
        }
        else // Coyote time
        {
            coyoteAirTime += Time.deltaTime;
        }

        // Double Jump
        if (canDoubleJump && inputBuffer.Count > 0)
        {
            if (inputBuffer.Peek() == KeyCode.Space)
            {
                rb.velocity = new Vector2(rb.velocity.x, constantVerticalSpeed - doubleJumpHeightPenalty);
                inputBuffer.Dequeue();
                canDoubleJump = false; 
            } 
        }
    }

    void removeJumpQueue()
    {
        if (inputBuffer.Count > 0)
        {
            inputBuffer.Dequeue();
        }
    }

    void FixedUpdate()
    {   
        rb.velocity = new Vector2(moveX * constantHorizontalSpeed, rb.velocity.y);
    }
}
