using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialInteractor : Interactable {

    public PowerSupplyManager powerSupplyManager;
    private float currentAngle;
    private float originalAngle;
    public float turnAngle = 45f;
    private float turnEndAngle;
    public float targetAngle;

    private float initialYRotation;
    private float initialZRotation;

    private bool beingTurned = false;
    public float rotationTime = 2f;
    private float elapsedTime = 0f;

    private void Awake() {
        originalAngle = transform.eulerAngles.x;
        currentAngle = transform.eulerAngles.x;
        initialYRotation = transform.eulerAngles.y;
        initialZRotation = transform.eulerAngles.z;
    }

    private void FixedUpdate() {
        if (beingTurned) {

            RotateDial(elapsedTime / rotationTime);
            elapsedTime += Time.fixedDeltaTime;
        }
    }

    public override void Interact(Player player) {
        turnEndAngle = (currentAngle - turnAngle) % 360;
        beingTurned = true;
        powerSupplyManager.OnDialConfigurationChanged(this);
    }

    private void RotateDial(float frame) {
        Quaternion currentRotation = Quaternion.Euler(currentAngle, initialYRotation, initialZRotation);
        Quaternion endRotation = Quaternion.Euler(turnEndAngle, initialYRotation, initialZRotation);
        transform.rotation = Quaternion.Slerp(currentRotation, endRotation, frame);

        if (frame >= 1) {
            currentAngle = Mathf.Ceil(turnEndAngle);
            beingTurned = false;
            elapsedTime = 0f;
        }
    }

    // Function to reset the dial's angle to the original angle
    public void ResetToOriginalAngle()
    {
        if (!beingTurned) // Prevent resetting while the dial is being turned
        {
            StartCoroutine(ResetDialCoroutine());
        }
    }

    private IEnumerator ResetDialCoroutine()
    {
        beingTurned = true; // Block other interactions during reset
        float resetElapsedTime = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(originalAngle, initialYRotation, initialZRotation);

        while (resetElapsedTime < rotationTime)
        {
            float progress = resetElapsedTime / rotationTime;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, progress);
            resetElapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; // Ensure it ends at the exact target
        currentAngle = originalAngle; // Update current angle to reflect reset
        beingTurned = false;
    }

    public void TurnDialMultipleTimes(int numberOfTurns)
    {
        if (!beingTurned) // Ensure the dial is not already being turned
        {
            StartCoroutine(TurnDialCoroutine(numberOfTurns));
        }
    }

    private IEnumerator TurnDialCoroutine(int numberOfTurns)
    {
        for (int i = 0; i < numberOfTurns; i++)
        {
            turnEndAngle = (currentAngle - turnAngle) % 360;
            beingTurned = true;
            powerSupplyManager.OnDialConfigurationChanged(this);

            elapsedTime = 0f;

            while (beingTurned) // Wait for the current turn to complete
            {
                yield return null;
            }
        }
    }


    public bool hasCorrectConfiguration() {
        return currentAngle == -targetAngle;
    }
}
