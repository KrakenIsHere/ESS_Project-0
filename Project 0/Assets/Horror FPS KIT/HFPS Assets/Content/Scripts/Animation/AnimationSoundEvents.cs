using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class events {
	public string eventName;
	public AudioClip eventSound;
}

public class AnimationSoundEvents : MonoBehaviour {

	public float soundVolume = 0.75f;
	public List<events> SoundEvents = new List<events> ();

	public void EventPlaySound (string SoundEvent) {
		for (int i = 0; i < SoundEvents.Count; i++) {
			if (SoundEvents [i].eventName == SoundEvent) {
				if(SoundEvents[i].eventSound){
					AudioSource.PlayClipAtPoint(SoundEvents[i].eventSound, transform.position, soundVolume);
				}
			}
		}
	}
}
