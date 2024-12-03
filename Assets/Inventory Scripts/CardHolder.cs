using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardHolder : MonoBehaviour {
    public Transform cardPositioner;
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

    public void AddCard(Card card) {
        GameObject newCard = Instantiate(cardPrefab, cardPositioner.position, Quaternion.identity, cardPositioner);
        CardImplementation cardManager = newCard.GetComponent<CardImplementation>();

        cardManager.card = card;
        cardManager.UpdateStats();
        cards.Add(newCard);
    }

    public void DeleteCard(int cardIndex) {
        GameObject cardToDelete = cards[cardIndex];
        cards.Remove(cardToDelete);
        Destroy(cardToDelete);
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
