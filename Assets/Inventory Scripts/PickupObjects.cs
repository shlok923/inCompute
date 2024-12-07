using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupObjects : Interactable
{
    public ArtefactManager artefact;
    [SerializeField] private InventoryManager inventoryManager;

    [SerializeField] private Player playerInScene;
    [SerializeField] private bool needsScanner;
    [SerializeField] private GameObject scannerObject;
    [SerializeField] private Vector3 startPosition = new Vector3(0, 2, -0.5f);
    [SerializeField] private Vector3 endPosition = new Vector3(0,-0.5f,-0.5f);
    [SerializeField] private float lerpDuration = 1.0f;

    private void Start()
    {
        //inventoryManager = FindFirstObjectByType<InventoryManager>();
    }

    public override void Interact(Player player)
    {
        base.Interact(player);
        playerInScene = player;
        if (playerInScene != null) PickUp();
        else Debug.Log("Player not found");
    }

    public bool PickUp()
    {
        if (inventoryManager.PickupArtefact(artefact.gameObject))
        {
            if (needsScanner)
            {
                StartCoroutine(PickupWithAnimation());
            }
            else
            {
                Debug.Log("Didn't need scanner");
                ShowArtefactInfo();
                Destroy(gameObject);
            }
            //AudioManager.Instance.PlaySFX(AudioManager.Instance.scan);

            return true;

        };

        return false;
    }

    private void ShowArtefactInfo()
    {
        Debug.Log("Picked up " + artefact.artefact.name);
    }

    private IEnumerator PickupWithAnimation()
    {
        Debug.Log("Performing scanning animation");
        if (scannerObject == null) yield break;

        // Perform the scanning animation
        yield return StartCoroutine(PerformScanningAnimation());

        // Log information and destroy the game object after the animation
        ShowArtefactInfo();
        Destroy(gameObject);

        // Optionally unpause the player
        playerInScene.SetPaused(false);
    }

    private IEnumerator PerformScanningAnimation()
    {
        Debug.Log("Performing scanning animation");
        if (scannerObject == null) yield break;

        // Pause the player
        playerInScene.SetPaused(true);

        // Activate the scanner and set its position
        scannerObject.SetActive(true);
        scannerObject.transform.localPosition = startPosition; // Ensure it's at the correct start position
        //Debug.Log("Scanner starting at: " + scannerObject.transform.localPosition);

        // Move to the end position
        float elapsed = 0f;
        while (elapsed < lerpDuration)
        {
            scannerObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsed / lerpDuration);
            elapsed += Time.deltaTime;
            //Debug.Log("Scanner position during upward motion: " + scannerObject.transform.localPosition);
            yield return null;
        }
        scannerObject.transform.localPosition = endPosition;

        // Move back to the start position
        elapsed = 0f;
        while (elapsed < lerpDuration)
        {
            scannerObject.transform.localPosition = Vector3.Lerp(endPosition, startPosition, elapsed / lerpDuration);
            elapsed += Time.deltaTime;
            //Debug.Log("Scanner position during downward motion: " + scannerObject.transform.localPosition);
            yield return null;
        }
        scannerObject.transform.localPosition = startPosition;

        // Disable the scanner
        scannerObject.SetActive(false);
        playerInScene.SetPaused(false);
        Debug.Log("Scanner animation completed and disabled.");
    }

}
