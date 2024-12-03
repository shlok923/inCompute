using UnityEngine;

public interface IObjectParent
{
    Transform ObjectFollowTransform();
    void SetObject(PickupObject pickupObject);
    PickupObject GetObject();
    void ClearObject();
    bool HasObject();
}
