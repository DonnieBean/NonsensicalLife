using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SokobanManager : NonsensicalMono
{
    [SerializeField] private TMP_InputField ipf_level;
    [SerializeField] private Button btn_start;
    [SerializeField] private GameObject winWindow;


    protected override void Awake()
    {
        base.Awake();

        Subscribe("sokobanWin",OnWin);

        btn_start.onClick.AddListener(OnButtonClick);
    }
    private void OnButtonClick()
    {
        if (int.TryParse(ipf_level.text, out var v))
        {
            if (SokobanData.levels.ContainsKey(v))
            {
                winWindow.gameObject.SetActive(false);
                Publish("sobobanInputEnable",true);
                Publish("sokobanStart", v);
            }
        }
    }

    private void OnWin()
    {
        winWindow.gameObject.SetActive(true);
        Publish("sobobanInputEnable",false);
    }
}
