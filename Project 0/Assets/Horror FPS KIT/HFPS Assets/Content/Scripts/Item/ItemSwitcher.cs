/* ItemSwitcher.cs by ThunderWire Games - Script only for Switching Items */
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ItemSwitcher : MonoBehaviour {

	public List<GameObject> ItemList = new List<GameObject>();

	[HideInInspector]
	public int currentItem = 0;
	private int newItem = 0;

	public void selectItem(int id)
	{
		newItem = id;
		if (!CheckActiveItem()) {
			SelectItem ();
		} else {
			StartCoroutine (SwitchItem ());
		}
	}

	public void DeselectItems()
	{
		ItemList [currentItem].SendMessage ("Deselect", SendMessageOptions.DontRequireReceiver);
	}

	bool CheckActiveItem()
	{
		for (int i = 0; i < ItemList.Count; i++) {
			ActiveState ACState = ItemList [i].GetComponent<ActiveState> ();
			if (ACState.activeState())
				return true;
		}
		return false;
	}

	IEnumerator SwitchItem()
	{
		ItemList [currentItem].SendMessage ("Deselect", SendMessageOptions.DontRequireReceiver);

		yield return new WaitUntil (() => ItemList [currentItem].GetComponent<ActiveState> ().activeState () == false);

		ItemList [newItem].SendMessage ("Select", SendMessageOptions.DontRequireReceiver);
		currentItem = newItem;
	}

	void SelectItem()
	{
		ItemList [newItem].SendMessage ("Select", SendMessageOptions.DontRequireReceiver);
		currentItem = newItem;
	}
}