using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TetrisManager : NonsensicalMono
{
    [SerializeField] private TMP_InputField ipf_level;
    [SerializeField] private Button btn_Start;
    [SerializeField] private TextMeshProUGUI txt_score;

    private int score;
    protected override void Awake()
    {
        base.Awake();
        Subscribe("scoreReset", ScoreReset);
        Subscribe<int>("scoreAdd", ScoreAdd);
        
        btn_Start.onClick.AddListener(OnStart);
    }

    private void OnStart()
    {
        Publish("startTetris", int.Parse(ipf_level.text));
    }

    private void ScoreReset()
    {
        score = 0;
        UpdateScoreText();
    }

    private void ScoreAdd(int score)
    {
        this.score += score;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        txt_score.text= score.ToString();
    }
}
