using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldInventoy : MonoBehaviour {
    public SlotManager slot;
    public InventoryManager inventoryManager;
    
    public GameObject artefactPrefab;
    public GameObject instantiatedArtefact;

    private void Awake() {
        slot = transform.GetChild(0).GetComponent<SlotManager>();
    }

    public void SetHeld(SlotManager previousSlot) {
        if (instantiatedArtefact != null) ResetHeld();
        if (previousSlot.artefact == null) return;

        instantiatedArtefact = Instantiate(artefactPrefab, transform);
        instantiatedArtefact.GetComponent<ArtefactManager>().artefact = previousSlot.artefact;
        instantiatedArtefact.GetComponent<ArtefactManager>().UpdateStats();

        //inventoryManager.RemoveArtefact(previousSlot);
        slot.AddToSlot(instantiatedArtefact.GetComponent<ArtefactManager>());
    }

    public void ResetHeld() {
        if (instantiatedArtefact == null) return;

        inventoryManager.PickupArtefact(instantiatedArtefact);
        slot.RemoveFromSlot();
        instantiatedArtefact = null;
    }
}
