using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoWidth : NonsensicalMono
{
    protected override void Awake()
    {
        base.Awake();

        ChangeWidth(Screen.width, Screen.height);
        Subscribe<int, int>("screenSizeChanged", ChangeWidth);
    }

    /// <summary>
    /// 动态改变宽度
    /// </summary>
    /// <param name="width">屏幕宽度：Screen.width</param>
    /// <param name="height">屏幕高度：Screen.height</param>
    private void ChangeWidth(int width, int height)
    {
        float logWidth = Mathf.Log(width / 1920f, 2);
        float logHeight = Mathf.Log(height / 1080f, 2);
        float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, 0.5f);
        float power = Mathf.Pow(2, logWeightedAverage);
        float trueWidth = width / power;
        float trueHeight = height / power;
        float left = trueWidth - (trueHeight / 2);
        float newWidth = left / 2 - 20;
        GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, 0);
    }
}
