using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 10;
    //public float gravity = 20;

    Rigidbody2D body;
    BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Animator anim;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
    //Horizontal movement
        float HorizontalInput = Input.GetAxis("Horizontal");

        body.velocity = new Vector3(HorizontalInput * speed, body.velocity.y);

    //flips player towards the running direction
        if (HorizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (HorizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

    //jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            Jump();
        }

    // slide on walls thing
        if (onWall())
        {

            anim.SetTrigger("IsOnWall");
            body.velocity = Vector2.zero;
            body.gravityScale = 70;
        }
        else
        {
            body.gravityScale = 3;
        }


    //Animator states
        anim.SetBool("Run", HorizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());
        anim.SetBool("IsOnWall", onWall());
    }

   // void wallslide()
   // {
   //     body.velocity = new vector3(body.velocity.x, body.gravityscale *=2, body.velocity.y);
   //     anim.settrigger("isonwall");
   //     onwall();
   //}


    void Jump()
    {
        body.velocity = new Vector3(body.velocity.x, speed, body.velocity.y);
        anim.SetTrigger("Jump");
        isGrounded();
    }


    // Checks if the player is gounded using boxCast and physics2D
    // Not sure if the BoxCollider2D and RigidBody2D will break the game
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }


    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
