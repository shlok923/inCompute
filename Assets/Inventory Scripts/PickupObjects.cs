using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupObjects : Interactable
{
    public GameObject artefact;
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

    private void PickUp()
    {
        if (inventoryManager.PickupArtefact(artefact))
        {
            Destroy(gameObject);

        };
    }

}
