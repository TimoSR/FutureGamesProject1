using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Agent : MonoBehaviour
{

    [field: SerializeField] private bool debugMode;
    [field: SerializeField] private LayerMask _layerMask;
    
    public Rigidbody2D rb2d;
    [FormerlySerializedAs("playerInput")] public PlayerInput agentInput;
    public AgentAnimation animationManager;
    public AgentRenderer agentRenderer;
    public BoxCollider2D boxCollider2D;
    
    public State currentState = null, previousState = null;
    public State idleState;
    
    [Header("State debugging")]
    public string stateName = "";

    [field: SerializeField]
    float jumpForce = 10f;

    private void Awake()
    {
        agentInput = GetComponentInParent<PlayerInput>();
        rb2d = GetComponent<Rigidbody2D>();
        animationManager = GetComponentInChildren<AgentAnimation>();
        agentRenderer = GetComponentInChildren<AgentRenderer>();
        boxCollider2D = GetComponentInChildren<BoxCollider2D>();
        
        State[] states = GetComponentsInChildren<State>();
        foreach (var state in states)
        {
            state.InitializeState(this);
        }
    }

    private void Start()
    {
        agentInput.OnMovement += agentRenderer.FaceDirection;
        TransitionToState(idleState);
        agentInput.OnJumpPressed += Jump;
    }

    private void Update()
    {
        currentState.StateUpdate();
    }

    private void FixedUpdate()
    {
        currentState.StateFixedUpdate();
    }

    public void TransitionToState(State desiredState)
    {
        if (desiredState == null)
            return;
        if (currentState != null)
            currentState.Exit();

        previousState = currentState;
        currentState = desiredState;
        currentState.Enter();

        DisplayState();
    }

    private void DisplayState()
    {
        if (previousState == null || previousState.GetType() != currentState.GetType())
        {
            stateName = currentState.GetType().ToString(); 
        }
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
    
    public bool IsGrounded()
    {

        var bounds = boxCollider2D.bounds;
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(bounds.center, bounds.size,
            0f, Vector2.down, .1f, _layerMask);
        Debug.Log(raycastHit2D);
        return raycastHit2D.collider != null;

    }
    
}
