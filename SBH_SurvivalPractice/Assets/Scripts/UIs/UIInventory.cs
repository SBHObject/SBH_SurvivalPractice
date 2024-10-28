using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;
    public ItemSlot[] equipSlots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform equipSlotPanel;

    private PlayerController controller;
    private PlayerCondition condition;
    
    public Transform dropPosition;

    //아이템 정보 표기
    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;

    public GameObject useButton;
    public GameObject equipeButton;
    public GameObject unEquipeButton;
    public GameObject dropButton;

    //정보를 표기할 아이템
    private ItemSlot selectedItem;

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];
        equipSlots = new ItemSlot[equipSlotPanel.childCount];

        for(int i = 0;  i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }

        for(int i = 0; i < equipSlots.Length; i++)
        {
            equipSlots[i] = equipSlotPanel.GetChild(i).GetComponent<ItemSlot>();
            equipSlots[i].index = i;
            equipSlots[i].inventory = this;
            equipSlots[i].Clear();
        }

        ClearSelectedItemInfo();

        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;
    }

    private void ClearSelectedItemInfo()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipeButton.SetActive(false);
        unEquipeButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if(IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if(data.canStack == true)
        {
            ItemSlot slot = GetItemStack(data);
            if(slot != null )
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if(emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        ThrowItem(data);

        CharacterManager.Instance.Player.itemData = null;
    }

    private void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }

        for(int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].item != null)
            {
                equipSlots[i].Set();
            }
            else
            {
                equipSlots[i].Clear();
            }
        }
    }

    private ItemSlot GetItemStack(ItemData data)
    {
        for(int i = 0; i < slots.Length; i ++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }

        return null;
    }

    private ItemSlot GetEmptySlot()
    {
        for(int i = 0; i < slots.Length; i ++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }

        return null;
    }

    private void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index, bool isEquiped)
    {
        if (isEquiped == false)
        {
            selectedItem = slots[index];
        }
        else
        {
            selectedItem = equipSlots[index];
        }

        if (selectedItem.item == null)
        {
            return;
        }

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
        {
            selectedStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }

        for( int i = 0; i < selectedItem.item.equipmentStat.Length; i++)
        {
            selectedStatName.text += selectedItem.item.equipmentStat[i].equipStat.ToString() + "\n";
            selectedStatValue.text += selectedItem.item.equipmentStat[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipeButton.SetActive(selectedItem.item.type == ItemType.Equipable && !selectedItem.equipped);
        unEquipeButton.SetActive(selectedItem.item.type == ItemType.Equipable && selectedItem.equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if(selectedItem.item.type == ItemType.Consumable)
        {
            for(int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                switch(selectedItem.item.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.item.consumables[i].value);
                        break;

                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.item.consumables[i].value);
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }

    public void OnDropButton()
    {
        if(selectedItem.equipped && selectedItem.item.equipType == EquipType.Weapon)
        {
            CharacterManager.Instance.Player.equip.UnEquip();
        }

        ThrowItem(selectedItem.item);
        RemoveSelectedItem();
    }

    private void RemoveSelectedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity <= 0 )
        {
            selectedItem.item = null;
            selectedItem = null;
            ClearSelectedItemInfo();
        }

        UpdateUI();
    }

    public void OnEquipButton()
    {
        int slotIndex = -1;
        switch(selectedItem.item.equipType)
        {
            case EquipType.Weapon:
                slotIndex = 0;
                CharacterManager.Instance.Player.equip.EquipNew(selectedItem.item);
                break;
            case EquipType.Head:
                slotIndex = 1;
                break;
            case EquipType.Armor:
                slotIndex = 2;
                break;
            case EquipType.Shoes:
                slotIndex = 3;
                break;
            case EquipType.None:
                slotIndex = -1;
                break;
        }

        if (slotIndex < 0) return;

        ItemData tempItem = null;

        if (equipSlots[slotIndex].equipped)
        {
            tempItem = equipSlots[slotIndex].item;
        }

        equipSlots[slotIndex].item = selectedItem.item;
        equipSlots[slotIndex].quantity = 1;
        equipSlots[slotIndex].equipped = true;

        if (tempItem != null)
        {
            selectedItem.item = tempItem;
        }
        else
        {
            selectedItem.item = null;
        }

        SelectItem(slotIndex, equipSlots[slotIndex].equipped);
        ExtraStatCheck();
        UpdateUI();
    }

    private void UnEquip()
    {
        if(selectedItem.item.equipType == EquipType.Weapon)
        {
            CharacterManager.Instance.Player.equip.UnEquip();
        }

        selectedItem.equipped = false;

        ItemSlot emptySlot = GetEmptySlot();
        if(emptySlot != null)
        {
            emptySlot.item = selectedItem.item;
            emptySlot.quantity = 1;
            RemoveSelectedItem();
            selectedItem = emptySlot;
        }
        else
        {
            ThrowItem(selectedItem.item);
            ClearSelectedItemInfo();
        }

        ExtraStatCheck();
        UpdateUI();
    }

    public void OnUnEquipButton()
    {
        UnEquip();
    }

    private void ExtraStatCheck()
    {
        float maxHealth = 0;
        float maxStamina = 0;
        float speed = 0;

        for(int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].item == null) continue;
            for (int j = 0; j < equipSlots[i].item.equipmentStat.Length; j++)
            {
                switch (equipSlots[i].item.equipmentStat[j].equipStat)
                {
                    case EquipStat.Health:
                        maxHealth += equipSlots[i].item.equipmentStat[j].value;
                        break;

                    case EquipStat.Stamina:
                        maxStamina += equipSlots[i].item.equipmentStat[j].value;
                        break;

                    case EquipStat.Speed:
                        speed += equipSlots[i].item.equipmentStat[j].value;
                        break;
                }
            }
        }

        condition.SetItemStats(maxHealth, maxStamina);
        controller.SetAdditinalSpeed(speed);
    }
}
