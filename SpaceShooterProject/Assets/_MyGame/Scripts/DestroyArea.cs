using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyArea : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);
        if(layerName == "PlayerBullet")
        {
            other.gameObject.GetComponent<PlayerBullet>().isActive = false;
            return;
        }
        Destroy(other.gameObject);
    }
}
