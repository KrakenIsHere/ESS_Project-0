using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GavityInverter_Controller : MonoBehaviour
{
    AntiGravityObject_Controller otherController;
    public bool emissionOn = false;

    public Material GetMaterial;

    Color emissionActive;
    Color emissionInactive;

    private void Start()
    {
        emissionActive = new Color(0.08f, 0,1f, 1f);
        emissionInactive = new Color(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        otherController = other.GetComponent<AntiGravityObject_Controller>();

        EmitionToggle(GetMaterial);

        otherController.usingAntiGravity = BoolToggle(otherController.usingAntiGravity);
    }

    private void OnTriggerExit(Collider other)
    {
        EmitionToggle(GetMaterial);
    }

    bool BoolToggle(bool toToggle)
    {
        return toToggle = !toToggle;
    }

    void EmitionToggle(Material material)
    {
        emissionOn = !emissionOn;

        switch (emissionOn)
        {
            case true:
                {
                    material.SetColor("_EmissionColor", emissionActive);
                    break;
                }
            case false:
                {
                    material.SetColor("_EmissionColor", emissionInactive);
                    break;
                }
        }
    }
}
