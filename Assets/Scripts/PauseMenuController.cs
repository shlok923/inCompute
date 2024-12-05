using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject settingsUI;
    [SerializeField] GameObject audioMixers;

    private bool isPaused = false;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        settingsUI.SetActive(false);
        audioMixers.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        audioMixers.SetActive(true);
        settingsUI.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        settingsUI.SetActive(false);
        audioMixers.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}