using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageHover : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private string inputText;
    public static bool IsTriggered = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (textMeshPro != null)
        {
            textMeshPro.text = inputText;
        }
        SetTextOpacity(0f);
    }

    private void Update()
    {
        animator.SetBool("IsTriggered", IsTriggered);
        SetTextOpacity(IsTriggered ? 1f : 0f);
    }

    private void SetTextOpacity(float alpha)
    {
        if (textMeshPro != null)
        {
            Color currentColor = textMeshPro.color;
            textMeshPro.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
        }
    }

    public void ShowHoverText(string text)
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = text;
        }

        IsTriggered = true;
        animator.SetBool("IsTriggered", IsTriggered);

    }

    public void HideHoverText()
    {
        if (IsTriggered)
        {
            IsTriggered = false;
            animator.SetBool("IsTriggered", IsTriggered);
        }
        else
        {
            Debug.Log("Hover text already hidden");
        }
    }
}