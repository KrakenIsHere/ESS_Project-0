using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLamp : MonoBehaviour {

	public bool isFilckering;
	public bool isOn;

	void Start()
	{
		if (!isFilckering) {
			GetComponent<Light> ().GetComponent<Animation> ().playAutomatically = false;
			GetComponent<Light> ().GetComponent<Animation> ().Stop ();
			GetComponent<Light> ().GetComponent<Animation> ().enabled = false;
		}

		if (isFilckering && isOn) {
			GetComponent<Light> ().GetComponent<Animation> ().wrapMode = WrapMode.Loop;
			GetComponent<Light> ().GetComponent<Animation> ().Play ();
		}
	}

	void Update () {
		if (GetComponent<Light>().enabled) {
			GetComponent<Light>().transform.parent.gameObject.GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", new Color (1f, 1f, 1f));
		} else {
			GetComponent<Light>().transform.parent.gameObject.GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", new Color (0f, 0f, 0f));
		}
	}

	public void LightState(bool State)
	{
		switch (State) {
		case true:
			GetComponent<Light> ().enabled = true;
			if (isFilckering) {
				GetComponent<Light> ().GetComponent<Animation> ().enabled = true;
				GetComponent<Light> ().GetComponent<Animation> ().Play ();
			}
			break;
		case false:
			if (isFilckering) {
				GetComponent<Light> ().GetComponent<Animation> ().Stop ();
				GetComponent<Light> ().GetComponent<Animation> ().enabled = false;
			}
			GetComponent<Light>().enabled = false;
			break;
		}
	}
}
