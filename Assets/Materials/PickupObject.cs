using UnityEngine;

public class PickupObject : MonoBehaviour
{
    private IObjectParent objectParent;

    public void SetObjectParent(IObjectParent newParent)
    {
        if (this.objectParent != null)
        {
            objectParent.ClearObject();
        }

        this.objectParent = newParent;

        if (newParent != null)
        {
            newParent.SetObject(this);
            transform.SetParent(newParent.ObjectFollowTransform());
            transform.localPosition = Vector3.zero; // Align with the parent
        }
    }

    public IObjectParent GetObjectParent()
    {
        return objectParent;
    }
}
