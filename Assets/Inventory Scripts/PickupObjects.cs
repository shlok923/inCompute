using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupObjects : Interactable
{
    public ArtefactManager artefact;
    [SerializeField] private InventoryManager inventoryManager;

    private void Start()
    {
        //inventoryManager = FindFirstObjectByType<InventoryManager>();
    }

    public override void Interact(Player player)
    {
        base.Interact(player);
        PickUp();
    }

    public bool PickUp()
    {
        if (inventoryManager.PickupArtefact(artefact.gameObject))
        {
            ShowArtefactInfo();
            Destroy(gameObject);
            return true;
        };

        return false;
    }

    private void ShowArtefactInfo()
    {
        Debug.Log("Picked up " + artefact.artefact.name);
    }

}
