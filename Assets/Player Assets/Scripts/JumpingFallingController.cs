using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingFallingController : MonoBehaviour
{
    private PlayerController playerController;
    private Rigidbody2D rb;
    public float maxFallSpeed = 50f;
    public float jumpHangY = 19f;
    public float jumpHangMult = 12.5f;
    public float airtime;
    private bool isHanging;
    public int dropJumpButton;
    public bool hasHanged = false;


    
    // Start is called before the first frame update
    void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //airtime checker for debugging purposes
        if (playerController.canJump == false)
        {
            airtime += Time.deltaTime;
        }

        if (Input.GetKey("t"))
        {
            airtime = 0;
        }
        if (Input.GetKeyUp(KeyCode.Space) && playerController.onGround == false)
        {
            dropJumpButton = dropJumpButton +1;
        } 

        
        //falling handler when dropping jumping button
        if (dropJumpButton == 1)
        {

            if (!Input.GetKey("a") && !Input.GetKey("d"))   //when not moving player doesnt stand mid air when jumping
            {
                if (rb.velocity.y < 0)
                {
                    return; 
                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*0.5f);
                    dropJumpButton = dropJumpButton +1;
                    return;
                }
            }
            else    //when dropping jump player start falling
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*0.5f);
                dropJumpButton = dropJumpButton +1;
            }
        }
    }

    private void FixedUpdate()
    {   
        //Falling speed and Jumping hang time
        if (Mathf.Abs(rb.velocity.y) < jumpHangY && playerController.canJump == false)
        {
            rb.gravityScale = jumpHangMult;
            hasHanged = true;
        }
        //Falling speed when 1.2s on air
        else if ((Mathf.Abs(rb.velocity.y)) > jumpHangY && playerController.onGround == false)
        {
            if (playerController.coyoteAirTime > 0.65f )
            {

                if (dropJumpButton <= 1)
                {
                    rb.gravityScale = 20f;
                }
                else
                {
                    rb.gravityScale = 25;
                }
                
            }
            else 
            {
                rb.gravityScale = 13f;
            }
            
        }

        if (playerController.onGround == true)
        {
            rb.gravityScale = 13f;
            hasHanged = false;
        }
    }

}