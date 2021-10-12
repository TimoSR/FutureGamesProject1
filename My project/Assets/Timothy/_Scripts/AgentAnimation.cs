using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AgentAnimation : MonoBehaviour
{

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimation(AnimationType animationType)
    {
        switch (animationType)
        {
            case AnimationType.die:
                Play("Die");
                break;
            case AnimationType.hit:
                Play("Hit");
                break;
            case AnimationType.idle:
                Play("Idle");
                break;
            case AnimationType.attack:
                Play("Attack");
                break;
            case AnimationType.run:
                Play("Run");
                break;
            case AnimationType.jump:
                Play("Jump");
                break;
            case AnimationType.fall:
                Play("Fall");
                break;
            case AnimationType.climb:
                Play("Climb");
                break;
            case AnimationType.land:
                Play("Land");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(animationType), animationType, null);
        }
    }
    
    public void Play(string name)
    {
        _animator.Play(name, -1, 0f);
    }
    
}

public enum AnimationType
{
    die,
    hit,
    idle,
    attack,
    run,
    jump,
    fall,
    climb,
    land
}