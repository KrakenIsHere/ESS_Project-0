using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	
	[Header("Main Setup")]
	public CharacterController controller;
	public ScriptManager scriptManager;

	private InputManager inputManager;
	private HealthManager hm;
	private Footsteps footsteps;
	
	[Header("Movement Speed")]
    public int crouchSpeed = 4;
    public int walkSpeed = 6;
    public int runSpeed = 10;
    public float jumpSpeed = 8.0f;
	public float climbSpeed = 3.0f;
	public float climbRate = 0.5f;
	public float startWalkSpeed = 2.0f;
	public float stopWalkSpeed = 2.2f;
	public int transitionSpeed = 3;

	[Header("Player Gravity")]
    public float baseGravity = 24;
    public float proneGravity = 15;
	
	private float gravity = 24;
	
    private float normalFDTreshold = 8;
    private float proneFDTreshold = 4;
    private float fallingDamageThreshold;

    [Header("Player")]
    public float doorPush = 0.5f;
    public float fallDamageMultiplier = 5.0f;
	public float slideLimit = 45.0f;
	public bool airControl = false;
	public float rayDistance;
	
    private float normalHeight = 0.9f;
    private float crouchHeight = 0.2f;
	
    private float fallDistance;
    private float slideSpeed = 8.0f;
    private float antiBumpFactor = .75f;
    private float antiBunnyHopFactor = 1;
	private bool sliding = false;
    private bool falling = false;
	
	[Header("Animations")]
    public Animation walkRunAnim;
    public Animation cameraAnimations;
    private string runAnimation = "CameraRun";
    private string idleAnimation = "IdleAnimation";
	private float adjustAnimSpeed = 7.0f;
	
	[Header("Other")]
	public Transform playerWeapGO;
	public Transform cameraGO;
    public Transform fallEffect;
    public Transform fallEffectWeap;
	public AudioSource aSource;

    [HideInInspector]
    public int state = 0;	
	
    [HideInInspector]
    public bool onLadder = false;
	
    [HideInInspector]
    Vector3 moveDirection = Vector3.zero;	
	
    [HideInInspector]
    public bool run;
	
    [HideInInspector]
    public bool canRun = true;
	
	[HideInInspector]
    public bool grounded = false;

	[HideInInspector]
	public bool controllable = true;
	
    [HideInInspector]
    public float speed;
	
    [HideInInspector]
    public float velMagnitude;
	
	private Transform myTransform;
	private float distanceToObstacle;
	
    private RaycastHit hit;

	private float playTime = 0.0f;
	private float climbDownThreshold = -0.4f;
	private bool useLadder = true;		
	private Vector3 climbDirection = Vector3.up;
	private Vector3 lateralMove = Vector3.zero;
	private Vector3 ladderMovement = Vector3.zero;
    private Vector3 contactPoint;
    private Vector3 currentPosition;
    private Vector3 lastPosition;
    private float highestPoint;
	private int jumpTimer;
	
	private KeyCode ForwardKey;
	private KeyCode BackwardKey;
	private KeyCode LeftKey;
	private KeyCode RightKey;
	private KeyCode JumpKey;
	private KeyCode RunKey;
	private KeyCode CrouchKey;
	private KeyCode ZoomKey = KeyCode.Mouse1;
	
	private bool isSet;
	
	private float inputY; //Vertical Movement
	private float inputX; //Horizontal Movement
	
	private bool mfw;
	private bool mbw;
	private bool ml;
	private bool mr;


    void Start()
    {
		inputManager = scriptManager.inputManager;
		hm = GetComponent<HealthManager> ();
		footsteps = GetComponent<Footsteps> ();
        myTransform = transform;
        rayDistance = controller.height / 2 + 1.1f;
        slideLimit = controller.slopeLimit - .2f;
        walkRunAnim.wrapMode = WrapMode.Loop;
        walkRunAnim.Stop();
        cameraAnimations[runAnimation].speed = 0.8f;
    }
	
	void SetKeys()
	{
		ForwardKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Forward"));
		BackwardKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Backward"));
		LeftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Left"));
		RightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Right"));
		JumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Jump"));
		RunKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Run"));
		CrouchKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Crouch"));
		isSet = true;
	}
	
	void GetKeyAxis(string direction, string dir, int axis)
	{
		switch(direction)
		{
			case "Y":
				switch(dir)
				{
					case "fw":
						switch(axis)
						{
							case 1:
								if(inputY < 1){
									inputY += Time.deltaTime * startWalkSpeed;
									mfw = true;
								}else{
									inputY = 1f;
								}
							break;
							case 0:
								if(mfw)
								{
									if(inputY > 0){
										inputY -= Time.deltaTime * stopWalkSpeed;
									}else{
										inputY = 0f;
										mfw = false;
									}
								}
							break;
						}
					break;
					case "bw":
						switch(axis)
						{
							case -1:
								if(inputY > -1){
									inputY -= Time.deltaTime * startWalkSpeed;
									mbw = true;
								}else{
									inputY = -1f;
								}
							break;
							case 0:
								if(mbw){
									if(inputY < 0){
										inputY += Time.deltaTime * stopWalkSpeed;
									}else{
										inputY = 0f;
										mbw = false;
									}
								}
							break;
						}
					break;
				}
			break;
			case "X":
				switch(dir)
				{
					case "r":
						switch(axis)
						{
							case 1:
								if(inputX < 1){
									inputX += Time.deltaTime * startWalkSpeed;
									mr = true;
								}else{
									inputX = 1f;
								}
							break;
							case 0:
								if(mr)
								{
									if(inputX > 0){
										inputX -= Time.deltaTime * startWalkSpeed;
									}else{
										inputX = 0f;
										mr = false;
									}
								}
							break;
						}
					break;
					case "l":
						switch(axis)
						{
							case -1:
								if(inputX > -1){
									inputX -= Time.deltaTime * startWalkSpeed;
									ml = true;
								}else{
									inputX = -1f;
								}
							break;
							case 0:
								if(ml){
									if(inputX < 0){
										inputX += Time.deltaTime * startWalkSpeed;
									}else{
										inputX = 0f;
										ml = false;
									}
								}
							break;
						}
					break;
				}			
			break;
		}
	}

    void Update()
    {
		if(inputManager.DictCount() > 0 && !isSet)
		{
			SetKeys();
		}

		if (inputManager.GetRefreshStatus () && isSet) {
			isSet = false;
		}
		
        velMagnitude = controller.velocity.magnitude;
        //float inputX = Input.GetAxis("Horizontal");
        //float inputY = Input.GetAxis("Vertical");

		if (controllable) {
			if (Input.GetKey (ForwardKey)) {
				GetKeyAxis ("Y", "fw", 1);
			} else {
				GetKeyAxis ("Y", "fw", 0);
			}
		
			if (Input.GetKey (BackwardKey)) {
				GetKeyAxis ("Y", "bw", -1);
			} else {
				GetKeyAxis ("Y", "bw", 0);
			}
		
			if (Input.GetKey (LeftKey)) {
				GetKeyAxis ("X", "l", -1);
			} else {
				GetKeyAxis ("X", "l", 0);
			}
		
			if (Input.GetKey (RightKey)) {
				GetKeyAxis ("X", "r", 1);
			} else {
				GetKeyAxis ("X", "r", 0);
			}
		} else {
			GetKeyAxis ("Y", "fw", 0);
			GetKeyAxis ("Y", "bw", 0);
			GetKeyAxis ("X", "l", 0);
			GetKeyAxis ("X", "r", 0);
		}
		
        float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f) ? .7071f : 1.0f;

        if (onLadder)
        {
            LadderUpdate();
            highestPoint = myTransform.position.y;
            run = false;
            fallDistance = 0.0f;
            grounded = false;
            walkRunAnim.CrossFade(idleAnimation);
            cameraAnimations.CrossFade(idleAnimation);
            return;
        }
			
        if (grounded)
        {
            gravity = baseGravity;

            if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, rayDistance))
            {
                float hitangle = Vector3.Angle(hit.normal, Vector3.up);
                if (hitangle > slideLimit)
                {
                    sliding = true;
                }
                else
                {
                    sliding = false;
                }
            }

            if (canRun && state == 0)
            {
                if (Input.GetKey(RunKey) && Input.GetKey(ForwardKey) && !Input.GetKey(ZoomKey))
                {
                    run = true;
                }
                else
                {
                    run = false;
                }
            }

            if (falling)
            {
                if (state == 2)
                    fallingDamageThreshold = proneFDTreshold;
                else
                    fallingDamageThreshold = normalFDTreshold;

                falling = false;
                fallDistance = highestPoint - currentPosition.y;
                if (fallDistance > fallingDamageThreshold)
                {
                    ApplyFallingDamage(fallDistance);
                }

                if (fallDistance < fallingDamageThreshold && fallDistance > 0.1f)
                {
                    if (state < 2) footsteps.JumpLand();
                    StartCoroutine(FallCamera(new Vector3(7, Random.Range(-1.0f, 1.0f), 0), new Vector3(3, Random.Range(-0.5f, 0.5f), 0), 0.15f));
                }
            }

            if (sliding)
            {
                Vector3 hitNormal = hit.normal;
                moveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                Vector3.OrthoNormalize( ref hitNormal, ref moveDirection);
                moveDirection *= slideSpeed;
            }
            else
            {
				if (state == 0) {
					if (run)
						speed = runSpeed;
					else if (Input.GetKey (ZoomKey)) {
						speed = crouchSpeed;
					} else {
						speed = walkSpeed;
					}

				} else if (state == 1) {
					speed = crouchSpeed;
					run = false;
				}

                if (Cursor.lockState == CursorLockMode.Locked)
                    moveDirection = new Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);
                else
                    moveDirection = new Vector3(0, -antiBumpFactor, 0);

                moveDirection = myTransform.TransformDirection(moveDirection);
                moveDirection *= speed;

				if (!Input.GetKey (JumpKey)) {
					jumpTimer++;
				} else if (jumpTimer >= antiBunnyHopFactor) {
					jumpTimer = 0;
					if (state == 0) {
						moveDirection.y = jumpSpeed;
					}

					if (state > 0 && !(state == 2)) {
						CheckDistance ();
						if (distanceToObstacle > 1.6f) {
							state = 0;
						}
					}
				}
            }
        }
        else
        {
            currentPosition = myTransform.position;
            if (currentPosition.y > lastPosition.y)
            {
                highestPoint = myTransform.position.y;
                falling = true;
            }

            if (!falling)
            {
                highestPoint = myTransform.position.y;
                falling = true;
            }

            if (airControl)
            {
                moveDirection.x = inputX * speed;
                moveDirection.z = inputY * speed;
                moveDirection = myTransform.TransformDirection(moveDirection);
            }
        }

        if (grounded)
        {
            if (velMagnitude > crouchSpeed && !run)
            {
                walkRunAnim["Walk"].speed = velMagnitude / adjustAnimSpeed;
                walkRunAnim.CrossFade("Walk");
            }
            else
            {
                walkRunAnim.CrossFade(idleAnimation);
            }

            if (run && velMagnitude > walkSpeed)
            {
                walkRunAnim.CrossFade("Run");
                cameraAnimations.CrossFade(runAnimation);
            }
            else
            {
                cameraAnimations.CrossFade(idleAnimation);
            }

        }
        else
        {
            walkRunAnim.CrossFade(idleAnimation);
            cameraAnimations.CrossFade(idleAnimation);
            run = false;
        }

		if (Input.GetKeyDown(CrouchKey))
        {
            CheckDistance();

            if (state == 0)
            {
                state = 1;
            }
            else if (state == 1)
            {
                if (distanceToObstacle > 1.6f)
                {
                    state = 0;
                }
            }
        }

		if (state == 0) { //Stand Position
			controller.height = 2.0f;
			controller.center = new Vector3 (0, 0, 0);

			if (cameraGO.localPosition.y > normalHeight) {
				cameraGO.localPosition = new Vector3 (cameraGO.localPosition.x, normalHeight, cameraGO.localPosition.z);
			} else if (cameraGO.localPosition.y < normalHeight) {
				cameraGO.localPosition = new Vector3 (cameraGO.localPosition.x, cameraGO.localPosition.y + Time.deltaTime * transitionSpeed, cameraGO.localPosition.z);
			}

		} else if (state == 1) { //Crouch Position
			controller.height = 1.4f;
			controller.center = new Vector3 (0, -0.3f, 0);

			if (cameraGO.localPosition.y != crouchHeight) {
				if (cameraGO.localPosition.y > crouchHeight) {
					cameraGO.localPosition = new Vector3 (cameraGO.localPosition.x, cameraGO.localPosition.y - Time.deltaTime * transitionSpeed, cameraGO.localPosition.z);
				}
				if (cameraGO.localPosition.y < crouchHeight) {
					cameraGO.localPosition = new Vector3 (cameraGO.localPosition.x, cameraGO.localPosition.y + Time.deltaTime * transitionSpeed, cameraGO.localPosition.z);
				}

			}
		}

		moveDirection.y -= gravity * Time.deltaTime;
        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
    }

    void CheckDistance()
    {
        Vector3 pos = myTransform.position + controller.center - new Vector3(0, controller.height / 2, 0);
        RaycastHit hit;
        if (Physics.SphereCast(pos, controller.radius, myTransform.up, out hit, 10))
        {
            distanceToObstacle = hit.distance;
            Debug.DrawLine(pos, hit.point, Color.red, 2.0f);
        }
        else
        {
            distanceToObstacle = 3;
        }
    }
	
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (controller.collisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }

		body.AddForceAtPosition (controller.velocity * 0.1f, hit.point, ForceMode.Impulse);

		/*
		if (!(body.tag == "DynamicObject")) {
			body.AddForceAtPosition (controller.velocity * 0.1f, hit.point, ForceMode.Impulse);
		} else if (body.GetComponent<DynamicObjectScript> () && body.GetComponent<DynamicObjectScript> ().dynamicType == DynamicObjectScript.dynamic.HingeDoor) {
			//body.GetComponent<DynamicObjectScript> ().PushDoor (controller.velocity);
		}
		*/
    }

    void LateUpdate()
    {
        lastPosition = currentPosition;
    }

    IEnumerator FallCamera(Vector3 d, Vector3 dw, float ta)
    {
        Quaternion s = fallEffect.localRotation;
        Quaternion sw = fallEffectWeap.localRotation;
        Quaternion e = fallEffect.localRotation * Quaternion.Euler(d);
       // Quaternion ew = fallEffectWeap.localRotation * Quaternion.Euler(dw);
        float r = 1.0f / ta;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * r;
            fallEffect.localRotation = Quaternion.Slerp(s, e, t);
            fallEffectWeap.localRotation = Quaternion.Slerp(sw, e, t);
            yield return null;
        }
    }

    void ApplyFallingDamage(float fallDistance)
    {
		hm.ApplyDamage(fallDistance * fallDamageMultiplier);
        if (state < 2) StartCoroutine(footsteps.JumpLand());
        StartCoroutine( FallCamera(new Vector3(12, Random.Range(-2.0f, 2.0f), 0), new Vector3(4, Random.Range(-1.0f, 1.0f), 0), 0.1f));
    }

	//On Trigger Stay
	void OnTriggerStay(Collider collider){
		if (collider.tag == "Ladder" && useLadder) {
			onLadder = true;
		}
	}

	//On Trigger Exit
	void OnTriggerExit(Collider collider){
		if (collider.tag == "Ladder") {
			onLadder = false;
			useLadder = true;
		}
	}
		
	//Ladder Movement
	private void LadderUpdate () {
		float CamRot = Camera.main.gameObject.transform.forward.y;
		if(onLadder)
		{
			Vector3 verticalMove;
			verticalMove = climbDirection.normalized;
			verticalMove *= inputY;
			verticalMove *= (CamRot > climbDownThreshold) ? 1 : -1;
			lateralMove = new Vector3(inputX, 0, inputY);
			lateralMove = transform.TransformDirection(lateralMove);
			ladderMovement = verticalMove + lateralMove;
			controller.Move(ladderMovement * climbSpeed * Time.deltaTime);
		 
			if(inputY == 1 && !(aSource.isPlaying) && Time.time >= playTime)
			{
				PlayLadderSound();
			}
		 
			if(Input.GetKey(JumpKey)) {
				useLadder = false;
				onLadder = false;
			}
		}
	}
		
	//Ladder Footsteps
	void PlayLadderSound()
	{
		footsteps.PlayLadderSound();
		playTime = Time.time + climbRate;
	}
}