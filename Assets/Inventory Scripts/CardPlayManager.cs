using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayManager : MonoBehaviour {
    private CardHolder holder;
    private GameObject playedCard = null;
    public Transform playPlaceholder;

    private void Awake() {
        holder = GetComponent<CardHolder>();
    }

    private void Update() {
        CheckPlay();
        if (playedCard) {
            PlayCard();
        }
    }

    private void PlayCard() {
        TogglePlayability();
        GameObject instantiatedCard = Instantiate(playedCard, playPlaceholder.position, Quaternion.identity, playPlaceholder);

        instantiatedCard.GetComponent<CardMovement>().UI.enabled = true;
        StartCoroutine(PlayAnimation(instantiatedCard));
    }

    private void CheckPlay() {
        List<GameObject> cards = holder.cards;
        for (int i = 0; i < cards.Count; i++) {
            if (cards[i].GetComponent<CardMovement>().cardState == CardMovement.CardStates.play) {
                playedCard = cards[i];
                holder.DeleteCard(i);
                break;
            }
        }
    }

    private void TogglePlayability() {
        List<GameObject> cards = holder.cards;
        for (int i = 0; i < cards.Count; i++) {
            if (cards[i] == playedCard) continue;
            cards[i].GetComponent<CardMovement>().isPlayable ^= true;
        }
    }

    private IEnumerator PlayAnimation(GameObject cardPlayed) {
        yield return new WaitForSeconds(2);

        Destroy(cardPlayed);
        playedCard = null;
        TogglePlayability();
    }
}
