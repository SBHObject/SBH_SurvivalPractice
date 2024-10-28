using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpForce;
    public LayerMask groundLayerMask;
    private float groundCheckRange = 0.22f;

    [Header("Look")]
    public Transform camContainer;
    public float minLook;
    public float maxLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;
    public bool canMove = true;

    private bool isClamb = false;
    private RaycastHit clambRayHit;

    public UnityAction inventory;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (isClamb == false)
            {
                Move();
            }
            else
            {
                ClambMove();
            }
        }
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CamLook();
        }
    }

    //������ �Է�
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    //���� �Է�
    public void OnJump(InputAction.CallbackContext context)
    {
        if (canMove == false) return;

        if(context.phase == InputActionPhase.Started)
        {
            if (IsGrounded())
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            }
            else
            {
                Ray ray = new Ray(transform.position, transform.forward);
                if (Physics.Raycast(ray, out clambRayHit, 1f, groundLayerMask))
                {
                    rb.useGravity = false;
                    
                    isClamb = true;
                }
            }
        }
    }

    //���콺 ��ġ �Է�(������)
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnInventoryOpen(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    //ĳ���� �����̱�
    public void Move()
    {
        //�Է����κ��� ������ ���� ����
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        //���� * �ӵ� -> ���� ������
        dir *= moveSpeed;

        //y�� : ����, ���� velocity�� �����ͼ� ���� ����
        dir.y = rb.velocity.y;

        //velocity �� ������ ����
        rb.velocity = dir;
    }

    //ī�޶� ��,�Ʒ��� ������
    private void CamLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minLook, maxLook);

        camContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    //ĳ���Ͱ� ���� ��Ҵ��� Ȯ��
    private bool IsGrounded()
    {
        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
        if(Physics.CheckSphere(spherePos,groundCheckRange,groundLayerMask,QueryTriggerInteraction.Ignore))
        {
            return true;
        }

        return false;
    }

    //Ŀ�� ����, true�϶� Ŀ������ ����
    public void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = toggle;
        canLook = !toggle;
    }

    public void LaunchPlayer(float power, Vector3 launchDir)
    {
        rb.AddForce(launchDir.normalized * power, ForceMode.VelocityChange);
    }

    public void StopMove()
    {
        canMove = false;
        rb.velocity = Vector3.zero;
    }

    public void ClambMove()
    {
        if(IsGrounded())
        {
            isClamb = false;
            rb.useGravity = true;
        }

        //���� ���� �ν��� ���� Y�� ���� �� Y������ ����
        Vector3 grapPoint = new Vector3(clambRayHit.point.x, transform.position.y, clambRayHit.point.z);
        Vector3 dir = Vector3.zero;

        //Y���� ������ ���� �� ���� ��ġ�� ���� �߻�
        Ray ray = new Ray(transform.position, grapPoint - transform.position);
        //���� �浹 ������ ���� ������ ����ĳ��Ʈ�� �ٽ� ����
        if (Physics.Raycast(ray, out clambRayHit, 2f, groundLayerMask))
        {
            //���� �浹������ �븻���� y�� ����, ���� 90�� ȸ��
            Vector3 side = new Vector3(-clambRayHit.normal.z, 0, clambRayHit.normal.x);
            dir = clambRayHit.transform.up * curMovementInput.y + side * curMovementInput.x;
            dir = dir * moveSpeed * 0.5f;
        }
        else
        {
            //���� ������ ����µ� ���� ������ ��Ÿ�� ����
            isClamb = false;
            rb.useGravity = true;
        }

        rb.velocity = dir;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);

        if(IsGrounded())
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color= Color.red;
        }

        Gizmos.DrawSphere(spherePos, groundCheckRange);
        Gizmos.DrawLine(transform.position, clambRayHit.point);
    }
}
