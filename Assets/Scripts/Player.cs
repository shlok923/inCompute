using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IObjectParent
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

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] Transform objectHoldPoint;
    [SerializeField] private Camera mainCamera;

    private bool isPaused = false;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private Interactable interactableObject;
    private Interactable lastInteractableObject;
    [SerializeField] private PickupObject heldObject;
    [SerializeField] private HeldInventoy heldInventory;
    [SerializeField] private ArtefactInfo artefactInfo;
    [SerializeField] private GameObject interactCanvas;

    private void Awake()
    {
        heldInventory.inventoryManager.gameObject.SetActive(true);
        heldInventory.inventoryManager.gameObject.SetActive(false);
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
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
        gameInput.OnMirrorRotationAction += GameInput_OnMirrorRotationAction;
    }

    private void FixedUpdate() {
        if (interactableObject != null) {
            interactCanvas.SetActive(true);
        } else if (interactCanvas.transform.childCount >= 1) {
            interactCanvas.SetActive(false);
        }
    }

    private void GameInput_OnMirrorRotationAction(object sender, EventArgs e)
    {
        if (interactableObject != null && interactableObject.TryGetComponent<Mirror>(out Mirror mirror))
        {
            mirror.ToggleRotationState();
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (interactableObject != null)
        {
            interactableObject.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (interactableObject != null)
        {
            interactableObject.Interact(this);
        } else if (heldInventory.slot.artefact != null) {
            artefactInfo.ShowHint();
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

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y) + mainCamera.transform.forward * inputVector.y;

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        Vector3 rayOffset = new Vector3(0, 0.25f, 0);


        float interactDistance = 2f;
        //Debug.DrawRay(transform.position, transform.forward * 2, Color.red, 0.1f);
        if (Physics.Raycast(transform.position + rayOffset, transform.forward * 2 + rayOffset, out RaycastHit raycastHit, interactDistance))
        {
            if (raycastHit.transform.TryGetComponent(out Interactable interactable))
            {
                interactableObject = interactable;
                interactableObject.ShowMessageHoverUI(interactableObject.hoverUIMessage);
                Debug.Log("interactable object found");
                if (interactableObject is CardPickup cardPickup) {
                    cardPickup.PeekCard();
                    cardPickup.beingPeeked = true;
                }
            }
            else
            {
                if (interactableObject is CardPickup cardPickup) {
                    cardPickup.UnpeekCard();
                    cardPickup.beingPeeked = false;
                }

                interactableObject.HideMessageHoverUI();
                interactableObject = null;
                //Debug.Log("no interactable object found");
            }
        }
        else
        {
            if (interactableObject is CardPickup cardPickup) {
                cardPickup.UnpeekCard();
                cardPickup.beingPeeked = false;
            }
            interactableObject.HideMessageHoverUI();
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

    public Transform ObjectFollowTransform()
    {
        return objectHoldPoint;
    }

    public void SetObject(PickupObject heldObject)
    {
        this.heldObject = heldObject;
    }

    public PickupObject GetObject()
    {
        return heldObject;
    }

    public void ClearObject()
    {
        heldObject = null;
    }

    public bool HasObject()
    {
        return heldObject != null;
    }
}