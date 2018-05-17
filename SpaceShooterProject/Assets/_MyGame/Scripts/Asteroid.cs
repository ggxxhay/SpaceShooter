using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    // Impact to degree of angularVelocity
    public float tumble;

	/// <summary>
    /// Rotate the asteroid while moving
    /// </summary>
	void Start () {
        // Let the asteroid turn around
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
	}
}
