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

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject prefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataCunsumable[] cunsumables;

    [Header("Equip")]
    public GameObject equipPrefab;
}

public class ItemDataCunsumable
{
    public ConsumableType type;
    public float value;
}
