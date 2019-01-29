using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSide_Controller : MonoBehaviour
{
    public Renderer sideRendere;

    public int SideNum;
    public int OtherSide;

    public bool isDown = false;

    private void Start()
    {
        sideRendere = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        isDown = true;
        ConsoleProDebug.Watch(SideNum + " is down now", isDown.ToString());
    }

    private void OnTriggerExit(Collider other)
    {
        isDown = false;
        ConsoleProDebug.Watch(SideNum + " is down now", isDown.ToString());
    }
}
