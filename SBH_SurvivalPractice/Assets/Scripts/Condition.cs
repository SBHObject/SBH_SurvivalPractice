using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public float passiveValue;
    public Image uiBar;

    public float additinalMaxValue;
    public float realMaxValue;

    private void Start()
    {
        curValue = startValue;
        realMaxValue = maxValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, realMaxValue);
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0);
    }

    private float GetPercentage()
    {
        return curValue / realMaxValue;
    }

    public void SetAdditionalMaxValue(float amount)
    {
        additinalMaxValue = amount;
        realMaxValue = maxValue + additinalMaxValue;
    }
}
