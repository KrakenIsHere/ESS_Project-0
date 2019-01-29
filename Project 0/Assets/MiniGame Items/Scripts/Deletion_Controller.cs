using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deletion_Controller : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        Destroy(other);
    }
}
