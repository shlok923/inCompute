using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtefactInfo : MonoBehaviour {
    public Player player;
    public InventoryManager artefactInventory;
    public HeldInventoy heldInventory;
    public Image artefactSprite;

    public bool isShowing = false;
    public bool inventoryOpenEarlier = false;

    public void ShowHint() {
        if (isShowing) return;

        Artefact artefactToShow = heldInventory.slot.artefact;
        Debug.Log(">> " + artefactToShow.ToString());
        if (artefactToShow == null) return;
        Debug.Log("> " + artefactToShow.type.ToString());
        if (artefactToShow.type != Artefact.Type.hint) return;

        gameObject.SetActive(true);
        artefactSprite.sprite = artefactToShow.infoSprite;
        isShowing = true;

        player.SetPaused(true);

        if (artefactInventory.isInventoryOpen) {
            inventoryOpenEarlier = true;
            artefactInventory.gameObject.SetActive(false);
            artefactInventory.ResetButtonSprite();
            artefactInventory.isInventoryOpen = false;
        } else {
            inventoryOpenEarlier = false;
            artefactInventory.GetCardHolder().ToggleInteractivity();
        }

        artefactInventory.inventoryToggle.enabled = false;
    }
}
