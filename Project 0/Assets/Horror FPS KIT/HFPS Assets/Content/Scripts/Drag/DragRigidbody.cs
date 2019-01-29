/*
DragRigidbody.cs ver. 19.10 - wirted by ThunderWire Games * Script for Drag, Drop & Throw Rigidbody Objects
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragRigidbody : MonoBehaviour {
	
	private Camera playerCam;		
	private InteractManager interact;
	private InputManager inputManager;
	private UIManager uiManager;
	private PlayerFunctions pfunc;
	private DelayEffect delay;

	[Header("Drag")]
	public LayerMask CullLayers;
	public string GrabLayer = "Interact";
	public string GrabTag = "Grab";
    public string OnlyGrabTag = "OnlyGrab";
    public float PickupRange = 3f;
	public float ThrowStrength = 50f;
	public float minDistance = 1.5f;
	public float maxDistance = 3f;
	public float maxDistanceGrab = 4f;
	public float spamWaitTime = 0.5f;
	
	private float distance;
	
	[Header("Other")]
	public float rotateSpeed = 10f;
	public float rotationDeadzone = 0.1f;
	public float objectZoomSpeed = 3f;	
	public bool FreezeRotation = true;
	public bool enableObjectPull = true;
	public bool enableObjectRotation = true;
	public bool enableObjectZooming = true;
	
	private GameObject objectHeld;	
	
	private Ray playerAim;
	private GameObject objectRaycast;
	private bool GrabObject;
	private bool isObjectHeld;
	private bool tryPickupObject;
	private bool isPressed;
	private bool antiSpam;
	
	private KeyCode rotateObject;
	private KeyCode GrabButton;
	private KeyCode ThrowButton;
	private KeyCode ZoomButton;

	private bool isSet;

	void SetKeys()
	{
		GrabButton = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Grab"));
		ThrowButton = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Throw"));
		ZoomButton = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Throw"));
		rotateObject = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Fire"));
		isSet = true;
	}

	void Start () {
		delay = gameObject.transform.GetChild (0).GetChild (1).GetChild (0).GetChild (0).GetChild (0).gameObject.GetComponent<DelayEffect> ();
		interact = GetComponent<InteractManager>();
        inputManager = GetComponent<ScriptManager>().inputManager;
        uiManager = GetComponent<ScriptManager>().uiManager;
		pfunc = GetComponent<PlayerFunctions> ();
        playerCam = Camera.main;
		isObjectHeld = false;
		tryPickupObject = false;
		isPressed = false;
		objectHeld = null;
	}
	
	void Update()
	{
		uiManager.isHeld = this.objectHeld != false;
		interact.isHeld = this.objectHeld != false;

		if(inputManager.DictCount() > 0 && !isSet)
		{
			SetKeys();
		}

		if (inputManager.GetRefreshStatus () && isSet) {
			isSet = false;
		}

		if(objectRaycast && !antiSpam)
		{
			if(Input.GetKeyDown(GrabButton) && !isPressed){
				isPressed = true;
				GrabObject = !GrabObject;
			}else if(isPressed){
				isPressed = false;
			}
		}

		if (GrabObject){
			if (!isObjectHeld){
				tryPickObject();
				tryPickupObject = true;
			}else{
				holdObject();
			}
		}else if(isObjectHeld){
			DropObject();
		}

		if (Input.GetKey(ThrowButton) && isObjectHeld)
		{
			isObjectHeld = false;
			objectHeld.GetComponent<Rigidbody>().useGravity = true;
			ThrowObject();
		}

		if (ZoomButton == ThrowButton) {
			if (Input.GetKeyUp (ThrowButton)) {
				pfunc.Enabled (true);
			}
		}
		
		Ray playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;

		/*
		float x = Input.GetAxis("Mouse X")  * rotateSpeed;
		float y = Input.GetAxis("Mouse Y")  * rotateSpeed;
		Vector3 angle = new Vector3(-y, x, 0);
		*/

		float rotationInputX = 0.0f;
		float rotationInputY = 0.0f;

		float x = Input.GetAxis("Mouse X");
		float y = Input.GetAxis("Mouse Y");

		if(Mathf.Abs(x) > rotationDeadzone){
			rotationInputX = -(x * rotateSpeed);
		}

		if(Mathf.Abs(y) > rotationDeadzone){
			rotationInputY = (y * rotateSpeed);
		}
		
		if (Physics.Raycast (playerAim, out hit, PickupRange, CullLayers)) {
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer (GrabLayer)) {
				if (hit.collider.tag == GrabTag || hit.collider.tag == OnlyGrabTag) {
					objectRaycast = hit.collider.gameObject;
					uiManager.canGrab = true;
				}
			} else {
				if (!tryPickupObject) {
					objectRaycast = null;
					uiManager.canGrab = false;
				}
			}
		
			if (objectHeld) {
				if (Input.GetKey (rotateObject) && enableObjectRotation) {
					objectHeld.GetComponent<Rigidbody> ().freezeRotation = true;
					uiManager.LockStates (true, false, false, false, false);
					objectHeld.transform.Rotate (playerCam.transform.up, rotationInputX, Space.World);
					objectHeld.transform.Rotate (playerCam.transform.right, rotationInputY, Space.World);
				} else {
					uiManager.LockStates (false, false, false, false, false);
				}
				if (enableObjectZooming) {
					distance = Mathf.Clamp (distance, minDistance, maxDistance);
					distance += Input.GetAxis ("Mouse ScrollWheel") * objectZoomSpeed;
				}
			}
		} else {
			if (!tryPickupObject) {
				objectRaycast = null;
				uiManager.canGrab = false;
			}
		}
	}
	
	private void tryPickObject(){
		StartCoroutine (AntiSpam ());

		objectHeld = objectRaycast;

		if (!(objectHeld.GetComponent<Rigidbody> ())) {
			return;
		}
			
		if (enableObjectPull) {
			if (!objectHeld.GetComponent<DragDistance> ()) {
				float dist = Vector3.Distance (this.transform.position, objectHeld.transform.position);
				if (dist > maxDistance - 1f) {
					distance = minDistance + 0.25f;
				} else {
					distance = dist;
				}
			} else {
				if (!objectHeld.GetComponent<DragDistance> ().useRealDostance) {
					distance = objectHeld.GetComponent<DragDistance> ().dragDistance;
				} else {
					distance = Vector3.Distance (this.transform.position, objectHeld.transform.position);
				}
			}
		}

		objectHeld.GetComponent<Rigidbody> ().useGravity = false;

		if (FreezeRotation) {
			objectHeld.GetComponent<Rigidbody> ().freezeRotation = true;
		} else {
			objectHeld.GetComponent<Rigidbody> ().freezeRotation = false;
		}
		delay.isEnabled = false;
		Physics.IgnoreCollision (objectHeld.GetComponent<Collider>(), this.transform.root.GetComponent<Collider> (), true);

		isObjectHeld = true;
	}
	
	private void holdObject(){
        uiManager.ShowGrabSprites();
		uiManager.HideSprites ("Interact");
		interact.CrosshairVisible (false);
		pfunc.Enabled (false);

        Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		
		Vector3 nextPos = playerCam.transform.position + playerAim.direction * distance;
		Vector3 currPos = objectHeld.transform.position;
		
		objectHeld.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 7;
		
		if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > maxDistanceGrab)
		{
           DropObject();
		}
	}

	public bool CheckHold(){
		return isObjectHeld;
	}
	
    private void DropObject()
    {
		pfunc.Enabled (true);
		uiManager.HideSprites ("Grab");
		interact.CrosshairVisible (true);
		objectHeld.GetComponent<Rigidbody>().useGravity = true;
		objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
		Physics.IgnoreCollision(objectHeld.GetComponent<Collider>(), this.transform.root.GetComponent<Collider>(), false);
		delay.isEnabled = true;
		objectRaycast = null;
		objectHeld = null;
		isObjectHeld = false;
		tryPickupObject = false;
		GrabObject = false;
		isPressed = false;
    }
	
    private void ThrowObject()
    {
		if (!(ZoomButton == ThrowButton)) {
			pfunc.Enabled (true);
		}
		uiManager.HideSprites ("Grab");
		interact.CrosshairVisible (true);
        objectHeld.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * ThrowStrength * 10);
		objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
		Physics.IgnoreCollision(objectHeld.GetComponent<Collider>(), this.transform.root.GetComponent<Collider>(), false);
		delay.isEnabled = true;
		objectRaycast = null;
		objectHeld = null;
		isObjectHeld = false;
		tryPickupObject = false;
		GrabObject = false;
		isPressed = false;
    }

	IEnumerator AntiSpam()
	{
		antiSpam = true;
		yield return new WaitForSeconds (spamWaitTime);
		antiSpam = false;
	}
}
