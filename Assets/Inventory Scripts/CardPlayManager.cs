using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayManager : MonoBehaviour {
    private CardHolder holder;
    private Player player;
    private InventoryManager inventory;

    private Card cardInHand;
    private GameObject playCard;
    private int playIndex;

    private GameObject infoOverlay;
    private Transform positionPlaceholder;

    public List<Mirror> mirrorStateOne;
    public List<Mirror> mirrorStateTwo;
    public List<PlaceObjects> crystalPlaceholders;
    public GameObject GPUManager;

    public MazeGenerator mazeGenerator;

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

        cardInHand = playCard.GetComponent<CardImplementation>().card;
        if (cardInHand.behaviour is ColorBehaviour colorCard) {
            colorCard.SetMirrors(mirrorStateOne, mirrorStateTwo, crystalPlaceholders, GPUManager);
        } else if (cardInHand.behaviour is FreezeBehaviour freezeCard) {
            freezeCard.SetMaze(mazeGenerator);
        }

        if (!cardInHand.behaviour.canUse()) {
            GetComponent<CardInfoManager>().ResetInfoCard();
            return;
        }

        if (!playCard) return;

        infoOverlay.SetActive(false);
        holder.DeleteCard(playIndex);
        holder.canTransition = true;
        StartCoroutine(PlayVisuals());
    }

    private IEnumerator PlayVisuals() {
        // Run animations here
        Debug.Log("Animations go here");
        yield return new WaitForSeconds(2);

        Debug.Log("Card Animations Completed!");
        //ColorBehaviour colorBehaviour = cardInHand.behaviour.GetComponent<CardBehaviour>() as ColorBehaviour;
        //Debug.Log(colorBehaviour);
        cardInHand.behaviour.Activate();
        Debug.Log("Card Played Successfully!");

        Destroy(positionPlaceholder.GetChild(0).gameObject);
        player.SetPaused(false);
        inventory.CanOpen(true);
        holder.ToggleInteractivity();

        playCard = null;
        playIndex = -1;
    }
}
