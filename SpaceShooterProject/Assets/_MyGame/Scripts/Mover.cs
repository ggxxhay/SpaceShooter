using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public float speed;

	// Move the object
    private void Start()
    {
        if(gameObject.tag == "Gift")
        {
            GetComponent<Rigidbody>().velocity = transform.up * speed;
        }
        else
        {
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        }
    }
}
