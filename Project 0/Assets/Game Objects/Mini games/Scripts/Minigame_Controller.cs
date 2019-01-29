using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Controller : MonoBehaviour
{

    Camera MainCamera;

	// Use this for initialization
	void Start ()
    {
        MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
