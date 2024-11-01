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
    private float clambStamina = 10f;
    private float jumpStamina = 20f;
    private float additinalSpeed = 0;
    private float additinalJumpPower = 0;
    private int maxJumpCount = 1;
    private int nowJumpCount = 0;

    [Header("Look")]
    public Transform camContainer;
    public float minLook;
    public float maxLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;
    [HideInInspector]
    public bool canMove = true;

    private bool isClamb = false;
    private RaycastHit clambRayHit;

    public UnityAction inventory;

    private Rigidbody rb;
    private CharacterBuffs buffs;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        buffs = GetComponent<CharacterBuffs>();
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

    //움직임 입력
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

    //점프 입력
    public void OnJump(InputAction.CallbackContext context)
    {
        if (canMove == false) return;

        if(context.phase == InputActionPhase.Started)
        {
            if (nowJumpCount < maxJumpCount + buffs.BuffedJumpCount && CharacterManager.Instance.Player.condition.UseStamina(jumpStamina))
            {
                nowJumpCount++;
                rb.AddForce(Vector2.up * (jumpForce + additinalJumpPower + buffs.BuffedJumpHight), ForceMode.Impulse);
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

    //마우스 위치 입력(시점용)
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

    //캐릭터 움직이기
    public void Move()
    {
        //입력으로부터 움직일 방향 결정
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        //방향 * 속도 -> 실제 움직임
        dir *= (moveSpeed + additinalSpeed + buffs.BuffedSpeed);

        //velocity 로 움직임 구현
        if (IsGrounded())
        {
            //y축 : 점프, 현재 velocity를 가져와서 높이 유지
            dir.y = rb.velocity.y;

            rb.velocity = dir;
        }
        else
        {
            transform.position += dir * 0.01f;
        }
    }

    //카메라 위,아래로 돌리기
    private void CamLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minLook, maxLook);

        camContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    //캐릭터가 땅에 닿았는지 확인
    private bool IsGrounded()
    {
        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
        if(Physics.CheckSphere(spherePos,groundCheckRange,groundLayerMask,QueryTriggerInteraction.Ignore))
        {
            nowJumpCount = 0;
            
            return true;
        }

        return false;
    }

    //커서 조작, true일때 커서조작 가능
    public void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = toggle;
        canLook = !toggle;
    }

    public void LaunchPlayer(Vector3 force)
    {
        rb.AddForce(force, ForceMode.VelocityChange);
    }

    public void StopMove(bool isCanMove)
    {
        canMove = isCanMove;
        rb.velocity = isCanMove ? rb.velocity : Vector3.zero;
        canLook = isCanMove;
    }

    public void ClambMove()
    {
        if(IsGrounded() || CharacterManager.Instance.Player.condition.UseStamina(clambStamina * Time.deltaTime) == false)
        {
            isClamb = false;
            rb.useGravity = true;
        }

        //이전 벽을 인식한 점의 Y축 값을 내 Y값으로 변경
        Vector3 grapPoint = new Vector3(clambRayHit.point.x, transform.position.y, clambRayHit.point.z);
        Vector3 dir = Vector3.zero;

        //Y축이 조정된 이전 벽 감지 위치로 레이 발사
        Ray ray = new Ray(transform.position, grapPoint - transform.position);
        //레이 충돌 지점에 벽이 있으면 레이캐스트를 다시 저장
        if (Physics.Raycast(ray, out clambRayHit, 2f, groundLayerMask))
        {
            //레이 충돌지점의 노말벡터 y축 무시, 벡터 90도 회전
            Vector3 side = new Vector3(-clambRayHit.normal.z, 0, clambRayHit.normal.x);
            dir = clambRayHit.transform.up * curMovementInput.y + side * curMovementInput.x;
            dir = dir * moveSpeed * 0.5f;
        }
        else
        {
            //레이 범위를 벗어났는데 벽이 없으면 벽타기 종료
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

    public void SetAdditinalSpeed(float _speed, float _jump)
    {
        additinalSpeed = _speed;
        additinalJumpPower = _jump;
    }
}
