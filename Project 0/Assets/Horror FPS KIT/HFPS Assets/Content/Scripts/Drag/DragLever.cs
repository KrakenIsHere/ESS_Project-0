using UnityEngine;
using System.Collections;

public class DragLever : MonoBehaviour {

    private Camera playerCam;
    private InputManager inputManager;

	public bool debugAngle;

    [Header("Raycast")]
    public LayerMask GrabMask;
    public string GrabTag = "GrabLever";
    public float PickupRange = 3f;
	public float switchSpeed = 10f;

	private float y;
	private float currentAngle;
	private float angleStop;		

    private GameObject objectRaycast;

	private JointMotor motor;
    private HingeJoint hinge;
	private JointLimits limits;
	
	private Ray playerAim;
	private bool isHeld;
	private bool isUp;
	
    private KeyCode GrabButton;

	void Start () {
        inputManager = GetComponent<ScriptManager>().inputManager;
        playerCam = Camera.main;
        objectRaycast = null;	
	}
	
	void Update()
	{
		if(inputManager.DictCount() > 0)
		{
			GrabButton = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Use"));
		}
		
		if(objectRaycast)
		{
			if(Input.GetKey(GrabButton)){
				grabLever();
				isHeld = true;
			}else if(isHeld){
				isHeld = false;
			}
		}

		if(isHeld)
		{
			objectRaycast.GetComponent<LeverAngle> ().hold = false;
			this.gameObject.transform.root.gameObject.GetComponent<MouseLook>().enabled = false;
			GetComponent<MouseLook>().enabled = false;
		}else{
			this.gameObject.transform.root.gameObject.GetComponent<MouseLook>().enabled = true;
			GetComponent<MouseLook>().enabled = true;
		}

		y = Input.GetAxis("Mouse Y")  * switchSpeed;

		if (objectRaycast && !objectRaycast.GetComponent<LeverAngle> ().DebugEulerAngle && !isHeld) {
			if (currentAngle >= angleStop && currentAngle > (angleStop - 5)) {
				objectRaycast.GetComponent<LeverAngle> ().hold = true;
				isUp = true;
				Debug.Log ("Lever Stop at: " + angleStop + " current: " + currentAngle);
			} else {
				objectRaycast.GetComponent<LeverAngle> ().hold = false;
				isUp = false;
			}
		}
		
		if(objectRaycast && isUp)
		{
			objectRaycast.GetComponent<LeverAngle>().SwitcherInteract.SendMessage("SwitcherUp", SendMessageOptions.DontRequireReceiver);
		}else if(objectRaycast){
			objectRaycast.GetComponent<LeverAngle>().SwitcherInteract.SendMessage("SwitcherDown", SendMessageOptions.DontRequireReceiver);
		}

		if (isHeld) 
		{
			currentAngle = objectRaycast.transform.eulerAngles.x;
		}

		Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;	

		if (Physics.Raycast (playerAim, out hit, PickupRange, GrabMask)){
			if(hit.collider.tag == GrabTag)
			{
				objectRaycast = hit.collider.gameObject;
				angleStop = objectRaycast.GetComponent<LeverAngle>().angleStop;
			}
		}
		else if(objectRaycast && !isHeld)
		{
			motor.targetVelocity = 0f;
			objectRaycast.GetComponent<HingeJoint>().useMotor = false;
			objectRaycast.GetComponent<HingeJoint>().motor = motor;
			objectRaycast = null;
		}
	}
	
	private void grabLever ()
	{
		hinge = objectRaycast.GetComponent<HingeJoint>();
		motor = hinge.motor;
		motor.targetVelocity = y;
		motor.force = switchSpeed;
		hinge.motor = motor;
		hinge.useMotor = true;
	}
}
