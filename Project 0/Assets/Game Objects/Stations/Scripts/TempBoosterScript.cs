using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBoosterScript : MonoBehaviour
{
    Animation depleationAinme;

    bool isEmpty = false;

    private void Start()
    {
        depleationAinme = GetComponent<Animation>();
    }

    private void OnMouseDown()
    {
        ConsoleProDebug.Watch("Station ","Was Clicked");

        if (isEmpty == false)
        {
            depleationAinme.Play("fluidRemoval");
            isEmpty = true;
        }
    }
}
