using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer_Controller : MonoBehaviour {

    public float speed;
    Rigidbody GetRigidbody;

	// Use this for initialization
	void Start ()
    {
        GetRigidbody = GetComponent<Rigidbody>();

        GetRigidbody.velocity = transform.forward * speed;
	}

}
