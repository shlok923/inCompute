using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderInteractable : Interactable
{

    public override void Interact()
    {
        base.Interact();
        Debug.Log("climbing ladder");
        
    }
}
