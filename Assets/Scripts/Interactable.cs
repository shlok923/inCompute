using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public InteractableSO interactableData;

    public virtual void Interact()
    {
        if (interactableData != null)
        {
            Debug.Log("Interacting with: " + interactableData.objectName);

            if (interactableData.triggersEvent)
            {
                Debug.Log("Triggering event: " + interactableData.objectName);
                // trigger some event
            }

            if (interactableData.isPickable)
            {
                Debug.Log("Picking up: " + interactableData.objectName);
                // pickup or move to inventory type shit
            }
        }
        else
        {
            Debug.Log("int data null!");
        }
    }
}