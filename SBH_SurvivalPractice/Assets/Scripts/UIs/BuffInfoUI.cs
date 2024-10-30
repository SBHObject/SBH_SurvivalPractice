using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuffInfoUI : MonoBehaviour
{
    public Image leftTimeImage;
    public TextMeshProUGUI buffTitleText;
    private int buffId;

    private float buffTime = 1;
    private float leftTime = 1;

    public UnityAction<int> onDestroy;

    private void Update()
    {
        leftTime -= Time.deltaTime;
        leftTimeImage.fillAmount = leftTime / buffTime;

        if(leftTime <= 0)
        {
            onDestroy?.Invoke(buffId);
            Destroy(gameObject);
        }
    }

    public void SetUp(BuffData buff)
    {
        buffTitleText.text = buff.name;
        buffTime = buff.buffTime;
        leftTime = buff.buffTime;
        buffId = buff.buffId;
    }
}
