using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;


public class Score : MonoBehaviour
{

    public IntEvent score;

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
        text.SetText(newScore.ToString());
    }
}
