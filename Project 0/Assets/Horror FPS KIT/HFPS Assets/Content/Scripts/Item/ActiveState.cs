using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveState : MonoBehaviour {

	public GameObject ItemGO;

	public bool activeState()
	{
		return ItemGO.activeSelf;
	}
}
