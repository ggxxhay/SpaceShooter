using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    // Impact to degree of angularVelocity
    public float tumble;

	// Use this for initialization
	void Start () {
        // Let the asteroid turn around
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
