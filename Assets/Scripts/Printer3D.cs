using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer3D : Interactable
{
    //[SerializeField] Artefact artefactForSlot;
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] private HeldInventoy heldInventoy;
    [SerializeField] private PickupObjects[] pickupObjects;
    private PickupObjects currentArtefact;
    private bool objectMade = false;

    public override void Interact(Player player)
    {
        base.Interact(player);
        MakeArtefact();
    }

    private void MakeArtefact()
    {
        //if (objectMade)
        //{
        //    Debug.Log("Artefact already exists, pick that up first.");
        //    return;
        //}

        Debug.Log("Recieving blueprint...");
        SlotManager slot = heldInventoy.slot;

        foreach(PickupObjects pickupObject in pickupObjects)
        {
            if (pickupObject == null) continue;
            if ("Broken " + pickupObject.artefact.artefact.artefactName == slot.artefact.artefactName)
            {
                Debug.Log("Blueprint found");

                AudioManager.Instance.PlaySFX(AudioManager.Instance.electricSpark);
                ArtefactInInventory(pickupObject);
                slot.RemoveFromSlot();
                heldInventoy.instantiatedArtefact = null;
                //pickupObject.gameObject.SetActive(true);
                //objectMade = true;
                return;
            }
            else
            {
                Debug.Log("Wanted " + "Broken " + pickupObject.artefact.artefact.name + "but got " + slot.artefact.artefactName);

            }
        }
        //if (slot.artefact == artefactForSlot)
        //{
        //    Debug.Log("Object found");
        //    slot.RemoveFromSlot();
        //    artefactObject.SetActive(true);
        //    return;
        //}
        Debug.Log("Object not found");
    }

    private void ArtefactInInventory(PickupObjects currentArtefact)
    {
        if (currentArtefact.PickUp())
        {
            return;
        };

        Debug.Log("Called to put artefact in inventory but failed!");
    }

    //public bool IsObjectPlaced()
    //{
    //    return objectPlaced;
    //}


}

