using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Agent : MonoBehaviour
{

    [field: SerializeField] private LayerMask _layerMask;
    [field: SerializeField] private bool debugMode;
    public Rigidbody2D rb2d;
    public PlayerInput playerInput;
    public AgentAnimation animationManager;
    public AgentRenderer agentRenderer;
    public BoxCollider2D boxCollider2D;

    [field: SerializeField]
    float jumpForce = 10f;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        rb2d = GetComponent<Rigidbody2D>();
        animationManager = GetComponentInChildren<AgentAnimation>();
        agentRenderer = GetComponentInChildren<AgentRenderer>();
        boxCollider2D = GetComponentInChildren<BoxCollider2D>();
    }

    private void Start()
    {
        playerInput.OnMovement += Movement;
        playerInput.OnMovement += agentRenderer.FaceDirection;
        playerInput.OnJumpPressed += Jump;

    }

    private void Jump()
    {
        if (debugMode == false)
        {
            if (IsGrounded())
            {
                rb2d.velocity = Vector2.up * jumpForce;
            }
        }
        else
        {
            rb2d.velocity = Vector2.up * jumpForce;
        }
    }
        

    private bool IsGrounded()
    {

        var bounds = boxCollider2D.bounds;
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(bounds.center, bounds.size,
            0f, Vector2.down, .1f, _layerMask);
        Debug.Log(raycastHit2D);
        return raycastHit2D.collider != null;

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
