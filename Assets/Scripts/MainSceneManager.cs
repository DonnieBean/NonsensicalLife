using NonsensicalKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : NonsensicalMono
{
    [SerializeField] private GameObject mainSceneCanvas;

    protected override void Awake()
    {
        base.Awake();

        Subscribe<string,bool>("loadScene",OnLoadScene);
        Subscribe("returnMainMenu",OnReturnMainMenu);
    }

    private void OnReturnMainMenu()
    {
        mainSceneCanvas.gameObject.SetActive(true);
    }

    private void OnLoadScene(string arg1, bool arg2)
    {
        mainSceneCanvas.gameObject.SetActive(false);
    }
}
