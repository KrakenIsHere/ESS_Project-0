using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotation : MonoBehaviour {


	void Update () {
		Debug.Log (transform.eulerAngles + "  " + transform.eulerAngles.y);
	}
}
