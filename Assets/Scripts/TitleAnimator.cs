using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RawImageAnimator : MonoBehaviour
{
    public RawImage image1;
    public RawImage image2;
    public RawImage image3;
    public RawImage image4;

    public float displayTime1 = 2f;
    public float displayTime2 = 0.05f;
    public float displayTime3 = 0.05f;
    public float displayTime4 = 0.05f;
    public float transitionDuration = 0.02f;

    void Start()
    {
        StartCoroutine(AnimateImages());
    }

    IEnumerator AnimateImages()
    {
        while (true)
        {
            yield return StartCoroutine(ShowImage(image1, displayTime1));
            yield return StartCoroutine(ShowImage(image2, displayTime2));
            yield return StartCoroutine(ShowImage(image3, displayTime3));
            yield return StartCoroutine(ShowImage(image4, displayTime4));
        }
    }

    IEnumerator ShowImage(RawImage imageToShow, float displayTime)
    {
        imageToShow.gameObject.SetActive(true);
        yield return StartCoroutine(FadeInImage(imageToShow));
        yield return new WaitForSeconds(displayTime);
        yield return StartCoroutine(FadeOutImage(imageToShow));
        imageToShow.gameObject.SetActive(false);
    }

    IEnumerator FadeInImage(RawImage image)
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            float alpha = elapsedTime / transitionDuration;
            image.color = new Color(1f, 1f, 1f, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.color = new Color(1f, 1f, 1f, 1f);
    }

    IEnumerator FadeOutImage(RawImage image)
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            float alpha = 1f - (elapsedTime / transitionDuration);
            image.color = new Color(1f, 1f, 1f, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.color = new Color(1f, 1f, 1f, 0f);
    }
}