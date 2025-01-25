using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
    public SceneManagerScript sceneManager;
    private void Awake()
    {
        sceneManager = FindFirstObjectByType<SceneManagerScript>();
    }

    public void BackToMenuButton()
    {
        sceneManager.ChangeScene("Menu");
    }
}
