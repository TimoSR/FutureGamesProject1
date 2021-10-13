using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{

    public State MoveState; 
    
    protected override void EnterState()
    {
        _agent.animationManager.PlayAnimation(AnimationType.idle);
    }

    protected override void HandleMovement(Vector2 input)
    {
        if (Mathf.Abs(input.x) > 0)
        {
            _agent.TransitionToState(MoveState);
        }
    }
}
