using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Mirror : Interactable
{

    [SerializeField] private Quaternion stateOne = Quaternion.Euler(0, 45, 0);
    [SerializeField] private Quaternion stateTwo = Quaternion.Euler(0, -45, 0);
    [SerializeField] private float moveDistance = 1f; // Distance the mirror moves up and down
    [SerializeField] private float moveSpeed = 2f;    // Speed of movement

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isUp = false;
    private bool isMoving = false;

    private Quaternion targetRotation;
    private bool isRotating = false;

    public bool canInteract = true;

    private void Start()
    {
        initialPosition = transform.localPosition;
        targetPosition = initialPosition;

        // Ensure the mirror has a collider for laser interaction
        Collider collider = GetComponent<Collider>();
        if (!collider.isTrigger)
        {
            collider.isTrigger = false; // Ensure the collider is not a trigger
        }

        targetRotation = transform.localRotation; // Set the initial target rotation
    }

    private void Update()
    {
        Debug.Log(isMoving);
        // Smoothly move the mirror up and down
        if (isMoving)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.whoosh);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

            // Stop movement once close enough to the target
            if (Vector3.Distance(transform.localPosition, targetPosition) < 0.01f)
            {
                transform.localPosition = targetPosition;
                isMoving = false;
            }
        }

        // Smoothly rotate the mirror between CardStates
        if (isRotating)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.whoosh);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, moveSpeed * Time.deltaTime);

            // Stop rotation once close enough to the target rotation
            if (Quaternion.Angle(transform.localRotation, targetRotation) < 0.01f)
            {
                transform.localRotation = targetRotation;
                isRotating = false;
            }
        }
    }

    public override void Interact(Player player)
    {
        // Toggle the mirror's up/down state
        if (!canInteract) return;
        ToggleUpDown();
    }

    public void ToggleUpDown()
    {
        if (isMoving || isRotating) return; // Prevent toggling while moving or rotating

        isUp = !isUp;
        targetPosition = isUp ? initialPosition + Vector3.up * moveDistance : initialPosition;
        isMoving = true;
    }

    public void ToggleRotationState()
    {
        if (!isUp) return; // Ensure the mirror is up and not currently in motion

        targetRotation = targetRotation == stateOne ? stateTwo : stateOne;
        isRotating = true;
    }
}

