using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardHolder : MonoBehaviour {
    public Transform cardPositioner;
    public Transform playPositioner;
    public float horizontalSpacing;

    public GameObject cardPrefab;
    public List<GameObject> cards;

    public List<Card> testCards;
    
    private void Awake() {
        cards = new List<GameObject>();
    }

    private void Start() {
        for (int i = 0; i < testCards.Count; i++) {
            AddCard(testCards[i]);
        }
    }

    private void Update() {
        UpdateVisuals();
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
}
