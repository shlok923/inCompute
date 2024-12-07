using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static System.TimeZoneInfo;

public class CardMovement : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {
    public enum CardStates {
        idle,
        hover,
        info,
        play
    }

    public CardStates cardState = CardStates.idle;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Quaternion originalRotation;

    public bool canInteract = true;
    private CardHolder holder;
    private CardPlayManager playManager;
    private CardInfoManager infoManager;

    public RectTransform bounds;
    public Canvas UI;

    public GameObject highlight;
    public Transform hoverPlaceholder;

    public Vector3 descriptionOffset;
    public Vector3 descriptionScale;
    public GameObject descriptionBox;
    private GameObject instantiatedDescriptionBox;

    private float elapsedTime = 0f;
    public float transitionTimer = 0f;
    public float transitionDuration = 0.5f;
    public bool canTransition = true;
    public bool isHovered = false;

    private void Awake() {
        originalPosition = bounds.localPosition;
        originalScale = bounds.localScale;
        originalRotation = transform.localRotation;

        Debug.Log(transform.position);

        holder = GetComponentInParent<CardHolder>();
        playManager = GetComponentInParent<CardPlayManager>();
        infoManager = GetComponentInParent<CardInfoManager>();

        UI.overrideSorting = true;
        UI.sortingOrder = 1;
    }

    private void Update() {
        if (instantiatedDescriptionBox) {
            instantiatedDescriptionBox.transform.rotation = Quaternion.identity;
            instantiatedDescriptionBox.transform.localPosition = hoverPlaceholder.localPosition;
        }

        if (canTransition) {
            if (isHovered && transitionTimer < transitionDuration) {
                transitionTimer += Time.deltaTime;
            } else if (!isHovered && transitionTimer > 0f) {
                transitionTimer -= Time.deltaTime;
            } else if (!isHovered) {
                return;
            }

            // Clamp the timer to ensure it's between 0 and transitionDuration
            transitionTimer = Mathf.Clamp(transitionTimer, 0f, transitionDuration);

            Transition(transitionTimer / transitionDuration);
            //elapsedTime += Time.deltaTime;
        }
    }

    private void UpdateState() {
        switch (cardState) {
            case CardStates.hover:
                HoverState();
                break;
            case CardStates.info:
                InfoState();
                break;
            default:
                IdleState();
                break;
        }
    }

    public void IdleState() {
        highlight.SetActive(false);
        isHovered = false;

        //bounds.localPosition = originalPosition;
        //bounds.localScale = originalScale;
        //transform.localRotation = originalRotation;

        if (instantiatedDescriptionBox != null) {
            Destroy(instantiatedDescriptionBox);
            instantiatedDescriptionBox = null;
        }

        UI.sortingOrder = 1;
    }

    private void HoverState() {
        highlight.SetActive(true);
        isHovered = true;

        //float previousAngle = originalRotation.eulerAngles.z * Mathf.Deg2Rad;
        //bounds.localPosition = hoverPlaceholder.localPosition.y * new Vector3(-Mathf.Sin(previousAngle), Mathf.Cos(previousAngle), 0f);
        //bounds.localPosition = hoverPlaceholder.localPosition;
        //bounds.localScale = hoverPlaceholder.localScale.magnitude / Mathf.Sqrt(3) * originalScale;
        //transform.localRotation = Quaternion.identity;

        instantiatedDescriptionBox = Instantiate(descriptionBox, transform.position + descriptionOffset, Quaternion.identity, UI.transform);
        instantiatedDescriptionBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GetComponent<CardImplementation>().card.title;
        instantiatedDescriptionBox.transform.localScale = descriptionScale;

        UI.sortingOrder = 2;
    }

    private void InfoState() {
        highlight.SetActive(false);

        if (instantiatedDescriptionBox != null) {
            Destroy(instantiatedDescriptionBox);
            instantiatedDescriptionBox = null;
        }

        holder.canTransition = false;
        infoManager.ShowInfo(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (cardState == CardStates.idle && canInteract) {
            cardState = CardStates.hover;
            UpdateState();
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (cardState == CardStates.hover && canInteract) {
            cardState = CardStates.idle;
            UpdateState();
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (cardState == CardStates.hover && canInteract) {
            cardState = CardStates.info;
            UpdateState();
        }
    }

    private void Transition(float frame) {
        // Target transform when hovered
        Vector3 targetPosition = hoverPlaceholder.localPosition;
        Quaternion targetRotation = hoverPlaceholder.localRotation;
        Vector3 targetScale = hoverPlaceholder.localScale;

        // Lerp to the target values
        bounds.localPosition = Vector3.Lerp(originalPosition, targetPosition, frame);
        transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, frame);
        bounds.localScale = Vector3.Lerp(originalScale, targetScale, frame);
        //} else {
        //    // Lerp back to the initial values
        //    transform.position = Vector3.Lerp(transform.position, originalPosition, frame);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, frame);
        //    transform.localScale = Vector3.Lerp(transform.localScale, originalScale, frame);
        //}
    }
}
