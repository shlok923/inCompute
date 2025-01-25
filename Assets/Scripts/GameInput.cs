using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnMirrorRotationAction; 
    public event EventHandler OnInventoryToggle;


    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.RotateMirror.performed += RotateMirror_performed;
        //playerInputActions.Player./
        playerInputActions.Player.InventoryToggle.performed += InventoryToggle_performed;
    }

    private void InventoryToggle_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInventoryToggle?.Invoke(this, EventArgs.Empty);
    }

    private void RotateMirror_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMirrorRotationAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        return inputVector;
    }
}
