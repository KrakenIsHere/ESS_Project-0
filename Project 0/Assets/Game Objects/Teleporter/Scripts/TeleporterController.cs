using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterController : MonoBehaviour {

    [Header("Audio:")]
    [Tooltip("Audio that plays when an object is teleported to somewhere else")]
    public AudioSource disapearAudio;
    [Tooltip("Audio that plays when an object is teleported to it")]
    public AudioSource apearAudio;

    [Header("Other:")]
    [Tooltip("The Collider Script of the Teleporter you wish this one to link with")]
    public TeleporterController TeleporterToTeleportTo;
    [Tooltip("Set to active if multible objects will travel through this teleporter\nGives Better Functionality")]
    public bool useAddPos2;
    [Tooltip("Shows if an object was teleported to this or not")]
    public bool wasTeleportedTo = false;
    public bool oneWay = false;


    Vector3 thisPos;
    Vector3 addPos;
    Vector3 addPos2;

    [Header("addPos Vars:")]
    [Range(-1000.0f, 1000.0f)]
    [Tooltip("addPos's X variable ")]
    public float addpos_X;
    [Range(-1000.0f, 1000.0f)]
    [Tooltip("addPos's Y variable ")]
    public float addpos_Y;
    [Range(-1000.0f, 1000.0f)]
    [Tooltip("addPos's Z variable ")]
    public float addpos_Z;

    private float addposX;
    private float addposY;
    private float addposZ;

    private void Start()
    {
        addposX = TeleporterToTeleportTo.addpos_X;
        addposY = TeleporterToTeleportTo.addpos_Y;
        addposZ = TeleporterToTeleportTo.addpos_Z;

        disapearAudio.GetComponentInParent<AudioSource>();
        apearAudio.GetComponent<AudioSource>();

        addPos = new Vector3(addposX, addposY, addposZ);
    }

    private void Status()
    {
        TeleporterToTeleportTo.wasTeleportedTo = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (wasTeleportedTo)
        {
            case true:
                {
                    if (!oneWay)
                    {
                        apearAudio.Play();
                    }
                    break;
                }
            case false:
                {
                    thisPos = TeleporterToTeleportTo.transform.position;

                    thisPos += addPos;

                    switch (useAddPos2)
                    {
                        case true:
                            {
                                addPos2 = other.transform.localScale;
                                thisPos = thisPos + addPos2;
                                break;
                            }
                        case false:
                            {
                                break;
                            }

                    }

                    Status();
                    other.transform.position = thisPos;
                    disapearAudio.Play();
                    break;
                }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!oneWay)
        {
            wasTeleportedTo = false;
        }
    }
}
