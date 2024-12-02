using UnityEngine;

[CreateAssetMenu(fileName = "NewInteractable", menuName = "Interactable Object")]
public class InteractableSO : ScriptableObject
{
    public string objectName;
    public string description;
    public Transform prefab;
    //public Sprite icon;
    public bool isPickable;
    public bool triggersEvent;
}
