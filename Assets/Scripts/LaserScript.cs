using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserScript : MonoBehaviour
{
    [SerializeField] private LineRenderer laserLineRenderer;
    [SerializeField] private float maxLaserDistance = 50f;
    [SerializeField] private LayerMask reflectionLayerMask;
    [SerializeField] private Crystal[] crystals; // Reference all crystals in the scene.

    private HashSet<Crystal> crystalsHitThisFrame = new HashSet<Crystal>(); // Track crystals hit in this frame.

    private void Start()
    {
        if (laserLineRenderer == null)
        {
            laserLineRenderer = GetComponent<LineRenderer>();
        }
    }

    private void Update()
    {
        ShootLaser();
        HandleLaserExit();
    }

    private void ShootLaser()
    {
        crystalsHitThisFrame.Clear(); // Clear the set at the start of each frame.

        List<Vector3> laserPoints = new List<Vector3>();
        Vector3 laserOrigin = transform.position;
        Vector3 laserDirection = transform.forward;

        laserPoints.Add(laserOrigin);

        for (int i = 0; i < 10; i++) // Limit reflections.
        {
            Ray ray = new Ray(laserOrigin, laserDirection);
            if (Physics.Raycast(ray, out RaycastHit hit, maxLaserDistance, reflectionLayerMask))
            {
                laserPoints.Add(hit.point);

                // Check if the hit object is a crystal.
                foreach (Crystal crystal in crystals)
                {
                    if (hit.collider.gameObject == crystal.gameObject)
                    {
                        Material laserMaterial = laserLineRenderer.material;
                        crystal.OnLaserHit(this, laserMaterial); // Pass the current laser instance to the crystal.
                        crystalsHitThisFrame.Add(crystal); // Mark this crystal as hit.
                        //Debug.Log("Laser " + gameObject.name + " hit Crystal: " + crystal.gameObject.name + " applying material " + laserMaterial.name);
                        break; // Stop further checks for this crystal.
                    }
                }

                // Handle reflection.
                if (hit.collider.CompareTag("Mirror"))
                {
                    laserDirection = Vector3.Reflect(laserDirection, hit.normal);
                    laserOrigin = hit.point;
                }
                else
                {
                    break;
                }
            }
            else
            {
                laserPoints.Add(laserOrigin + laserDirection * maxLaserDistance);
                break;
            }
        }

        laserLineRenderer.positionCount = laserPoints.Count;
        laserLineRenderer.SetPositions(laserPoints.ToArray());
    }

    private void HandleLaserExit()
    {
        // Reset crystals that were not hit by any laser this frame.
        foreach (Crystal crystal in crystals)
        {
            if (!crystalsHitThisFrame.Contains(crystal))
            {
                crystal.OnLaserExit(this); // Pass the current laser instance to reset it.
            }
        }
    }
}
