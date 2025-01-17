using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private MessageHoverNew messageHoverUI;
    // singleton instance
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ShowHoverUI(string message)
    {
        if (message == null)
        {
            Debug.LogError("empty message");
        }
        else
        {
            messageHoverUI.ShowHoverText(message);
        }
    }

    public void HideHoverUI()
    {
        messageHoverUI.HideHoverText();
    }


}
