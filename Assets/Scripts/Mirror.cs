using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Mirror : MonoBehaviour
{

    [SerializeField] private Quaternion stateOne;
    [SerializeField] private Quaternion stateTwo;

    private void Start()
    {
        // Ensure the mirror has a collider for laser interaction
        Collider collider = GetComponent<Collider>();
        if (!collider.isTrigger)
        {
            collider.isTrigger = false; // Make sure the collider is not a trigger
        }
    }

    private void ChangeState()
    {
        if (transform.rotation == stateOne)
        {
            transform.rotation = stateTwo;
        }
        else
        {
            transform.rotation = stateOne;
        }
    }
}

