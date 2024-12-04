using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPickup : Interactable {
    public CardHolder holder;
    public Card cardStored;
    public Transform peekPlaceholder;

    public bool beingPeeked = false;

    public override void Interact(Player player) {
        if (cardStored == null) return;

        holder.AddCard(cardStored);

        if (beingPeeked) UnpeekCard();
        cardStored = null;
    }

    public void PeekCard() {
        if (cardStored == null || beingPeeked) return;
        GameObject peekCard = holder.GenerateCard(cardStored, peekPlaceholder.position, peekPlaceholder);
        peekCard.GetComponent<CardMovement>().canInteract = false;
    }

    public void UnpeekCard() {
        if (cardStored == null) return;
        Destroy(peekPlaceholder.GetChild(0).gameObject);
    }
}
