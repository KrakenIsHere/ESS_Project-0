using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExamineManager : MonoBehaviour {

	private InputManager inputManager;
	private InteractManager interact;
	private UIManager uiManager;
	private PlayerFunctions pfunc;
	private GameObject paperUI;
	private Text paperText;
	private DelayEffect delay;

	[Header("Raycast")]
	public LayerMask CullLayers;
	public string InteractLayer = "Interact";
	public string TagExamine = "Examine";
	public string TagExaminePaper = "Paper";
	public float PickupRange = 3f; 
	public float rotationDeadzone = 0.1f;
	public float rotateSpeed = 10f;
	public float spamWaitTime = 0.5f;

    private string examineName;

	private bool rotSet;
	public bool isPaper;
	private bool isPressedRead;
	private bool isReaded;

	private bool antiSpam;
	private bool isPressed;
	private bool isObjectHeld;
	private bool isExaminig;
	private bool tryExamine;
	private bool otherHeld;
	private bool objectUsable;

	private Vector3 objectPosition;
	private Quaternion objectRotation;
	private float distance;

	private Vector3 rotateAngle;

	private GameObject objectRaycast;
	private GameObject objectHeld;	
	private Camera playerCam;

	private Ray playerAim;

	private KeyCode rotateKey;
	private KeyCode useKey;
	private KeyCode examineKey;
	private KeyCode useKey2;

	private ExamineItem examinedItem;
	private bool isSet;

	void SetKeys()
	{
		useKey = (KeyCode)System.Enum.Parse (typeof(KeyCode), inputManager.GetInput ("Use"));
		useKey2 = (KeyCode)System.Enum.Parse (typeof(KeyCode), inputManager.GetInput ("Grab"));
		rotateKey = (KeyCode)System.Enum.Parse (typeof(KeyCode), inputManager.GetInput ("Fire"));
		examineKey = (KeyCode)System.Enum.Parse (typeof(KeyCode), inputManager.GetInput ("Examine"));
	}

	void Start () {
		if (GetComponent<ScriptManager> () && GetComponent<InteractManager> () && GetComponent<PlayerFunctions> ()) {
			inputManager = GetComponent<ScriptManager> ().inputManager;
			uiManager = GetComponent<ScriptManager> ().uiManager;
			interact = GetComponent<InteractManager> ();
			pfunc = GetComponent<PlayerFunctions> ();
			paperUI = uiManager.PaperTextUI;
			paperText = uiManager.PaperReadText;
		} else {
			Debug.LogError ("Missing one or more scripts in " + gameObject.name);
		}
		delay = gameObject.transform.GetChild (0).GetChild (1).GetChild (0).GetChild (0).GetChild (0).gameObject.GetComponent<DelayEffect> ();
		playerCam = Camera.main;
	}
	

	void Update ()
	{
		if(inputManager.DictCount() > 0 && !isSet)
		{
			SetKeys();
		}

		if (inputManager.GetRefreshStatus () && isSet) {
			isSet = false;
		}

		//Prevent Interact Dynamic Object when player is holding other object
		otherHeld = GetComponent<DragRigidbody> ().CheckHold ();

		if (objectRaycast && !antiSpam && examinedItem) {
			if (Input.GetKeyDown (useKey) && !isPressed && !otherHeld && !examinedItem.isUsable) {
				isPressed = true;
				isExaminig = !isExaminig;
			} else if (isPressed) {
				
				isPressed = false;
			}
			if (Input.GetKeyDown (useKey2) && !isPressed && !otherHeld && examinedItem.isUsable) {
				isPressed = true;
				isExaminig = !isExaminig;
			} else if (isPressed) {
				isPressed = false;
			}
		}

		if (isExaminig){
			if (!isObjectHeld){
				firstGrab();
				tryExamine = true;
			}else{
				holdObject();
			}
		}else if(isObjectHeld){
			dropObject();
		}

		RaycastHit hit;
		playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

		if (Physics.Raycast (playerAim, out hit, PickupRange, CullLayers)) {
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer (InteractLayer)) {
				if (hit.collider.tag == TagExamine || hit.collider.tag == TagExaminePaper) {
					objectRaycast = hit.collider.gameObject;
					if (objectRaycast.GetComponent<ExamineItem> ()) {
						examinedItem = objectRaycast.GetComponent<ExamineItem> ();
					}
				} else {
					if (!tryExamine) {
						objectRaycast = null;
						examinedItem = null;
					}
				}
			} else {
				if (!tryExamine) {
					objectRaycast = null;
					examinedItem = null;
				}
			}
		} else {
			if (!tryExamine) {
				objectRaycast = null;
				examinedItem = null;
			}
		}

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
			
		if (objectHeld && isObjectHeld) {
			if (Input.GetKey (rotateKey)) {
				objectHeld.transform.Rotate (playerCam.transform.up, rotationInputX, Space.World);
				objectHeld.transform.Rotate (playerCam.transform.right, rotationInputY, Space.World);
			}

			if (Input.GetKey (examineKey) && !isPaper) {
				uiManager.ShowHint (examineName);
			}

			if (isPaper) {
				if(Input.GetKeyDown(examineKey) && !isPressedRead){
					isPressedRead = true;
					isReaded = !isReaded;
				}else if(isPressedRead){
					isPressedRead = false;
				}

				if (isReaded) {
					string text = objectRaycast.GetComponent<ExamineItem>().PaperReadTexts;
					paperText.text = text;
					paperUI.SetActive (true);
				} else {
					paperUI.SetActive (false);
				}
			}
		}
	}

	void firstGrab()
	{
		if(objectRaycast.tag == TagExamine || objectRaycast.tag == TagExaminePaper)
		{
			StartCoroutine (AntiSpam ());
			objectHeld = objectRaycast.gameObject;
			if (objectRaycast.tag == TagExaminePaper) {
				isPaper = true;
			} else {
				isPaper = false;
			}

			if (objectHeld.GetComponent<ExamineItem> ()) {
				examinedItem = objectHeld.GetComponent<ExamineItem> ();
				distance = examinedItem.examineDistance;
				examineName = examinedItem.examineObjectName;
				if (examinedItem.examineSound) {
					AudioSource.PlayClipAtPoint(examinedItem.examineSound, objectRaycast.transform.position, 0.75f);
				}
			}

			if(!(objectHeld.GetComponent<Rigidbody>())){
				Debug.LogError (objectHeld.name + " need Rigidbody Component to pickup");
				return;
			}

			if (!isObjectHeld) {
				objectPosition = objectHeld.transform.position;
				objectRotation = objectHeld.transform.rotation;
				objectHeld.GetComponent<Collider> ().isTrigger = true;
			}

			if (uiManager.gameObject.GetComponent<UIFloatingItem> ().AllItemsList.Contains (objectHeld)) {
				uiManager.gameObject.GetComponent<UIFloatingItem> ().SetItemVisible (objectHeld, false);
			}

			if (objectHeld.transform.childCount > 1) {
				objectHeld.layer = LayerMask.NameToLayer ("Examine");
				foreach (Transform child in objectHeld.transform) {
					if (child.GetComponent<MeshFilter> ()) {
						child.gameObject.layer = LayerMask.NameToLayer ("Examine");
					}
				}
			} else {
				objectHeld.layer = LayerMask.NameToLayer ("Examine");
			}

			delay.isEnabled = false;
			Physics.IgnoreCollision(objectRaycast.GetComponent<Collider>(), this.transform.root.GetComponent<Collider>(), true);
            isObjectHeld = true;
        }
	}

	void holdObject()
	{
		if (examinedItem.isUsable) {
			uiManager.ShowExamineSprites (useKey2.ToString (), examineKey.ToString ());
		} else {
			uiManager.ShowExamineSprites (useKey.ToString (), examineKey.ToString ());
		}

		uiManager.HideSprites ("Interact");
		pfunc.Enabled (false);

		Vector3 nextPos = playerCam.transform.position + playerAim.direction * distance;
		Vector3 currPos = objectRaycast.transform.position;

		interact.CrosshairVisible (false);
		uiManager.LockStates (true, true, true, false, true);

		objectHeld.GetComponent<Rigidbody> ().isKinematic = false;
		objectHeld.GetComponent<Rigidbody> ().useGravity = false;
		objectHeld.GetComponent<Rigidbody> ().velocity = (nextPos - currPos) * 10;

		if (!rotSet && isPaper) {
			Vector3 rotation = objectRaycast.GetComponent<ExamineItem> ().paperRotation;
			objectRaycast.transform.rotation = Quaternion.LookRotation (nextPos - currPos) * Quaternion.Euler (rotation);
			rotSet = true;
		}
	}
		

	void dropObject()
	{
		if (uiManager.gameObject.GetComponent<UIFloatingItem> ().AllItemsList.Contains (objectHeld)) {
			uiManager.gameObject.GetComponent<UIFloatingItem> ().SetItemVisible (objectHeld, true);
		}
		distance = 0;
		pfunc.Enabled (true);
        uiManager.HideSprites ("Examine");
		if (objectHeld.transform.childCount > 1) {
			objectHeld.layer = LayerMask.NameToLayer ("Interact");
			foreach (Transform child in objectHeld.transform) {
				if (child.GetComponent<MeshFilter> ()) {
					child.gameObject.layer = LayerMask.NameToLayer ("Interact");
				}
			}
		} else {
			objectHeld.layer = LayerMask.NameToLayer ("Interact");
		}
		examinedItem = null;
        examineName = null;
        isObjectHeld = false;
		isExaminig = false;
		rotSet = false;
		isReaded = false;
		paperUI.SetActive (false);
        interact.CrosshairVisible (true);
		uiManager.LockStates (false, true, true, false, true);
		objectHeld.transform.position = objectPosition;
		objectHeld.transform.rotation = objectRotation;
		if (!isPaper) {
			objectHeld.GetComponent<Collider> ().isTrigger = false;
			objectHeld.GetComponent<Rigidbody> ().isKinematic = false;
			objectHeld.GetComponent<Rigidbody> ().useGravity = true;
		} else {
			objectHeld.GetComponent<Rigidbody> ().isKinematic = true;
			objectHeld.GetComponent<Rigidbody> ().useGravity = false;
		}
		tryExamine = false;
		objectRaycast = null;
		objectHeld = null;
		delay.isEnabled = true;
		StartCoroutine (AntiSpam ());
	}

	IEnumerator AntiSpam()
	{
		antiSpam = true;
		yield return new WaitForSeconds (spamWaitTime);
		antiSpam = false;
	}
}