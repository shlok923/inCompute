using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Transform cardPositioner;
    public Transform playPositioner;
    public float horizontalSpacing;

    public GameObject cardPrefab;
    public List<GameObject> cards;

    public List<Card> testCards;

    private float elapsedTime = 0f;
    public float transitionDuration = 0.5f;
    public bool canTransition = true;
    public bool isTransitioning = false;

    private float originalWidth;
    public float requiredWidthF = 20f;
    private float requiredWidthB;
    private float requiredW;

    private float originalPosition;
    public float requiredPositionF = 640f;
    private float requiredPositionB;
    private float requiredP;

    private float originalSpacing;
    public float requiredSpacingF = 3.5f;
    private float requiredSpacingB;
    private float requiredS;
    
    private void Awake() {
        cards = new List<GameObject>();
        requiredWidthB = GetComponent<RectTransform>().rect.width;
        requiredPositionB = GetComponent<RectTransform>().anchoredPosition.x;
        requiredSpacingB = horizontalSpacing;
    }

    private void Start() {
        for (int i = 0; i < testCards.Count; i++) {
            AddCard(testCards[i]);
        }
    }

    private void Update() {
        UpdateVisuals();
        if (isTransitioning) {
            Transition(requiredW, requiredP, requiredS, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
        }
    }

    public GameObject GenerateCard(Card card, Vector3 position, Transform parent) {
        GameObject newCard = Instantiate(cardPrefab, position, Quaternion.identity, parent);
        CardImplementation cardManager = newCard.GetComponent<CardImplementation>();

        cardManager.card = card;
        cardManager.UpdateStats();
        return newCard;
    }

    public void AddCard(Card card) {
        GameObject newCard = GenerateCard(card, cardPositioner.position, transform);
        cards.Add(newCard);
    }

    public void DeleteCard(int cardIndex) {
        GameObject cardToDelete = cards[cardIndex];
        cards.Remove(cardToDelete);
        Destroy(cardToDelete);
    }

    public void HideCard(int cardIndex) {
        cards[cardIndex].gameObject.SetActive(false);
    }

    public void UnhideCard(int cardIndex) {
        cards[cardIndex].gameObject.SetActive(true);
    }

    public int GetCardIndex(GameObject card) {
        for (int i = 0; i < cards.Count; i++) {
            if (cards[i] == card) return i;
        }
        return -1;
    }

    public void ToggleInteractivity() {
        for (int i = 0; i < cards.Count; i++) {
            cards[i].GetComponent<CardMovement>().canInteract ^= true;
        }
    }

    private void UpdateVisuals() {
        if (cards.Count == 1) {
            cards[0].transform.localPosition = Vector3.zero;
            return;
        }

        for (int i = 0; i < cards.Count; i++) {
            float horizontalOffset = horizontalSpacing * (i - (cards.Count - 1) / 2f);
            Vector3 cardPosition = new Vector3(horizontalOffset, 0f, 0f);
            cards[i].transform.localPosition = cardPosition;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!canTransition) return;

        ResetParams();
        Debug.Log("Entered Transition Forward");

        requiredW = requiredWidthF;
        requiredP = requiredPositionF;
        requiredS = requiredSpacingF;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!canTransition) return;

        ResetParams();
        Debug.Log("Entered Transition Backward");

        requiredW = requiredWidthB;
        requiredP = requiredPositionB;
        requiredS = requiredSpacingB;
    }

    private void Transition(float requiredWidth, float requiredPosition, float requiredSpacing, float frame) {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(originalWidth * (1 - frame) + requiredWidth * frame, rectTransform.sizeDelta.y);
        rectTransform.anchoredPosition = new Vector2(originalPosition * (1 - frame) + requiredPosition * frame, rectTransform.anchoredPosition.y);
        horizontalSpacing = originalSpacing * (1 - frame) + requiredSpacing * frame;

        if (frame >= 1) {
            elapsedTime = 0f;
            isTransitioning = false;
            Debug.Log("Exiting Transition");
            return;
        }
    }

    private void ResetParams() {
        originalWidth = GetComponent<RectTransform>().rect.width;
        originalPosition = GetComponent<RectTransform>().anchoredPosition.x;
        originalSpacing = horizontalSpacing;
        elapsedTime = 0f;
        isTransitioning = true;
    }
}
