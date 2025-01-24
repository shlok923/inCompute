using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjects : Interactable
{
    [SerializeField] Artefact artefactForSlot;
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] private GameObject artefactObject;
    [SerializeField] private HeldInventoy heldInventoy;
    private bool objectPlaced = false;

    public override void Interact(Player player)
    {
        base.Interact(player);
        PlaceObject();
    }

    public override void ShowMessageHoverUI(string hoverUIMessage)
    {
        base.ShowMessageHoverUI(hoverUIMessage);
        UIManager.Instance.ShowHoverUI(hoverUIMessage);
    }

    public override void HideMessageHoverUI()
    {
        base.HideMessageHoverUI();
        UIManager.Instance.HideHoverUI();
    }

    private void PlaceObject()
    {
        if (objectPlaced)
        {
            Debug.Log("Object already placed");
            return;
        }

        Debug.Log("Placing object");
        SlotManager slot = heldInventoy.slot;
        Debug.Log(slot.artefact);

        // place object if held object supposed to be here
        if (slot.artefact == artefactForSlot)
        {
            Debug.Log("Object found");
            AudioManager.Instance.PlaySFX(AudioManager.Instance.plugIn);
            slot.RemoveFromSlot();
            heldInventoy.instantiatedArtefact = null;
            artefactObject.SetActive(true);
            objectPlaced = true;
            return;
        }
        Debug.Log("Object not found");
    }

    public bool IsObjectPlaced()
    {
        return objectPlaced;
    }

}
