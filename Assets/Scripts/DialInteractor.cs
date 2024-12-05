using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialInteractor : Interactable {
    private float currentAngle;
    public float turnAngle = 45f;
    private float turnEndAngle;
    public float targetAngle;

    private float initialYRotation;
    private float initialZRotation;

    private bool beingTurned = false;
    public float rotationTime = 2f;
    private float elapsedTime = 0f;

    private void Awake() {
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

    public bool hasCorrectConfiguration() {
        return currentAngle == -targetAngle;
    }
}
