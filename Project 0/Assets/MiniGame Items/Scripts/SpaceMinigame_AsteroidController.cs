using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceMinigame_AsteroidController : MonoBehaviour
{
    public float RotationVelocity;
    Rigidbody rigidbody;
    Collider ship;

    bool thisHit;
    public bool triggered;

    private void Update()
    {
        if (thisHit && triggered)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ship = other;
        }
        thisHit = true;
    }

    private void OnTriggerExit(Collider other)
    {
        thisHit = false;
    }
}
