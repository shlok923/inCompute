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
    private bool isUp = true;
    private bool isMoving = false;

    private Quaternion targetRotation;
    private bool isRotating = false;

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition;

        // Ensure the mirror has a collider for laser interaction
        Collider collider = GetComponent<Collider>();
        if (!collider.isTrigger)
        {
            collider.isTrigger = false; // Ensure the collider is not a trigger
        }

        targetRotation = transform.rotation; // Set the initial target rotation
    }

    private void Update()
    {
        // Smoothly move the mirror up and down
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Stop movement once close enough to the target
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }

        // Smoothly rotate the mirror between states
        if (isRotating)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, moveSpeed * Time.deltaTime);

            // Stop rotation once close enough to the target rotation
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.01f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    public override void Interact(Player player)
    {
        // Toggle the mirror's up/down state
        ToggleUpDown();
    }

    private void ToggleUpDown()
    {
        if (isMoving || isRotating) return; // Prevent toggling while moving or rotating

        isUp = !isUp;
        targetPosition = isUp ? initialPosition + Vector3.up * moveDistance : initialPosition;
        isMoving = true;
    }

    public void ToggleRotationState()
    {
        if (!isUp || isMoving || isRotating) return; // Ensure the mirror is up and not currently in motion

        targetRotation = transform.rotation == stateOne ? stateTwo : stateOne;
        isRotating = true;
    }
}

