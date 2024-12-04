using UnityEngine;
using System.Collections.Generic;

public class Crystal : MonoBehaviour
{
    [SerializeField] private MeshRenderer crystalRenderer;
    [SerializeField] private Material originalMaterial; // Reference to the original material.
    private Material initialMaterial; // Keeps track of the current material.
    private HashSet<LaserScript> lasersCurrentlyHitting = new HashSet<LaserScript>(); // Track lasers that are currently hitting the crystal.
    [SerializeField] private Material requiredMaterial; // The material that should light the crystal

    private void Start()
    {
        if (crystalRenderer == null)
        {
            crystalRenderer = GetComponent<MeshRenderer>();
        }

        // Initialize material references.
        initialMaterial = crystalRenderer.material;
        if (originalMaterial == null)
        {
            originalMaterial = initialMaterial; // Default to the starting material.
        }
    }

    // Called when a laser hits the crystal
    public void OnLaserHit(LaserScript laser, Material laserMaterial)
    {
        if (!lasersCurrentlyHitting.Contains(laser)) // Only change material if the laser is not already recorded.
        {
            lasersCurrentlyHitting.Add(laser); // Add the laser to the hit set.
            crystalRenderer.material = laserMaterial; // Change material to the laser's material.
            //Debug.Log("Crystal hit: " + gameObject.name + " applying material: " + laserMaterial.name);
        }
    }

    // Called when a laser stops hitting the crystal
    public void OnLaserExit(LaserScript laser)
    {
        if (lasersCurrentlyHitting.Contains(laser)) // Only reset if the laser was previously hitting the crystal.
        {
            lasersCurrentlyHitting.Remove(laser); // Remove the laser from the hit set.
            if (lasersCurrentlyHitting.Count == 0) // If no lasers are hitting, reset the material.
            {
                crystalRenderer.material = originalMaterial;
                //Debug.Log("Crystal material reset to: " + originalMaterial.name);
            }
        }
    }

    public Material CurrentMaterial => this.crystalRenderer.material;

    public Material RequiredMaterial => requiredMaterial;
}
