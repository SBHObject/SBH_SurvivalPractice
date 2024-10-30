using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffListUI : MonoBehaviour
{
    public GameObject buffInfoUI;
    private Dictionary<int, GameObject> buffInfos = new Dictionary<int, GameObject>();

    private void Start()
    {
        CharacterManager.Instance.Player.buffs.onAddBuff += AddBuffInfo;
    }

    public void AddBuffInfo(BuffData buff)
    {
        if (buffInfos.ContainsKey(buff.buffId))
        {
            Destroy(buffInfos[buff.buffId]);
            RemoveKey(buff.buffId);
        }

        buffInfos.Add(buff.buffId, Instantiate(buffInfoUI, transform));
        buffInfos[buff.buffId].GetComponent<BuffInfoUI>().SetUp(buff);
        buffInfos[buff.buffId].GetComponent<BuffInfoUI>().onDestroy += RemoveKey;
    }

    private void RemoveKey(int buffId)
    {
        buffInfos.Remove(buffId);
    }
}
