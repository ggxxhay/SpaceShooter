using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public float speed;

    /// <summary>
    /// Move the object down
    /// </summary>
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

    private void Update()
    {
        // Change the moving of enemy bullet when its' rotation is changed (boss case)
        if (gameObject.tag == "EnemyBullet")
        {
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        }
    }
}
