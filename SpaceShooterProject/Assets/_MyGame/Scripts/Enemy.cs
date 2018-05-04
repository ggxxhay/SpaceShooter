using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    SpaceShip spaceShip;

	// Use this for initialization
	IEnumerator Start () {
        spaceShip = GetComponent<SpaceShip>();
        while (true)
        {

        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
