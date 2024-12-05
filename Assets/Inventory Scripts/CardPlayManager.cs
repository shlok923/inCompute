using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayManager : MonoBehaviour {
    private CardHolder holder;
    private Player player;
    private InventoryManager inventory;

    private GameObject playCard;
    private int playIndex;

    private GameObject infoOverlay;
    private Transform positionPlaceholder;

    private void Awake() {
        holder = GetComponent<CardHolder>();
        player = FindFirstObjectByType<Player>();
        inventory = GetComponent<CardInfoManager>().inventory;

        infoOverlay = GetComponent<CardInfoManager>().infoOverlay;
        positionPlaceholder = holder.playPositioner;
    }

    public void PlayCard() {
        playCard = GetComponent<CardInfoManager>().infoCard;
        playIndex = holder.GetCardIndex(playCard);

        if (!playCard) return;

        infoOverlay.SetActive(false);
        holder.DeleteCard(playIndex);
        StartCoroutine(PlayVisuals());
    }

    private IEnumerator PlayVisuals() {
        // Run animations here
        yield return new WaitForSeconds(2);

        Destroy(positionPlaceholder.GetChild(0).gameObject);
        player.SetPaused(false);
        inventory.CanOpen(true);
        holder.ToggleInteractivity();

        playCard = null;
        playIndex = -1;
    }
}
