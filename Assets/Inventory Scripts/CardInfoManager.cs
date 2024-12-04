using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfoManager : MonoBehaviour {
    private CardHolder holder;

    public GameObject infoCard;
    private int infoIndex;

    public GameObject infoOverlay;
    private Transform positionPlaceholder;

    private void Awake() {
        holder = GetComponent<CardHolder>();
        positionPlaceholder = holder.playPositioner;
    }

    public void ShowInfo(GameObject card) {
        if (infoCard) ResetInfoCard();

        infoCard = card;
        infoIndex = holder.GetCardIndex(card);

        holder.ToggleInteractivity();
        Instantiate(card, positionPlaceholder);
        infoOverlay.SetActive(true);
        holder.HideCard(infoIndex);
    }

    public void ResetInfoCard() {
        holder.UnhideCard(infoIndex);
        holder.cards[infoIndex].GetComponent<CardMovement>().cardState = CardMovement.CardStates.idle;
        holder.cards[infoIndex].GetComponent<CardMovement>().IdleState();
        
        infoOverlay.SetActive(false);
        Destroy(positionPlaceholder.GetChild(0).gameObject);
        holder.ToggleInteractivity();

        infoCard = null;
        infoIndex = -1;
    }
}
