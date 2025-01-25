using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{

    public SceneManagerScript sceneManager; 
    private void Awake()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManagerScript>();
    }

    public void StartGameButton()
    {
        sceneManager.ChangeScene("GameScene");
    }
}
