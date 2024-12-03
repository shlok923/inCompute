using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserScript : MonoBehaviour
{
    [SerializeField] private LineRenderer laserLineRenderer; // LineRenderer for the laser
    [SerializeField] private float maxLaserDistance = 50f;   // Maximum distance the laser can travel
    [SerializeField] private LayerMask reflectionLayerMask;  // Layers the laser can interact with

    private void Awake()
    {
        // Ensure the LineRenderer is initialized
        if (laserLineRenderer == null)
        {
            laserLineRenderer = GetComponent<LineRenderer>();
        }
    }

    private void Update()
    {
        ShootLaser();
    }

    private void ShootLaser()
    {
        // Initialize laser positions
        List<Vector3> laserPoints = new List<Vector3>();
        Vector3 laserOrigin = transform.position;
        Vector3 laserDirection = transform.forward;

        laserPoints.Add(laserOrigin); // Start point of the laser

        for (int i = 0; i < 10; i++) // Limit reflections to prevent infinite loops
        {
            Ray ray = new Ray(laserOrigin, laserDirection);
            if (Physics.Raycast(ray, out RaycastHit hit, maxLaserDistance, reflectionLayerMask))
            {
                laserPoints.Add(hit.point); // Add the hit point to the laser path

                // Check if the object hit is reflective
                if (hit.collider.CompareTag("Mirror"))
                {
                    Debug.Log("Mirror hit");
                    // Reflect the laser direction based on the hit normal
                    laserDirection = Vector3.Reflect(laserDirection, hit.normal);
                    laserOrigin = hit.point;
                }
                else
                {
                    // Laser hits a non-reflective object; stop further reflections
                    break;
                }
            }
            else
            {
                // If no hit, draw laser to the max distance
                laserPoints.Add(laserOrigin + laserDirection * maxLaserDistance);
                break;
            }
        }

        // Update the LineRenderer to match the laser path
        laserLineRenderer.positionCount = laserPoints.Count;
        laserLineRenderer.SetPositions(laserPoints.ToArray());
    }
}

