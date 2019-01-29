using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTurret_Controller : MonoBehaviour
{
    [Header("Self handeling")]
    public bool activated;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        activated = false;
    }
}
