using UnityEngine;

public class Platform : Interactable, IObjectParent
{
    [SerializeField] private Transform pickUpPoint;
    [SerializeField] private PickupObject heldObject;

    public override void Interact(Player player)
    {
        if (!player.HasObject())
        {
            if (HasObject())
            {
                // If player doesn't have an object, pick up the object
                this.GetObject().SetObjectParent(player);
                this.ClearObject();
            }
            
        }
        else
        {
            if (!HasObject())
            {
                // If player already has an object, take it
                player.GetObject().SetObjectParent(this);
                player.ClearObject();
            }
        }
    }

    public Transform ObjectFollowTransform()
    {
        return pickUpPoint;
    }

    public void SetObject(PickupObject heldObject)
    {
        this.heldObject = heldObject;
    }

    public PickupObject GetObject()
    {
        return heldObject;
    }

    public void ClearObject()
    {
        heldObject = null;
    }

    public bool HasObject()
    {
        return heldObject != null;
    }
}
