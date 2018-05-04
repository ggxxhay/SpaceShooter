using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed;

	// Shot Bullet
    private void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
}
