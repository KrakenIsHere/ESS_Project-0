using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour {

	public GameObject EventObject;
	public string eventToSend;

	public void SendEvent () {
		EventObject.SendMessage(eventToSend, SendMessageOptions.DontRequireReceiver);
	}
}
