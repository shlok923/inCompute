using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonBorder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RawImage borderImage;
    private float fadeDuration = 0.3f;

    private void Start()
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(FadeIn());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color currentColor = borderImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            borderImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
        borderImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color currentColor = borderImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - (elapsedTime / fadeDuration);
            borderImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
        borderImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
    }
}