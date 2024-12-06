using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private void Awake() {
        originalPosition = bounds.localPosition;
        originalScale = bounds.localScale;

        holder = GetComponentInParent<CardHolder>();
        playManager = GetComponentInParent<CardPlayManager>();
        infoManager = GetComponentInParent<CardInfoManager>();

        UI.overrideSorting = true;
        UI.sortingOrder = 1;
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
        GetComponent<CardImplementation>().background.SetActive(true);

        bounds.localPosition = originalPosition;
        bounds.localScale = originalScale;
       
        if (instantiatedDescriptionBox != null) {
            Destroy(instantiatedDescriptionBox);
            instantiatedDescriptionBox = null;
        }

        UI.sortingOrder = 1;
    }

    private void HoverState() {
        highlight.SetActive(true);
        GetComponent<CardImplementation>().background.SetActive(false);

        bounds.localPosition = hoverPlaceholder.localPosition;
        bounds.localScale = hoverPlaceholder.localScale.magnitude / Mathf.Sqrt(3) * originalScale;

        instantiatedDescriptionBox = Instantiate(descriptionBox, transform.position + descriptionOffset, Quaternion.identity, UI.transform);
        instantiatedDescriptionBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GetComponent<CardImplementation>().card.title;
        instantiatedDescriptionBox.transform.localScale = descriptionScale;

        UI.sortingOrder = 2;
    }

    private void InfoState() {
        highlight.SetActive(false);
        GetComponent<CardImplementation>().background.SetActive(false);

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
}
