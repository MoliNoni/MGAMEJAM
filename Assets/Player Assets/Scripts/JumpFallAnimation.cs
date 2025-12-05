using UnityEngine;

public class JumpFallAnimation : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    public float threshold = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float velY = rb.velocity.y;
        if (velY > threshold)
        {
            animator.SetBool("Is Jumping", true);
            animator.SetBool("Is Falling", false);
        }
        else if (velY < -threshold)
        {
            animator.SetBool("Is Jumping", false);
            animator.SetBool("Is Falling", true);
        }

        else
        {
            animator.SetBool("Is Jumping", false);
            animator.SetBool("Is Falling", false);
        }
    }
}
