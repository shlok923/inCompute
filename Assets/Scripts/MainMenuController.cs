using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject title;
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject settingTitle;
    [SerializeField] GameObject audioMixers;

    public void Start(){
        settingTitle.SetActive(false);
        audioMixers.SetActive(false);
        title.SetActive(true);
        buttons.SetActive(true);
    }

    public void StartGame(){
        SceneManager.LoadScene("GameScene");
    }

    public void ContinueGame(){
        if (PlayerPrefs.HasKey("LastSavedGame"))
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    public void OpenSettings(){
        title.SetActive(false);
        buttons.SetActive(false);
        settingTitle.SetActive(true);
        audioMixers.SetActive(true);
    }


    public void OpenMenu(){
        settingTitle.SetActive(false);
        audioMixers.SetActive(false);
        title.SetActive(true);
        buttons.SetActive(true);
    }

    public void QuitGame(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}