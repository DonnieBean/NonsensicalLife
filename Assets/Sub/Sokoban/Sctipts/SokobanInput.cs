using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanInput : NonsensicalMono
{
    protected override void Awake()
    {
        base.Awake();

        Subscribe<bool>("sobobanInputEnable", OnInputSwitch);
    }

    private void Start()
    {
        enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Publish<Vector2Int>("sokobanMove", new Vector2Int(0, 1));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Publish<Vector2Int>("sokobanMove", new Vector2Int(0, -1));
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Publish<Vector2Int>("sokobanMove", new Vector2Int(-1, 0));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Publish<Vector2Int>("sokobanMove", new Vector2Int(1, 0));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Publish("sokobanWithdraw");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Publish("sokobanRestart");
        }
    }

    private void OnInputSwitch(bool enable)
    {
        enabled = enable;
    }
}
