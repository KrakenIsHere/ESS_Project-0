using Vexe.Runtime.Types;
using UnityEngine;

public class MoveSquareMoveables : BaseBehaviour
{
    GameSettings.CurrentlyGrabbed currentlyGrabbed;
    GameObject go;
    GameSettings gameSettings;

    [Comment("From where the ray will come. Recommended: Main Camera", helpButton: true)]
    public Transform center = null;

    [Comment("From how far you can reach objects", helpButton: true)]
    public float reach = 25f;

    [Comment("The rate in which objects move towards the grab position. Higher = faster", helpButton: true)]
    public float rate = 5f;

    [Comment("The rate in which the charge for throw / push goes up", helpButton: true)]
    public float chargeRate = 12.5f;

    [Comment("Maximum force of a throw/push", helpButton: true)]
    public float maxThrowForce = 5f;

    [Comment("The time you must wait to pick up an object after throwing another (Push does not affect this)", helpButton: true)]
    public float grabCooldown = 1f;

    [Comment("The rate in which the object repositions itself", helpButton: true)]
    public float repositionRate = 50f;

    [Comment("The rate in which you can rotate an object", helpButton: true)]
    public float rotateRate = 5f;

    [Comment("Minimum reposition distance", helpButton: true)]
    public float minimumDistance = 2f;

    [Comment("Maximum reposition distance", helpButton: true)]
    public float maximumDistance = 25f;


    [Comment("Default Grab Key", helpButton: true)]
    public KeyCode GrabKey = KeyCode.E;

    [Comment("Default Throw/Push Key", helpButton: true)]
    public KeyCode ThrowPushKey = KeyCode.LeftShift;

    [Comment("Defualt Rotate Key", helpButton: true)]
    public KeyCode RotateKey = KeyCode.LeftControl;

    [Comment("Makes you able to grab objects", helpButton: true)]
    public bool canGrab = true;

    [Comment("Makes you able to throw objects that are grabbed", helpButton: true)]
    public bool canThrow = true;

    [Comment("Makes you able to push objects that are not grabbed", helpButton: true)]
    public bool canPush = true;

    [Comment("Makes you able to reposition objects that are grabbed", helpButton: true)]
    public bool canReposition = true;

    [Comment("Makes you able to rotate the objects that are grabbed", helpButton: true)]
    public bool canRotate = true;

    [Comment("A layermask is a bitmask of one or more layer which allow the casted ray to detect the chosen layer", helpButton: true)]
    public LayerMask layerMask;

    [Tags]
    [Comment("The tag used by moveable objects", helpButton: true)]
    public string liftTag = "Square Moveables";


    private string weightClass = "Square";

    private float wantedPosition = 3f;
    private float grabTimer = 0f;
    private float throwForce = 0f;
    private bool wantGrab = false;
    private bool wantThrow = false;
    private bool wantPush = false;
    private bool wantReposition = false;
    public bool wantRotate = false;
    private bool canGrabAgain = true;
    private GameObject grabbed = null;

    public bool isRotating { get; private set; }

    private void Awake()
    {
        go = GameObject.Find("Settings");
        gameSettings = go.GetComponent<GameSettings>();
    }

    void Update()
    {
        UpdateCurrentlyGrabbed();
    }

    private void UpdateInput()
    {
        // Check if the player is trying to Grab / Release an Object.
        if (Input.GetKey(GrabKey) && canGrab)
        {
            wantGrab = true;
        }
        else
        {
            wantGrab = false;
        }

        // Check if the player is charging a Throw/Push, also see if the player wants to Throw/Push.
        if (Input.GetKey(ThrowPushKey))
        {
            if (throwForce < maxThrowForce)
                throwForce += (chargeRate * Time.deltaTime);
            if (throwForce > maxThrowForce)
                throwForce = maxThrowForce;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (canPush && grabbed == null)
                wantPush = true;
            if (canThrow && grabbed != null)
                wantThrow = true;
        }

        // Check if the player wants to re-position the object.
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0 && canReposition)
        {
            wantReposition = true;
        }
        else
        {
            wantReposition = false;
        }

