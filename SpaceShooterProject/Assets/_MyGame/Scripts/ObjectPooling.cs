using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour {

    [HideInInspector]
    public bool isActive;

    private void Start()
    {
        isActive = true;
    }
}
