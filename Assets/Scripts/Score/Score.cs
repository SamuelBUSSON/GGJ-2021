using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;


public class Score : MonoBehaviour
{

    public IntEvent score;
    private int oldScore = 0;

    private TextMeshProUGUI text;
    
    // Start is called before the first frame update
    void Start()
    {
        score.Register(UpdateScore);
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnDisable()
    {
        score.Unregister(UpdateScore);
    }

    void UpdateScore(int newScore)
    {
        DOVirtual.Float(oldScore, newScore, 1.0f, SetTextJuice).SetEase(Ease.OutQuad);
    }

    void SetTextJuice(float value)
    {
        text.SetText(Mathf.RoundToInt(value).ToString());
    }
}
