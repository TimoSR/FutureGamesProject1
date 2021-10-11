using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Agent : MonoBehaviour
{

    public Rigidbody2D rb2d;
    public PlayerInput playerInput;
    public AgentAnimation animationManager;
    public AgentRenderer agentRenderer; 
    
    [field: SerializeField]
    float jumpForce = 10f;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        rb2d = GetComponent<Rigidbody2D>();
        animationManager = GetComponentInChildren<AgentAnimation>();
        agentRenderer = GetComponentInChildren<AgentRenderer>();
    }

    private void Start()
    {
        playerInput.OnMovement += Movement;
        playerInput.OnMovement += agentRenderer.FaceDirection;
        playerInput.OnJumpPressed += Jump;

    }

    private void Jump()
    {
        
        rb2d.velocity = Vector2.up * jumpForce;
    }

    private void Movement(Vector2 input)
    {
        if (Mathf.Abs(input.x) > 0)
        {
            if (Mathf.Abs(rb2d.velocity.x) < 0.01f)
            {
                animationManager.PlayAnimation(AnimationType.run);
            }
            
            rb2d.velocity = new Vector2(input.x*10, rb2d.velocity.y);
            
        }
        else
        {
            
            if (Mathf.Abs(rb2d.velocity.x) > 0)
            {
                animationManager.PlayAnimation(AnimationType.idle);
            }
            
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
    }
}
