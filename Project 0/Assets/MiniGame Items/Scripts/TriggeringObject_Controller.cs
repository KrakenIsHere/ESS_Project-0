using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeringObject_Controller : MonoBehaviour
{
    SpaceMinigame_AsteroidController asteroidController;

	void Start ()
    {
        asteroidController = GetComponentInParent<SpaceMinigame_AsteroidController>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            asteroidController.triggered = true;
        }
    }
}
