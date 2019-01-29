using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadButton : MonoBehaviour {

	public int number;
	private Keypad keypad;

	void Start()
	{
		keypad = transform.parent.GetComponent<Keypad> ();
	}

	public void UseObject () {
		keypad.InsertCode (number);
	}
}
