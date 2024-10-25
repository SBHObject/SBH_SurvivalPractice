using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Hardware;
using UnityEngine;

public class Platform : MonoBehaviour
{
    //�÷����� �̵���� ���� �ʵ�
    [SerializeField]
    private Transform[] nodes;
    private Transform targetNode;
    public Transform nodeParent;
    private int targetIndex = 0;
    private bool isReturn = false;

    //���� �������� �Ÿ�
    private float targetDistance;

    //�÷����� ���� �̵� ���� �ʵ�
    [SerializeField] private float moveSpeed = 0.3f;
    private float currSpeed = 0;
    private float accel = 0.02f;

    [SerializeField] private float waitingTime = 3f;
    private bool isDestination = false;

    private void Start()
    {
        nodes = new Transform[nodeParent.childCount];
        for(int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = nodeParent.GetChild(i);
        }

        //1�� ��带 ���� �̵�
        SetNextNode();
    }

    private void FixedUpdate()
    {
        PlatformMove();
    }

    private void PlatformMove()
    {
        if (isDestination) return;

        Vector3 dir = targetNode.position - transform.position;
        float distance = Vector3.Distance(targetNode.position, transform.position);

        if(targetDistance * 0.5f <= distance )
        {
            currSpeed = Mathf.Lerp(currSpeed, moveSpeed, accel);
        }
        else
        {
            currSpeed = Mathf.Lerp(currSpeed, 0, accel);
        }

        transform.position += dir.normalized * currSpeed;

        if (distance <= 0.3f)
        {
            SetNextNode();
        }
    }

    private void SetNextNode()
    {
        if (isReturn)
        {
            targetIndex --;
            if (targetIndex == 0)
            {
                isReturn = false;
                isDestination = true;
                StartCoroutine(WaitTransfer());
            }
        }
        else
        {
            targetIndex ++;
            if (targetIndex == nodes.Length - 1)
            {
                isReturn = true;
                isDestination = true;
                StartCoroutine(WaitTransfer());
            }
        }

        targetNode = nodes[targetIndex];
        targetDistance = Vector3.Distance(targetNode.position, transform.position);
    }

    //������� ������ 3�ʰ� ���
    private IEnumerator WaitTransfer()
    {
        yield return new WaitForSeconds(waitingTime);
        isDestination = false;
    }
}
