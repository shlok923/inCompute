using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string interactableName;

    public virtual void Interact(Player player)
    {
        Debug.Log($"Interacting with {interactableName}");
    }
}
