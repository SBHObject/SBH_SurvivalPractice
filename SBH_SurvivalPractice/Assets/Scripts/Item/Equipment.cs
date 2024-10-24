using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    //현재 장착중인 장비
    public Equip currEquip;

    public Transform equipParent;

    private PlayerController controller;
    private PlayerCondition condition;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    public void EquipNew(ItemData data)
    {
        //장착해제
        UnEquip();

        currEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
    }

    public void UnEquip()
    {
        if(currEquip != null)
        {
            Destroy(currEquip.gameObject);
            currEquip = null;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && currEquip != null && controller.canLook)
        {
            currEquip.OnAttackInput();
        }
    }
}
