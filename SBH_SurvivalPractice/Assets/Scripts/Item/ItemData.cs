using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resorce
}

public enum ConsumableType
{
    Hunger,
    Health,
    Buff
}

public enum EquipType
{
    None,
    Weapon,
    Head,
    Armor,
    Shoes
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataCunsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
    public ItemDataEquip equipment;
}

[System.Serializable]
public class ItemDataCunsumable
{
    public ConsumableType type;
    public float value;
}

[System.Serializable]
public class ItemDataEquip
{
    public EquipType equipType;
}