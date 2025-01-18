using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MessageHoverNew : MonoBehaviour
{
    public Canvas canvas;
    public float slideDuration = 0.5f;

    private Vector2 startPosition = new Vector2(-500, -500);
    private Vector2 targetPosition = new Vector2(20, -20);
    private Vector2 positionBeforeSlide;
    private RectTransform rectTransform;
    private TextMeshProUGUI tmpText;
    private Coroutine currentCoroutine; // Track the currently running coroutine
    [SerializeField] private int hoverTextSize = 40;
    [SerializeField] private TMP_FontAsset customFont;

    private void Awake()
    {
        CreateHoverUI();
    }

    private void CreateHoverUI()
    {
        if (canvas == null)
        {
            Debug.LogError("Canvas is not assigned. Please assign the canvas in the Inspector.");
            return;
        }

        // Create the Background Panel
        GameObject background = new GameObject("Background");
        background.transform.SetParent(canvas.transform, false);

        rectTransform = background.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = startPosition;

        // Add Image Component
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = Color.black;

        // Add Content Size Fitter
        ContentSizeFitter contentFitter = background.AddComponent<ContentSizeFitter>();
        contentFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Add Layout Group
        VerticalLayoutGroup layoutGroup = background.AddComponent<VerticalLayoutGroup>();
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;

        // Create the TMP Text Object
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(background.transform, false);

        tmpText = textObject.AddComponent<TextMeshProUGUI>();
        tmpText.fontSize = hoverTextSize;
        tmpText.alignment = TextAlignmentOptions.Center;
        tmpText.color = Color.white;

        // Assign custom font here
        if (customFont != null)
        {
            tmpText.font = customFont;
        }

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.sizeDelta = Vector2.zero;

        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        Debug.Log("it is width " +rectTransform.rect.width);
        positionBeforeSlide = new Vector2(-rectTransform.rect.width - 100, -20);
        rectTransform.anchoredPosition = positionBeforeSlide;

        background.SetActive(false); // Hide by default
    }

    public void ShowHoverText(string text, Color backgroundColor = default)
    {
        if (rectTransform == null || tmpText == null)
        {
            Debug.LogError("Hover UI components are not initialized.");
            return;
        }

        if (backgroundColor == default)
        {
            backgroundColor = Color.black;
        }

        tmpText.text = text;
        rectTransform.GetComponent<Image>().color = backgroundColor;

        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        positionBeforeSlide = new Vector2(-rectTransform.rect.width - 50, -20);

        rectTransform.gameObject.SetActive(true); // Ensure the UI is visible

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine); // Stop any ongoing animation
        }

        currentCoroutine = StartCoroutine(ShowHoverTextDelayed());
    }

    private IEnumerator ShowHoverTextDelayed()
    {
        yield return null; // Wait for one frame to ensure layout updates

        positionBeforeSlide = new Vector2(-rectTransform.rect.width - 50, -20);

        rectTransform.anchoredPosition = positionBeforeSlide;
        rectTransform.gameObject.SetActive(true); // Ensure the UI is visible

        currentCoroutine = StartCoroutine(SlideInAnimation());
    }

    private IEnumerator SlideInAnimation()
    {
        float elapsedTime = 0f;
        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / slideDuration);
            rectTransform.anchoredPosition = Vector2.Lerp(positionBeforeSlide, targetPosition, t);
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
        currentCoroutine = null;
    }

    public void HideHoverText()
    {
        if (rectTransform == null)
        {
            Debug.LogWarning("No hover text to hide.");
            return;
        }

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine); // Stop any ongoing animation
        }

        currentCoroutine = StartCoroutine(SlideOutAnimation());
    }

    private IEnumerator SlideOutAnimation()
    {
        Vector2 currentPosition = rectTransform.anchoredPosition;

        float elapsedTime = 0f;
        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / slideDuration);
            rectTransform.anchoredPosition = Vector2.Lerp(currentPosition, positionBeforeSlide, t);
            yield return null;
        }

        rectTransform.anchoredPosition = positionBeforeSlide;
        rectTransform.gameObject.SetActive(false); // Hide the UI after animation
        currentCoroutine = null;
    }
}
