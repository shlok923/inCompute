using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public string message;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MessageHover._instance.ShowMessage(message);
            Debug.Log("Player has entered the box collider!");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MessageHover._instance.HideMessage();
            Debug.Log("Player has exited the box collider!");
        }
    }
}