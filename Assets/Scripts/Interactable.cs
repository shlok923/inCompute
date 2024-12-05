using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string interactableName;

    public virtual void Interact(Player player)
    {
        Debug.Log($"Interacting with {interactableName}");
    }

    public virtual void InteractAlternate(Player player)
    {
        Debug.Log($"Interacting alternate with {interactableName}");
    }
}
