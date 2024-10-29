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
        string str = "[E]\nŰ�� ���� ";
        if(isOpen)
        {
            str += "�ݱ�";
        }
        else
        {
            str += "����";
        }

        return str;
    }

    public void OnInteracte()
    {
        isOpen = !isOpen;
        animator.SetBool("IsOpen", isOpen);
    }
}
