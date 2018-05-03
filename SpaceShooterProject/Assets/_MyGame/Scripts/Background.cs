using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    public float speed;
	
	// Change background's offset frequently => background moving
	void Update () {
        float y = Mathf.Repeat(Time.time * speed, 1);
        Vector2 offset = new Vector2(0, y);
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
	}
}
