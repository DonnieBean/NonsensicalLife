using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class TetrisManager : NonsensicalMono
{
    [SerializeField] private Button btn_Start;

    protected override void Awake()
    {
        base.Awake();
        btn_Start.onClick.AddListener(OnStart);
    }

    private void OnStart()
    {
        Publish("startTetris", 9);
    }
}
