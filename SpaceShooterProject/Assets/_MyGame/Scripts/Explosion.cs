using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float lifetime = 2;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, lifetime);
	}
}
