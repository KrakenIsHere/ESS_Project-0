using UnityEngine;
using System.Collections;

public class BatteryPickup : MonoBehaviour {

	private FlashlightScript BatteryComponent;

	public int BatteryQuantity = 1;	
	public AudioClip pickupSound;
	
	void Start () {
		BatteryComponent = Camera.main.transform.root.GetComponent<PlayerController> ().playerWeapGO.GetComponent<ItemSwitcher> ().ItemList [0].GetComponent<FlashlightScript> ();
	}
	 
	public void UseObject (){
		BatteryComponent.AddBattery (BatteryQuantity);
		if(BatteryComponent.canPickup)
		{
			if(pickupSound){AudioSource.PlayClipAtPoint(pickupSound, transform.position, 0.75f);}
			this.GetComponent<Renderer>().enabled = false;
			this.GetComponent<Collider>().enabled = false;			
		}
			
	}
}