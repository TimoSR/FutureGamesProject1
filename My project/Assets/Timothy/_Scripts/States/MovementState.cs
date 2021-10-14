using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MovementState : State
{
    [SerializeField]
    protected MovementData movementData;
    public State IdleState;

    public float acceleration, deAcceleration, maxSpeed;

    private void Awake()
    {
        movementData = GetComponentInParent<MovementData>();
    }

    protected override void EnterState()
    {
        _agent.animationManager.PlayAnimation(AnimationType.run);

        movementData.horizontalMovementDirection = 0;
        movementData.currentSpeed = 0;
        movementData.currentVelocity = Vector2.zero;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        CalculateVelocity();
        SetPlayerVelocity();
        if(Mathf.Abs(_agent.rb2d.velocity.x) < 0.01f)
        {
            _agent.TransitionToState(IdleState);
        }
    }

    protected void SetPlayerVelocity()
    {
        _agent.rb2d.velocity = movementData.currentVelocity;
    }

    protected void CalculateVelocity()
    {
        CalculateSpeed(_agent.agentInput.MovementVector, movementData);
        CalculateHorizontalDirection(movementData);
        movementData.currentVelocity = Vector3.right * movementData.horizontalMovementDirection * movementData.currentSpeed;
        movementData.currentVelocity.y = _agent.rb2d.velocity.y;
    }

    private void CalculateHorizontalDirection(MovementData data)
    {
        if(_agent.agentInput.MovementVector.x > 0)
        {
            data.horizontalMovementDirection = 1;
        }else if (_agent.agentInput.MovementVector.x < 0)
        {
            data.horizontalMovementDirection = -1;
        }
    }

    private void CalculateSpeed(Vector2 movementVector, MovementData data)
    {
        if(Mathf.Abs(movementVector.x) > 0)
        {
            data.currentSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            movementData.currentSpeed -= deAcceleration * Time.deltaTime;
        }
        data.currentSpeed = Mathf.Clamp(movementData.currentSpeed, 0, maxSpeed);
    }
}
