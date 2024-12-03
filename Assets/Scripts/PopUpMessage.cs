using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public string message;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MessageHover.IsTriggered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MessageHover.IsTriggered = false;
        }
    }
}
