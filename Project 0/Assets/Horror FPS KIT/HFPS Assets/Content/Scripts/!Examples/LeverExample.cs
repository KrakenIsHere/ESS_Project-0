using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverExample : MonoBehaviour {
	public Light SwitchLight;

	public void SwitcherUp () {
		SwitchLight.enabled = true;
	}

	public void SwitcherDown () {
		SwitchLight.enabled = false;
	}
}
