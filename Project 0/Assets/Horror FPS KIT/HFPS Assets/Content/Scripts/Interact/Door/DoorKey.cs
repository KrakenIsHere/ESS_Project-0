using UnityEngine;
using System.Collections;

public class DoorKey : MonoBehaviour {

	private UIFloatingItem floatingItem;
	private UIManager uiManager;

	public string keyName = "Door Key";
	public int inventoryKeyID = -1;
	public AudioClip pickupSound;
	public DynamicObjectScript keyForDoor;

	void Start()
	{
		floatingItem = GameObject.Find ("GAMEMANAGER").GetComponent<UIFloatingItem> ();
		uiManager = GameObject.Find ("GAMEMANAGER").GetComponent<UIManager> ();
	}

	public void UseObject () 
	{
		if(pickupSound){AudioSource.PlayClipAtPoint(pickupSound, this.transform.position, 0.7f);}
		if(inventoryKeyID >= 0){
			uiManager.inventoryScript.AddItemToSlot (inventoryKeyID, 1);
			keyForDoor.keyID = inventoryKeyID;
			keyForDoor.inv = uiManager.inventoryScript;
		}
		keyForDoor.hasKey = true;
		uiManager.AddPickupMessage (keyName);
		floatingItem.RemoveFloatingObject (gameObject);
	}
}