using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    private int score;

    public Text scoreUI;

    // Use this for initialization
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreUI.text = "Score: " + score;
    }

    public void AddPoint(int point)
    {
        score += point;
    }

    public int GetScore()
    {
        return score;
    }
}
