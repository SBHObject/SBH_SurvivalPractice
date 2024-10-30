using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    SpeedUp,
    JumpHeight,
    JumpCount
}

[CreateAssetMenu(fileName = "BuffData", menuName = "Buff")]
public class BuffData : ScriptableObject
{
    public string buffName;
    public int buffId;

    public BuffType buffType;
    public float value;
    public float buffTime;
}
