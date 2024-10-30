using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuffs : MonoBehaviour
{
    public Dictionary<int, Coroutine> nowBuffs = new Dictionary<int, Coroutine>();

    public float BuffedSpeed { get; private set; } = 0;
    public float BuffedJumpHight { get; private set; } = 0;
    public int BuffedJumpCount { get; private set; } = 0;

    //������ ������ ȣ��
    public void AddBuff(BuffData addedBuff)
    {
        if(nowBuffs.ContainsKey(addedBuff.buffId))
        {
            StopCoroutine(nowBuffs[addedBuff.buffId]);
            nowBuffs.Remove(addedBuff.buffId);
        }

        nowBuffs.Add(addedBuff.buffId, StartCoroutine(MaintainBuff(addedBuff)));
    }

    private IEnumerator MaintainBuff(BuffData buff)
    {
        //TODO : ������ ���� ���� ���� ����
        SetBuffStat(buff, true);

        yield return new WaitForSeconds(buff.buffTime);

        //TODO : ������ ���� ���� ���� ����
        SetBuffStat(buff, false);

        nowBuffs.Remove(buff.buffId);
    }

    public void SetBuffStat(BuffData buff, bool isAdd)
    {
        float value = isAdd? buff.value : buff.value * -1;

        switch(buff.buffType)
        {
            case BuffType.SpeedUp:
                BuffedSpeed += value;
                break;

            case BuffType.JumpHeight:
                BuffedJumpHight += value;
                break;

            case BuffType.JumpCount:
                BuffedJumpCount += (int)value;
                break;
        }
    }
}
