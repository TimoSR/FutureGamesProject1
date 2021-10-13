// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class JumpState : MovementState
// {
//     
//     public float jumpForce = 15;
//     public float lowJumpMultiplier = 2;
//     
//     public State FallState;
//
//     private bool jumpPressed = false;
//
//     protected override void EnterState()
//     {
//         _agent.animationManager.PlayAnimation(AnimationType.jump);
//         movementData.currentVelocity = _agent.rb2d.velocity;
//         movementData.currentVelocity.y = jumpForce;
//         _agent.rb2d.velocity = movementData.currentVelocity;
//         jumpPressed = true;
//     }
//
//     protected override void HandleJumpPressed()
//     {
//         jumpPressed = true;
//     }
//
//     protected override void HandleJumpReleased()
//     {
//         jumpPressed = false;
//     }
//
//     public override void StateUpdate()
//     {
//         ControlJumpHeight();
//         CalculateVelocity();
//         SetPlayerVelocity();
//         if (_agent.rb2d.velocity.y <= 0)
//         {
//             _agent.TransitionToState(FallState);
//         }
//     }
//
//     private void ControlJumpHeight()
//     {
//         if(jumpPressed == false)
//         {
//             movementData.currentVelocity = _agent.rb2d.velocity;
//             movementData.currentVelocity.y += lowJumpMultiplier*Physics2D.gravity.y * Time.deltaTime;
//             _agent.rb2d.velocity = movementData.currentVelocity;
//         }
//     }
// }
