using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
    public List<SlotManager> slots;
    public Artefact[] artefacts;
    private List<bool> occupied;

    public Image inventoryToggle;
    private Sprite inventoryOffSprite;
    public Sprite inventoryOnSprite;

    private Player player;
    private CardHolder cardHolder;
    public bool isInventoryOpen = false;

    public GameObject artefactInfo;
    public List<ArtefactManager> testArtefacts;

    private void Awake() {
        player = FindFirstObjectByType<Player>();
        cardHolder = FindFirstObjectByType<CardHolder>();
        inventoryOffSprite = inventoryToggle.sprite;

        InstantiateSlots();
    }

    private void Start() {
        for (int i = 0; i < testArtefacts.Count; i++) {
            slots[i].AddToSlot(testArtefacts[i]);
        }
    }

    private void InstantiateSlots() {
        slots = new List<SlotManager>();
        occupied = new List<bool>();
        for (int i = 0; i < transform.childCount; i++) {
            slots.Add(transform.GetChild(i).GetComponent<SlotManager>());
            occupied.Add(false);
        }
        artefacts = new Artefact[slots.Count];
    }

    public void CanOpen(bool canOpen) {
        inventoryToggle.gameObject.GetComponent<Button>().interactable = canOpen;
    }

    public void ToggleInventory() {
        if (!isInventoryOpen) {
            gameObject.SetActive(true);
            inventoryToggle.sprite = inventoryOnSprite;

            player.SetPaused(true);
            cardHolder.ToggleInteractivity();
            // Add for keyboard as well

            isInventoryOpen = true;
        } else {
            gameObject.SetActive(false);
            inventoryToggle.sprite = inventoryOffSprite;

            player.SetPaused(false);
            cardHolder.ToggleInteractivity();
            // Add for keyboard as well

            isInventoryOpen = false;
        }
    }

    private void RegisterAddition(GameObject element, int slotIndex) {
        ArtefactManager artefactManager = element.GetComponent<ArtefactManager>();

        artefacts[slotIndex] = artefactManager.artefact;
        slots[slotIndex].AddToSlot(artefactManager);
        artefactManager.state = ArtefactManager.States.idle;

        Destroy(element);
        occupied[slotIndex] = true;
    }

    public void PickupArtefact(GameObject pickupElement) {
        for (int i = 0; i < slots.Count; i++) {
            if (!occupied[i]) {
                RegisterAddition(pickupElement, i);
                return;
            }
        }
        Debug.Log("Inventory Space exceeded!");
    }
}
