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

public enum EquipStat
{
    Health,
    Stamina,
    Speed,
    Jump
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
    public BuffData[] buffs;

    [Header("Equip")]
    public GameObject equipPrefab;
    public EquipType equipType;
    public ItemDataEquip[] equipmentStat;
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
    public EquipStat equipStat;
    public float value;
}