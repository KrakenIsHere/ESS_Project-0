using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartBeat : MonoBehaviour {

	public GameObject m_animation;
	public string heartAnimation;
	public float beatSpeed = 1f;

	void Start () {
		m_animation.GetComponent<Animation>().Play (heartAnimation);
	}

	void Update () {
		m_animation.GetComponent<Animation>() [heartAnimation].speed = beatSpeed;
	}

	void OnEnable()
	{
		m_animation.GetComponent<Animation>().Play (heartAnimation);
	}
}
