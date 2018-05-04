using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float shotDelay;
    public GameObject bullet;

	// Use this for initialization
	IEnumerator Start () {
        while (true)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            yield return new WaitForSeconds(shotDelay);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