        // Check if the player wants to rotate the object.
        if (Input.GetKey(RotateKey) && grabbed != null && canRotate)
        {
            wantRotate = true;
        }
        else
        {
            wantRotate = false;
        }
    }

    private void UpdateLogic()
    {
        // Wants to grab/keep grabbing an object.
        if (wantGrab && canGrabAgain)
        {
            if (grabbed == null)
            {
                Ray ray = new Ray(center.position, center.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, reach, layerMask))
                {
                    if (hit.collider.tag == liftTag)
                    {
                        grabbed = hit.collider.gameObject;
                    }
                }
            }
            else
            {
                if (grabbed.GetComponent<Rigidbody>())
                {
                    Rigidbody rb = grabbed.GetComponent<Rigidbody>();
                    rb.velocity = ((center.position + (center.forward * wantedPosition)) - grabbed.transform.position) * rate;
                }
                else
                {
                    wantedPosition = 3f;
                    grabbed = null;
                }
            }
        }
        else
        {
            if (grabbed != null)
            {
                grabbed = null;
            }

            /* 
            If you activate this, the wantedPosition will reset everytime
            You let go of an object. You could also make another if statement to see if grabbed == null
            and have the code in there instead.
            wantedPosition = reach;
            */
        }

        // wants to re-position the grabbed object.
        if (wantReposition)
        {
            if (grabbed != null)
            {
                wantedPosition += (Input.GetAxis("Mouse ScrollWheel") * repositionRate) * Time.deltaTime;
                wantedPosition = Mathf.Clamp(wantedPosition, minimumDistance, maximumDistance);
            }
            else
            {
                wantedPosition = 3f;
                wantReposition = false;
            }
        }

        // wants to rotate the grabbed object.
        if (wantRotate)
        {
            if (grabbed != null)
            {

                float xa = Input.GetAxis("Mouse X") * 10;
                float ya = Input.GetAxis("Mouse Y") * 10;
                grabbed.transform.Rotate(new Vector3(ya, -xa, 0), Space.World);
                isRotating = true;
            }
            else
            {
                isRotating = false;
            }
        }
        else
        {
            isRotating = false;
        }

        // want to throw the grabbed object.
        if (wantThrow)
        {
            if (grabbed != null)
            {
                Rigidbody rb = grabbed.GetComponent<Rigidbody>();
                rb.velocity += center.rotation * new Vector3(0, 0, throwForce);
                throwForce = 0f;
                grabbed = null;
                canGrabAgain = false;
            }

            wantThrow = false;
            throwForce = 0f;
        }

        // want to push the object.
        if (wantPush)
        {
            if (grabbed == null)
            {
                Ray ray = new Ray(center.position, center.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, reach, layerMask))
                {
                    if (hit.collider.tag == liftTag)
                    {
                        Rigidbody rb = hit.collider.transform.GetComponent<Rigidbody>();
                        rb.velocity = center.rotation * new Vector3(0, 0, throwForce);
                        throwForce = 0f;
                    }
                }
            }

            wantPush = false;
            throwForce = 0f;
        }

        // Has thrown a grabbed object and need to cooldown.
        if (!canGrabAgain)
        {
            if (grabTimer < grabCooldown)
            {
                grabTimer += Time.deltaTime;
            }
            else
            {
                canGrabAgain = true;
            }
        }
        else
        {
            grabTimer = 0f;
        }
    }

    public void UpdateCurrentlyGrabbed()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Ray ray = new Ray(center.position, center.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, reach, layerMask))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hit.collider.tag == liftTag)
                    {
                        UpdateGameSettings();
                        UpdateInput();
                        UpdateLogic();
                    }
                    else
                    {
                        GetGameSettings();
                    }
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    if (hit.collider.tag == liftTag && gameSettings.currentWeightClass == weightClass)
                    {
                        UpdateGameSettings();
                        UpdateInput();
                        UpdateLogic();
                    }
                    else
                    {
                        GetGameSettings();
                    }
                }
                else if (Input.GetKeyUp(KeyCode.E))
                {
                    gameSettings.currentWeightClass = "";
                    grabbed = null;
                    currentlyGrabbed = GameSettings.CurrentlyGrabbed.NoneGrabbed;
                    gameSettings.currentlyGrabbed = currentlyGrabbed;
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            gameSettings.currentWeightClass = "";
            grabbed = null;
            currentlyGrabbed = GameSettings.CurrentlyGrabbed.NoneGrabbed;
            gameSettings.currentlyGrabbed = currentlyGrabbed;
        }
    }

    void UpdateGameSettings()
    {
        gameSettings.currentWeightClass = weightClass;
        currentlyGrabbed = GameSettings.CurrentlyGrabbed.SquareMoveablesGrabbed;
        gameSettings.currentlyGrabbed = currentlyGrabbed;
    }

    void GetGameSettings()
    {
        currentlyGrabbed = gameSettings.currentlyGrabbed;
    }
}
