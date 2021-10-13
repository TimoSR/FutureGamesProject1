using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{

    [field: SerializeField]
    public Vector2 MovementVector { get; private set; }

    public event Action OnAttack, OnJumpPressed, OnJumpReleased, OnWeaponChange;

    public event Action<Vector2> OnMovement;

    public KeyCode weaponSwap = KeyCode.E, jumpKey = KeyCode.Space, attackKey = KeyCode.Mouse0, menuKey = KeyCode.Escape;

    public UnityEvent onMenuKeyPressed;
    
    
    // Update is called once per frame
    void Update()
    {
        
        // Input only listened to if Menu is closed 
        if (Time.timeScale > 0)
        {
            GetMovementInput();
            GetJumpInput();
            GetAttackInput();
            GetWeaponSwapInput();
        }
        
        //GetMenuInput();

    }

    private void GetMenuInput()
    {
        throw new NotImplementedException();
    }

    private void GetWeaponSwapInput()
    {
        if (Input.GetKeyDown(weaponSwap))
        {
            OnWeaponChange?.Invoke();
        }
    }

    private void GetAttackInput()
    {
        if (Input.GetKey(attackKey))
        {
            OnAttack?.Invoke();
        }
    }

    private void GetJumpInput()
    {
        if (Input.GetKey(jumpKey))
        {
            OnJumpPressed?.Invoke();
        }

        if (Input.GetKey(jumpKey))
        {
            OnJumpReleased?.Invoke();
        }
    }

    private void GetMovementInput()
    {
        MovementVector = GetMovementVector();
        OnMovement?.Invoke(MovementVector);
    }

    private Vector2 GetMovementVector()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

}
