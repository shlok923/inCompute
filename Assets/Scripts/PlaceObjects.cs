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

        if (slot.artefact == artefactForSlot)
        {
            Debug.Log("Object found");
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
