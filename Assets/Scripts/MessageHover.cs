using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageHover : MonoBehaviour
{
    public static MessageHover _instance;
    public TextMeshProUGUI messageText;
    public GameObject Panel; 
    Animator animator;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        if (Panel != null)
        {
            animator = Panel.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Panel GameObject is not assigned in MessageHover!");
        }
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        animator.SetBool("Position", true);
        messageText.text = message;
        gameObject.SetActive(true);
    }

    public void HideMessage()
    {
        animator.SetBool("Position", false);
        gameObject.SetActive(false);
        messageText.text = "";
    }
}