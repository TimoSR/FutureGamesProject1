using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 10;
    public float gravity = 20;
    public bool isOnGround = true;



    Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
     //Horizontal movement
        float HorizontalInput = Input.GetAxis("Horizontal");

        body.velocity = new Vector3(HorizontalInput * speed, body.velocity.y);
        Physics.gravity = new Vector3(0, -gravity, 0);

        // flips player towards the running direction
        if (HorizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (HorizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        //jump
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            Jump();
        }
    }

    void Jump()
    {
        body.velocity = new Vector3(body.velocity.x, speed, body.velocity.y);
        isOnGround = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isOnGround = true;
        }
    }
}
