using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRemovalController : MonoBehaviour {

    public bool isGone;
	
	void Update ()
    {
	    if (isGone)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
	}
}
