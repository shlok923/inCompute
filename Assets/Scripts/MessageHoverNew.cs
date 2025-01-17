using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MessageHoverNew : MonoBehaviour
{
    public Canvas canvas; // Assign your canvas in the Inspector
    public float slideDuration = 0.5f; // Duration of the slide-in animation

    private Vector2 startPosition = new Vector2(-500, -500);
    private Vector2 targetPosition = new Vector2(20, -20);
    private Vector2 positionBeforeSlide;
    RectTransform rectTransform;
    public bool isHovering = false;

    public void ShowHoverText(string text, Color backgroundColor = default)
    {
        if (canvas == null)
        {
            Debug.LogError("Canvas is not assigned. Please assign the canvas in the Inspector.");
            return;
        }

        if (backgroundColor == default)
        {
            backgroundColor = Color.black;
        }

        // Create the Background Panel
        GameObject background = new GameObject("Background");
        background.transform.SetParent(canvas.transform, false);

        RectTransform bgRect = background.AddComponent<RectTransform>();
        rectTransform = bgRect;
        bgRect.anchorMin = new Vector2(0, 1); // Anchor to top-left
        bgRect.anchorMax = new Vector2(0, 1);
        bgRect.pivot = new Vector2(0, 1); // Pivot at top-left
        bgRect.anchoredPosition = startPosition; // Start off-screen or elsewhere

        // Add Image Component for the Background
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = backgroundColor;

        // Add Content Size Fitter
        ContentSizeFitter contentFitter = background.AddComponent<ContentSizeFitter>();
        contentFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Add Layout Group to Handle Child Alignment
        VerticalLayoutGroup layoutGroup = background.AddComponent<VerticalLayoutGroup>();
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;

        // Create the TMP Text Object
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(background.transform, false);

        TextMeshProUGUI tmpText = textObject.AddComponent<TextMeshProUGUI>();
        tmpText.text = text;
        tmpText.fontSize = 40; // Adjust font size as needed
        tmpText.alignment = TextAlignmentOptions.Center;
        tmpText.color = Color.white;

        // TMP RectTransform for Layout
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.sizeDelta = Vector2.zero; // Let the layout system manage size

        // Force Layout Rebuild to Update Sizes
        LayoutRebuilder.ForceRebuildLayoutImmediate(bgRect);

        positionBeforeSlide = new Vector2(-bgRect.rect.width - 100, -20);

        bgRect.anchoredPosition = positionBeforeSlide;

        // Start the slide-in animation
        StartCoroutine(SlideInAnimation(bgRect, positionBeforeSlide, targetPosition));
    }

    private IEnumerator SlideInAnimation(RectTransform rectTransform, Vector2 startPosition, Vector2 targetPosition)
    {
        float elapsedTime = 0f;
        if (rectTransform == null)
        {
            Debug.LogWarning("No hover text to slide in");
            yield break;
        }
        if (isHovering)
        {
            Debug.LogWarning("Hover text already showing");
            yield break;
        }

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / slideDuration);

            // Smoothly interpolate the position
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        // Ensure final position is exact
        rectTransform.anchoredPosition = targetPosition;
    }

    public void HideHoverText()
    {
        if (rectTransform == null)
        {
            Debug.LogWarning("No hover text to hide");
            return;
        }
        // Start the slide-out animation
        if (isHovering) StartCoroutine(SlideOutAnimation());
        isHovering = false;
    }

    private IEnumerator SlideOutAnimation()
    {
        float elapsedTime = 0f;

        while (elapsedTime < slideDuration)
        {
            // Check if the object is still valid
            if (rectTransform == null)
                yield break;

            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / slideDuration);

            // Smoothly interpolate the position
            rectTransform.anchoredPosition = Vector2.Lerp(targetPosition, positionBeforeSlide, t);

            yield return null;
        }

        // Ensure the final position is exact
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = positionBeforeSlide;
            Destroy(rectTransform.gameObject); // Destroy only after animation completes
        }
    }
}
