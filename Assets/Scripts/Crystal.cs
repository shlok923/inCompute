using UnityEngine;
using System.Collections;

public class Crystal : MonoBehaviour
{
    [SerializeField] private MeshRenderer crystalRenderer; // Renderer of the crystal object

    private void Start()
    {
        // Ensure the crystal has a renderer
        if (crystalRenderer == null)
        {
            crystalRenderer = GetComponent<MeshRenderer>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding is a laser
        if (collision.collider.CompareTag("Laser"))
        {
            Debug.Log("Laser hit crystal");
            // Get the laser's color
            LineRenderer laser = collision.collider.GetComponent<LineRenderer>();
            if (laser != null)
            {
                // Get the first color of the laser (assuming it's gradient)
                Color laserColor = laser.startColor;

                // Change the crystal's color to match the laser
                SetCrystalColor(laserColor);
            }
        }
    }

    public void SetCrystalColor(Color color)
    {
        StartCoroutine(ChangeCrystalColor(color, 1f));
    }

    private IEnumerator ChangeCrystalColor(Color targetColor, float duration)
    {
        Color initialColor = crystalRenderer.material.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            crystalRenderer.material.color = Color.Lerp(initialColor, targetColor, elapsedTime / duration);
            yield return null;
        }

        crystalRenderer.material.color = targetColor;
    }
}