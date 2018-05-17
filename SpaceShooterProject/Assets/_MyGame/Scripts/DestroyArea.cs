using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyArea : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<GameController>().RemovePoolingObject(other.gameObject);
    }
}
