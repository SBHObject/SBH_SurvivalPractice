using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private bool isOpen = false;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsOpen", isOpen);
    }

    public string GetInteractPrompt()
    {
        string str = "[E]\n키를 눌러 ";
        if(isOpen)
        {
            str += "닫기";
        }
        else
        {
            str += "열기";
        }

        return str;
    }

    public void OnInteracte()
    {
        isOpen = !isOpen;
        animator.SetBool("IsOpen", isOpen);
    }
}
