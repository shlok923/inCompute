using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string interactableName;
    public string hoverUIMessage;

    public virtual void Interact(Player player)
    {
        Debug.Log($"Interacting with {interactableName}");
    }

    public virtual void InteractAlternate(Player player)
    {
        Debug.Log($"Interacting alternate with {interactableName}");
    }

    public virtual void ShowMessageHoverUI(string hoverUIMessage)
    {
        Debug.Log($"Hovering over {interactableName}");
    }

    public virtual void HideMessageHoverUI()
    {
        Debug.Log($"Stopped hovering over {interactableName}");
    }
}
