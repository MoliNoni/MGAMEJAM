using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private JumpingFallingController  jumpingFallingController;
    private Animator animator;
    private float moveX = 0;
    private float originalGravityScale;
    private Queue<KeyCode> inputBuffer;
    [SerializeField ] private float minimalJumpVariable;
    public float maxAirTime;
    public bool canJump;
    public bool jumpInput;
    public float constantHorizontalSpeed = 1450f;
    public float constantVerticalSpeed = 77f;
    [SerializeField] private LayerMask groundLayer;
    public float coyoteAirTime;
    public RaycastHit2D raycastGround;
    public RaycastHit2D raycastGround1;
    public RaycastHit2D raycastGround2;
    public bool onGround;


   
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        originalGravityScale = rb.gravityScale;
        inputBuffer = new Queue<KeyCode>();
        jumpingFallingController = gameObject.GetComponent<JumpingFallingController>();
    }

    void Update()
    {   //Horizontal movement handler

        if (Input.GetKey("a"))
        {
            
            moveX = -1f;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            animator.SetBool("Is Moving", true);
        }
        else if (Input.GetKey("d"))
        {
            moveX = 1f;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            animator.SetBool("Is Moving", true);
        }
        else 
        {
            moveX = 0f;
        }

        if (Input.GetKey("a") && Input.GetKey("d") && onGround)
        {
            moveX = 0f;
        }

        if (rb.velocity.x == 0)
        {
            animator.SetBool("Is Moving", false);
        }

        Vector2 raycastOrigin = (Vector2)transform.position + new Vector2(-2.2f, -4.5f);
        Vector2 raycastOrigin1 = (Vector2)transform.position + new Vector2(0, -4.5f);
        Vector2 raycastOrigin2 = (Vector2)transform.position + new Vector2(2.2f, -4.5f);

        raycastGround = Physics2D.Raycast(raycastOrigin, Vector2.down, 1f, groundLayer); 
        raycastGround1 = Physics2D.Raycast(raycastOrigin1, Vector2.down, 1f, groundLayer); 
        raycastGround2 = Physics2D.Raycast(raycastOrigin2, Vector2.down, 1f, groundLayer); 

        Debug.DrawRay(raycastOrigin, Vector2.down * 1f, Color.red);
        Debug.DrawRay(raycastOrigin1, Vector2.down * 1f, Color.green);
        Debug.DrawRay(raycastOrigin2, Vector2.down * 1f, Color.blue);

        //Ground Detection
        if (raycastGround.collider != null || raycastGround1.collider != null || raycastGround2.collider != null)
        {
            canJump = true;
            onGround = true;
        }
        else
        {
            onGround = false;
        }
        //Jump detection and queueing
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
        }
        //Jumping setting & jumping queue handler 
        if (onGround == true)
        {   
            coyoteAirTime = 0; //Coyote airtime always zero when toching ground
            if (inputBuffer.Count > 0)
            {
                if (inputBuffer.Peek() == KeyCode.Space)
                {
                    rb.velocity = new Vector2(rb.velocity.x, constantVerticalSpeed);
                    inputBuffer.Dequeue();       
                }
            } 
        }
        else    //Coyote time
        {
            coyoteAirTime += Time.deltaTime;
        }
        
        if (onGround == false && coyoteAirTime < 0.18f && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, constantVerticalSpeed);
            inputBuffer.Dequeue();
            coyoteAirTime = 0;   
        }  
    }

    void removeJumpQueue()
    {
        if (inputBuffer.Count>0)
        {
            inputBuffer.Dequeue();
        }
        
    }

    void FixedUpdate()
    {   

        rb.velocity = new Vector2(moveX * constantHorizontalSpeed, rb.velocity.y);




    }
}
