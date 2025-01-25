using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript Instance { get; private set; }

    //[SerializeField] private Image fadeOverlay; // UI Image for fade effect
    //[SerializeField] private float fadeDuration = 1f;
    //[SerializeField] private Canvas fadeCanvas;


    private void Awake()
    {
        // Ensure this object persists across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //// Ensure fadeCanvas persists
            //if (fadeCanvas != null)
            //{
            //    DontDestroyOnLoad(fadeCanvas.gameObject);
            //}

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //if (fadeOverlay != null)
        //{
        //    fadeOverlay.color = new Color(0, 0, 0, 1); // Start fully black
        //    FadeIn();
        //}
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        //StartCoroutine(FadeOutAndChangeScene(sceneName));
    }

    //private IEnumerator FadeOutAndChangeScene(string sceneName)
    //{
    //    if (fadeOverlay != null)
    //    {
    //        // Fade to black
    //        float elapsedTime = 0f;
    //        while (elapsedTime < fadeDuration)
    //        {
    //            fadeOverlay.color = new Color(0, 0, 0, elapsedTime / fadeDuration);
    //            elapsedTime += Time.deltaTime;
    //            yield return null;
    //        }
    //        fadeOverlay.color = new Color(0, 0, 0, 1);
    //    }

    //    // Change the scene
    //    SceneManager.LoadScene(sceneName);

    //    // Wait for the scene to load
    //    yield return new WaitForEndOfFrame();

    //    // Fade in
    //    FadeIn();
    //}

    //private void FadeIn()
    //{
    //    if (fadeOverlay != null)
    //    {
    //        StartCoroutine(FadeInCoroutine());
    //    }
    //}

    //private IEnumerator FadeInCoroutine()
    //{
    //    float elapsedTime = 0f;
    //    while (elapsedTime < fadeDuration)
    //    {
    //        fadeOverlay.color = new Color(0, 0, 0, 1 - (elapsedTime / fadeDuration));
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //    fadeOverlay.color = new Color(0, 0, 0, 0); // Fully transparent
    //}
}
