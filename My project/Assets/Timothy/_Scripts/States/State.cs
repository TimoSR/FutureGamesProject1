using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class State : MonoBehaviour
{
    [SerializeField] protected State JumpState;
    
    protected Agent _agent;

    public UnityEvent OnEnter, OnExit;

    public void InitializeState(Agent agent)
    {
        this._agent = agent;
    }

    public void Enter()
    {
        this._agent.agentInput.OnAttack += HandleAttack;
        this._agent.agentInput.OnJumpPressed += HandleJumpPressed;
        this._agent.agentInput.OnJumpReleased += HandleJumpReleased;
        this._agent.agentInput.OnMovement += HandleMovement;
        OnEnter?.Invoke();
        EnterState();
    }

    protected virtual void EnterState()
    {

    }

    protected virtual void HandleMovement(Vector2 obj)
    {
    }

    protected virtual void HandleJumpReleased()
    {
    }

    protected virtual void HandleJumpPressed()
    {
        // TestJumpTransition();
    }

    // private void TestJumpTransition()
    // {
    //     if (_agent.IsGrounded())
    //     {
    //         _agent.TransitionToState(JumpState);
    //     }
    // }

    protected virtual void HandleAttack()
    {
    }

    public virtual void StateUpdate()
    {

    }

    public virtual void StateFixedUpdate()
    {

    }

    public void Exit()
    {
        this._agent.agentInput.OnAttack -= HandleAttack;
        this._agent.agentInput.OnJumpPressed -= HandleJumpPressed;
        this._agent.agentInput.OnJumpReleased -= HandleJumpReleased;
        this._agent.agentInput.OnMovement -= HandleMovement;
        OnExit?.Invoke();
        ExitState();
    }

    protected virtual void ExitState()
    {
    }
}
