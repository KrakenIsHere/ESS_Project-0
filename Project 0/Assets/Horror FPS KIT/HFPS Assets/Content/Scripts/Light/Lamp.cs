using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour {

	public Light switchLight;
	public bool isMaterialObject;
	public bool isFlickering;
	public bool isOn;

	[Header("Audio")]
	public AudioClip SwitchOn;
	public AudioClip SwitchOff;

	[HideInInspector]
	public bool canSwitchOn = true;

	void Update()
	{
		if (!isMaterialObject) {
			if (switchLight.enabled) {
				switchLight.transform.parent.gameObject.GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", new Color (1f, 1f, 1f));
			} else {
				switchLight.transform.parent.gameObject.GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", new Color (0f, 0f, 0f));
			}
		} else {
			if (switchLight.enabled) {
				GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", new Color (1f, 1f, 1f));
			} else {
				GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", new Color (0f, 0f, 0f));
			}
		}

		if (isOn) {
			switchLight.enabled = true;
		} else {
			switchLight.enabled = false;
		}
	}

	public void UseObject()
	{
		if (!isOn) {
			if (isFlickering) {
				switchLight.GetComponent<Animation> ().wrapMode = WrapMode.Loop;
				switchLight.GetComponent<Animation> ().Play ();
			}
			switchLight.enabled = true;
			AudioSource.PlayClipAtPoint (SwitchOn, this.transform.position, 0.75f);
			isOn = true;
		} else {
			if (isFlickering) {
				switchLight.GetComponent<Animation> ().Stop ();
			}
			switchLight.enabled = false;
			AudioSource.PlayClipAtPoint (SwitchOff, this.transform.position, 0.75f);
			isOn = false;
		}
	}
}
