using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    // Singleton----
    public static Player instance;
    public static Player Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    // ---- pattern

    //public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    //public class OnSelectedCounterChangedEventArgs : EventArgs
    //{
    //    public BaseCounter selectedCounter;
    //}

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] Transform objectHoldPoint;
    [SerializeField] private Camera mainCamera;

    private bool isPaused = false;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private Interactable interactableObject;
    //private KitchenObject kitchenObject;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("more than one player exists");
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        //gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    //private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    //{
    //    if (selectedCounter != null)
    //    {
    //        selectedCounter.InteractAlternate(this);
    //    }
    //}

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (interactableObject != null)
        {
            interactableObject.Interact();
        }
    }

    private void Update()
    {
        if (isPaused)
        {
            return;
        }
        HandleMovement();
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance))
        {
            if (raycastHit.transform.TryGetComponent(out Interactable interactable))
            {
                interactableObject = interactable;
            }
            else
            {
                interactableObject = null;
                //Debug.Log("no interactable object found");
            }
        }
        else
        {
            interactableObject = null;
            //Debug.Log("no interactable object found");
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // Convert input vector to world space relative to the camera
        Vector3 moveDir = mainCamera.transform.right * inputVector.x + mainCamera.transform.forward * inputVector.y;
        moveDir.y = 0f; // Keep movement horizontal

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.4f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        isWalking = moveDir != Vector3.zero;

        // for diagonal movement while collliding with smth
        if (!canMove)
        {
            // Cant move towards moveDir

            // Attempt move towards xDir
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // Can move only on X
                moveDir = moveDirX;
            }
            else
            {
                // Cant move only on X

                // Attempt move on Z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // Can move only on Z
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cant move in any direction
                }
            }
        }

        // Actually moving
        if (canMove)
        {
            transform.position += moveDir * Time.deltaTime * moveSpeed;
        }

        // Smooth rotation
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public void SetPaused(bool isPaused)
    {
        this.isPaused = isPaused;
        Debug.Log("paused: " + isPaused);
    }

    //private void SetSelectedCounter(BaseCounter selectedCounter)
    //{
    //    this.selectedCounter = selectedCounter;
    //    OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    //}

    //public Transform KitchenObjectFollowTransform()
    //{
    //    return kitchenObjectHoldPoint;
    //}

    //public void SetKitchenObject(KitchenObject kitchenObject)
    //{
    //    this.kitchenObject = kitchenObject;
    //}

    //public KitchenObject GetKitchenObject()
    //{
    //    return kitchenObject;
    //}

    //public void ClearKitchenObject()
    //{
    //    kitchenObject = null;
    //}
    //public bool HasKitchenObject()
    //{
    //    return kitchenObject != null;
    //}
}