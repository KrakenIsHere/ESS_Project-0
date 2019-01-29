using UnityEngine;
using System.Collections;

public class FlashlightPickup : MonoBehaviour {
	
	private FlashlightScript FlashlightComponent;

	public AudioClip pickupSound;

	
	void Start () {
		FlashlightComponent = Camera.main.transform.root.gameObject.GetComponent<PlayerController>().playerWeapGO.GetChild (0).GetComponent<FlashlightScript>();
	}

    public void UseObject (){
		FlashlightComponent.PickupFlashlight();
		
		this.GetComponent<Renderer>().enabled = false;
		this.GetComponent<Collider>().enabled = false;
		
        if(pickupSound){AudioSource.PlayClipAtPoint(pickupSound, transform.position, 0.75f);}//Main Audio		 
	}
}
