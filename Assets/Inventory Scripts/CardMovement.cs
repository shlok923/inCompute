using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMovement : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {
    public enum CardStates {
        idle,
        hover,
        play
    }

    public CardStates cardState = CardStates.idle;
    public GameObject highlight;
    public Transform hoverPlaceholder;

    public RectTransform bounds;
    public Canvas UI;

    private Vector3 originalPosition;
    private Vector3 originalScale;

    public bool isPlayable = true;

    private void Awake() {
        originalPosition = bounds.localPosition;
        originalScale = bounds.localScale;

        UI.overrideSorting = true;
        UI.sortingOrder = 1;
    }

    private void Update() {
        switch (cardState) {
            case CardStates.hover:
                HoverState();
                break;
            case CardStates.play:
                PlayState();
                break;
            default:
                IdleState();
                break;
        }
    }

    private void IdleState() {
        highlight.SetActive(false);

        bounds.localPosition = originalPosition;
        bounds.localScale = originalScale;

        UI.sortingOrder = 1;
    }

    private void HoverState() {
        highlight.SetActive(true);

        bounds.localPosition = hoverPlaceholder.localPosition;
        bounds.localScale = hoverPlaceholder.localScale.magnitude / Mathf.Sqrt(3) * originalScale;

        UI.sortingOrder = 2;
    }

    private void PlayState() {
        highlight.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (cardState == CardStates.idle && isPlayable) {
            originalPosition = bounds.localPosition;
            originalScale = bounds.localScale;

            cardState = CardStates.hover;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (cardState == CardStates.hover && isPlayable) {
            cardState = CardStates.idle;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (cardState == CardStates.hover && isPlayable) {
            cardState = CardStates.play;
        }
    }
}
