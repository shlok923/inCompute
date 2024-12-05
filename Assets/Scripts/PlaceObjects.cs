using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjects : Interactable
{
    [SerializeField] Artefact artefactForSlot;
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] private GameObject artefactObject;
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
        List<SlotManager> slots = inventoryManager.slots;

        foreach (SlotManager slot in slots)
        {
            if (slot.artefact == artefactForSlot)
            {
                Debug.Log("Object found");
                slot.RemoveFromSlot();
                artefactObject.SetActive(true);
                return;
            }
        }
        Debug.Log("Object not found");
    }

    public bool IsObjectPlaced()
    {
        return objectPlaced;
    }   


}
