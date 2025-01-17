using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    public MessageHoverNew tmpWithBackground;
    public bool remove = false;

    void Start()
    {
        // Example usage: create a message that slides into the canvas
        tmpWithBackground.ShowHoverText("hoduweghfaesbfsegfa\n fbaiu3fgauf");
    }

    void Update()
    {
        if (remove)
        {
            tmpWithBackground.HideHoverText();
        }
    }
}
