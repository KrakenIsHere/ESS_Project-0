/* Jumpscare.cs 06.04.17 - Jumpscare Manager */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare : MonoBehaviour {

	private JumpscareEffects effects;

	[Header("Jumpscare Setup")]
	public GameObject AnimationObject;
	public AudioClip AnimationSound;
	public float SoundVolume = 0.5f;

	[Tooltip("Value sets how long will be player scared.")]
	public float ScareLevelSec = 33f;

	private bool isPlayed;

	void Start()
	{
		effects = Camera.main.transform.parent.transform.parent.gameObject.GetComponent<JumpscareEffects> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !isPlayed) {
			AnimationObject.GetComponent<Animation> ().Play ();
			if(AnimationSound){AudioSource.PlayClipAtPoint(AnimationSound, Camera.main.transform.position, SoundVolume);}
			effects.Scare (ScareLevelSec);
			isPlayed = true;
		}
	}
}
