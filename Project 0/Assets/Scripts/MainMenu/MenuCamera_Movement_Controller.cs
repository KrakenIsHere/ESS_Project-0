using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera_Movement_Controller : MonoBehaviour
{
    [Header("Vector3")]
    public Vector3 currentPos;
    public Vector3 currentRot;
    public Vector3 ActivatePos;
    [Header("Other")]
    public GameObject canvas;

    private void Start()
    {
        canvas.SetActive(false);
    }



    // Update is called once per frame
    void Update ()
    {
        currentPos = transform.position;
        currentRot = transform.rotation.eulerAngles;
        Check();
    }

    void Check()
    {
        if (transform.position == ActivatePos)
        {
            canvas.SetActive(true);
        }
    }
}
