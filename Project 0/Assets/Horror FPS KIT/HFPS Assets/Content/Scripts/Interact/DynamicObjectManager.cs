using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectManager : MonoBehaviour {

	private InputManager inputManager;
	private UIManager uiManager;
	private DynamicObjectScript dynamic;

	[Header("Raycast")]
	public float RayLength;
	public LayerMask CullLayers;
	public string InteractLayer = "Interact";
	public string DynamicObjectTag;

	[Header("Settings")]
	public float moveDoorSpeed;
	public float moveDrawerSpeed;
	public float moveLeverSpeed;

	public bool isHeld;
	public bool isPressed;

	private DelayEffect delay;

	private float mouse_x;
	private float mouse_y;
	private float mouselever_y;

	private bool firstGrab;

	private bool isDynamic;
	private bool isDoor;

	private JointMotor motor;
	private HingeJoint joint;

	//Door
	private float doorRot;

	//Drawer
	float mouseY = 0.0f;

	//Lever
	private bool isLever;
	private bool otherHeld;
	private bool enableLock;

	private GameObject objectRaycast;
	private KeyCode UseKey;
	private Ray playerAim;
	private bool isSet;

	void SetKeys()
	{
		UseKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Use"));
		isSet = true;
	}

	void Start()
	{
		inputManager = GetComponent<ScriptManager>().inputManager;
		uiManager = GetComponent<ScriptManager>().uiManager;
		delay = gameObject.transform.GetChild (0).GetChild (1).GetChild (0).GetChild (0).GetChild (0).gameObject.GetComponent<DelayEffect> ();
	}

	void Update () {
		if(inputManager.DictCount() > 0 && !isSet)
		{
			SetKeys();
		}

		if (inputManager.GetRefreshStatus () && isSet) {
			isSet = false;
		}

		//Prevent Interact Dynamic Object when player is holding other object
		otherHeld = GetComponent<DragRigidbody> ().CheckHold ();

		if(objectRaycast && !otherHeld && isDynamic)
		{
			if(Input.GetKey(UseKey)){
				isHeld = true;
			}else if(isHeld){
				isHeld = false;
			}
		}

		if (isHeld){
			if (!firstGrab){
				grabObject();
			}else{
				if (isDoor) {
					grabDoor ();
				} else if (!isLever) {
					grabDrawer ();
				} else if (isLever) {
					grabLever ();
				}
			}
		}else if(firstGrab){
			Release ();
		}

		if(isHeld)
		{
			uiManager.MouseLookState (false);
			delay.isEnabled = false;
			if (objectRaycast && objectRaycast.GetComponent<DynamicObjectScript> ()) {
				objectRaycast.GetComponent<DynamicObjectScript> ().isHolding = true;
			}
		}else{
			uiManager.MouseLookState (true);
			if (objectRaycast && objectRaycast.GetComponent<DynamicObjectScript> ()) {
				objectRaycast.GetComponent<DynamicObjectScript> ().isHolding = false;
			}
		}

		mouse_x = Input.GetAxis("Mouse X")  * moveDoorSpeed;
		mouse_y = Input.GetAxis("Mouse Y");
		mouselever_y = Input.GetAxis("Mouse Y") * moveLeverSpeed;

		Ray playerAim = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;

		if(Physics.Raycast (playerAim, out hit, RayLength, CullLayers) && !isHeld){
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer (InteractLayer)) {
				objectRaycast = hit.collider.gameObject;
				if (objectRaycast.GetComponent<DynamicObjectScript> () && objectRaycast.tag == DynamicObjectTag) {
					dynamic = objectRaycast.GetComponent<DynamicObjectScript> ();
					isDynamic = true;
				} else {
					isDynamic = false;
				}
			} else if(objectRaycast && !isHeld && !firstGrab){
				isDoor = false;
				isLever = false;
				objectRaycast = null;
			}
		}
		else if(objectRaycast && !isHeld && !firstGrab)
		{
			isDoor = false;
			isLever = false;
			objectRaycast = null;
		}

		if (objectRaycast && !isHeld && joint) {
			if (isLever || isDoor) {
				motor.targetVelocity = 0;
				joint.useMotor = false;
				joint.motor = motor;
			}
		}
	}

	void grabObject()
	{
		if (dynamic.dynamicType == DynamicObjectScript.dynamic.HingeDoor) {
			isDoor = true;
			isLever = false;
		} else if (dynamic.dynamicType == DynamicObjectScript.dynamic.Drawer) {
			isDoor = false;
			isLever = false;
		} else if (dynamic.dynamicType == DynamicObjectScript.dynamic.Lever) {
			isLever = true;
			isDoor = false;
		} else if (dynamic.dynamicType == DynamicObjectScript.dynamic.MovableInteract) {
			isDoor = false;
			isLever = false;
		}
		firstGrab = true;
	}

	private void grabDoor()
	{
		if (dynamic) {
			if (dynamic.useType == DynamicObjectScript.type.Locked && !dynamic.hasKey) {
				if (string.IsNullOrEmpty(dynamic.CustomLockedText)) {
					uiManager.ShowHint ("The door is locked, you need a Key to open!");
				} else {
					uiManager.ShowHint (dynamic.CustomLockedText);
				}
				return;
			} else if (dynamic.useType == DynamicObjectScript.type.Jammed) {
				uiManager.ShowHint ("The door is Jammed.");
				return;
			}
		}

		joint = objectRaycast.GetComponent<HingeJoint>();
		motor = joint.motor;
		motor.targetVelocity = mouse_x;
		motor.force = moveDoorSpeed;
		joint.motor = motor;
		joint.useMotor = true;
	}

	private void grabDrawer()
	{
		bool vectoMoveX = dynamic.moveX;

		if (dynamic) {
			if (dynamic.useType == DynamicObjectScript.type.Locked && !dynamic.hasKey) {
				if (string.IsNullOrEmpty(dynamic.CustomLockedText)) {
					uiManager.ShowHint ("The drawer is locked, you need a Key to open!");
				} else {
					uiManager.ShowHint (dynamic.CustomLockedText);
				}
				return;
			} else if (dynamic.useType == DynamicObjectScript.type.Jammed) {
				uiManager.ShowHint ("The drawer is Jammed.");
				return;
			}
		}

        if(dynamic.dynamicType == DynamicObjectScript.dynamic.MovableInteract)
        {
            float interactPos = dynamic.InteractPos;
			if (vectoMoveX) {
				if (objectRaycast.transform.localPosition.z > interactPos) {
					dynamic.InteractObject.SendMessage ("Interact", SendMessageOptions.DontRequireReceiver);
				}
			} else {
				if (objectRaycast.transform.localPosition.x > interactPos) {
					dynamic.InteractObject.SendMessage ("Interact", SendMessageOptions.DontRequireReceiver);
				}
			}
        }

		if (dynamic.reverseMove) {
			mouseY = (-mouse_y * moveDrawerSpeed);
		} else {
			mouseY = (mouse_y * moveDrawerSpeed);
		}
			
		if (!vectoMoveX) {
			Vector3 move = new Vector3 (0, 0, mouseY);
			Vector3 pos = objectRaycast.transform.localPosition;
			objectRaycast.transform.localPosition = new Vector3 (pos.x, pos.y, Mathf.Clamp (pos.z, dynamic.MinMove, dynamic.MaxMove));
			objectRaycast.transform.Translate (move * Time.deltaTime);
		} else {
			Vector3 move = new Vector3 (mouseY, 0, 0);
			Vector3 pos = objectRaycast.transform.localPosition;
			objectRaycast.transform.localPosition = new Vector3 (Mathf.Clamp (pos.x, dynamic.MinMove, dynamic.MaxMove), pos.y, pos.z);
			objectRaycast.transform.Translate (move * Time.deltaTime);
		}
	}

	private void grabLever ()
	{
		joint = objectRaycast.GetComponent<HingeJoint>();
		motor = joint.motor;
		motor.targetVelocity = mouselever_y;
		motor.force = moveLeverSpeed;
		joint.motor = motor;
		joint.useMotor = true;
	}

	void Release()
	{		
		delay.isEnabled = true;
		dynamic = null;
		firstGrab = false;
	}
}
