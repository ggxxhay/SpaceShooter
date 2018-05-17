using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour
{

    // Hazards health
    public int hp;

    // Hazards kill point
    public int point;

    public GameObject gift;

    //GameController gameController;

    // Use this for initialization
    void Start()
    {
        //gameController = GetComponent<GameController>();
        //hp = gameController.currentWave * 2;
        //point = gameController.currentWave * 5;
    }

    // Get current wave number
    //public void ReduceHazardsAlive()
    //{
    //    gameController.hazardsAlive--;
    //}

    // Update is called once per frame
    void Update()
    {
    }
    
    
}
