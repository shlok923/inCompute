using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    [SerializeField] int numSlots = 18;

    public List<SlotManager> slots;
    public Artefact[] artefacts;
    private List<bool> occupied;

    public Image inventoryToggle;
    private Sprite inventoryOffSprite;
    public Sprite inventoryOnSprite;

    private Player player;
    private CardHolder cardHolder;
    public bool isInventoryOpen = false;

    public ArtefactInfo artefactInfo;
    public HeldInventoy heldInventory;
    //public List<ArtefactManager> testArtefacts;

    private void Awake() {
        player = FindFirstObjectByType<Player>();
        cardHolder = FindFirstObjectByType<CardHolder>();
        inventoryOffSprite = inventoryToggle.sprite;

        InstantiateSlots();
    }

    //private void Start() {
    //    for (int i = 0; i < testArtefacts.Count; i++) {
    //        if (testArtefacts[i] == null) continue;
    //        slots[i].AddToSlot(testArtefacts[i]);
    //        occupied[i] = true;
    //    }
    //}

    private void InstantiateSlots() {
        slots = new List<SlotManager>();
        occupied = new List<bool>();
        Debug.Log("helo");
        for (int i = 0; i < numSlots; i++) {
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
        if (artefactManager.artefact == null) return;

        Debug.Log(">> " + slotIndex);

        artefacts[slotIndex] = artefactManager.artefact;
        slots[slotIndex].AddToSlot(artefactManager);

        Destroy(element);
        occupied[slotIndex] = true;
    }

    public void RemoveArtefact(SlotManager slot) {
        int slotIndex = GetIndex(slot);

        slot.RemoveFromSlot();
        Debug.Log(slotIndex);
        artefacts[slotIndex] = null;
        occupied[slotIndex] = false;
    }
    
    private int GetIndex(SlotManager slot) {
        for (int i = 0; i < slots.Count; i++) {
            if (slots[i] == slot) return i;
        }
        return -1;
    }

    public bool PickupArtefact(GameObject pickupElement) {
        if (pickupElement == null) return false;

        for (int i = 0; i < slots.Count; i++) {
            if (!occupied[i]) {
                RegisterAddition(pickupElement, i);
                ShowInventory();
                return true;
            }
        }

        Debug.Log("Inventory Space exceeded!");
        return false;
    }

    public void ShowInventory() {
        Debug.Log("Checking Inventory...");
        for (int i = 0; i < slots.Count; i++) {
            if (occupied[i]) {
                Debug.Log(i + ": " + artefacts[i].ToString() + " = " + slots[i].artefact.ToString());
            } else {
                Debug.Log(i + ": " + null);
            }
        }
    }

    public void CloseInfo() {
        artefactInfo.gameObject.SetActive(false);
        artefactInfo.artefactSprite = null;

        player.SetPaused(false);
        artefactInfo.isShowing = false;

        if (artefactInfo.inventoryOpenEarlier) {
            gameObject.SetActive(true);
            inventoryToggle.sprite = inventoryOnSprite;
            isInventoryOpen = true;
        } else {
            cardHolder.ToggleInteractivity();
        }

        inventoryToggle.enabled = true;
    }

    public void ResetButtonSprite() {
        inventoryToggle.sprite = inventoryOffSprite;
    }

    public CardHolder GetCardHolder() {
        return cardHolder;
    }
}
