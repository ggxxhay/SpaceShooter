using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour {

    // Change master volume when slider's value changes
    public void VolumeChanged() {
        AudioListener.volume = FindObjectOfType<Slider>().value;
	}
}
