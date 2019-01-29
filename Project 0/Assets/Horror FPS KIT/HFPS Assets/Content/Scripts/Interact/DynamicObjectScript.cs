using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectScript : MonoBehaviour {

	public enum dynamic
	{
		HingeDoor,
		Drawer,
		Lever,
		MovableInteract
	}

	public enum type
	{
		Normal,
		Locked,
		Jammed,
	}

	public dynamic dynamicType = dynamic.HingeDoor;
	[Tooltip("Use only on Drawer or Door")]
	public type useType = type.Normal;

	public List<GameObject> IgnoreColliders = new List<GameObject>();
    public GameObject InteractObject;

    private Transform obj;
	private Transform old_parent;

	[Space(2)]
	[Tooltip("If empty, text to show is set to default.")]
	public string CustomLockedText;

	[Header("Sounds")]
	public AudioClip UnlockSound;
	public AudioClip LockedTry;
	public float soundVolume;

	[Header("Dynamic Door")]
	public AudioClip Open;
	public AudioClip Close;

	private float angle;

	[HideInInspector]
	public bool hasKey = false;

	[HideInInspector]
	public Inventory inv;

	[HideInInspector]
	public int keyID = -1;

	private float defaultAngle;
	private bool isPressed;
	private bool isUnlocked;
	private bool isOpen = false;
	private bool isHeld;

	private HingeJoint joint;

	[Header("Dynamic Drawer + Interact")]
	[Tooltip("If true default move vector will be X, if false default vector is Z")]
	public bool moveX;
	public float InteractPos;
	public float MinMove;
	public float MaxMove;
	public bool reverseMove;

	[Header("Dynamic Lever")]
	public AudioClip LeverUpSound;
	public bool upLock;
	public float angleStop;

	[HideInInspector]
	public bool isUp;

	[HideInInspector]
	public bool isHolding;

	private bool hold;

	[Header("Debug")]
	public bool DebugLeverAngle;
	public bool DebugDoorAngle;

	private bool isPlayed;

	void Start () {
		if (dynamicType == dynamic.HingeDoor) {
			joint = GetComponent<HingeJoint> ();
				defaultAngle = transform.eulerAngles.y;
			if (useType == type.Locked || useType == type.Jammed) {
				joint.useLimits = false;
				GetComponent<Rigidbody> ().freezeRotation = true;
				isUnlocked = false;
			} else if (useType == type.Normal) {
				joint.useLimits = true;
				GetComponent<Rigidbody> ().freezeRotation = false;
				isUnlocked = true;
			}
		} else if (dynamicType == dynamic.Drawer) {
			IgnoreColliders.Add (Camera.main.transform.root.gameObject);
			if (useType == type.Locked || useType == type.Jammed) {
				isUnlocked = false;
			} else if (useType == type.Normal) {
				isUnlocked = true;
			}
		}

		if (IgnoreColliders.Count > 0)
		{
			for (int i = 0; i < IgnoreColliders.Count; i++) {
				Physics.IgnoreCollision(GetComponent<Collider>(), IgnoreColliders[i].GetComponent<Collider>());
			}
		}
	}

	void Update () {
		if (dynamicType == dynamic.HingeDoor) {
			angle = this.gameObject.transform.eulerAngles.y;

			if (DebugDoorAngle) {
				Debug.Log ("Angle: " + angle.ToString () + " , Door Close: " + (defaultAngle - 0.5f).ToString ());
			}

			if (hasKey && isUnlocked) {
				useType = type.Normal;
				joint.useLimits = true;
				GetComponent<Rigidbody> ().freezeRotation = false;
			}
				
			if (angle > 1f && !(angle < 1f)) {
				if (angle <= (defaultAngle - 2f) && !isOpen) {
					if (Open) {
						AudioSource.PlayClipAtPoint (Open, transform.position);
					}
					isOpen = true;
				}

				if (angle >= (defaultAngle - 0.5f) && isOpen) {
					if (Close) {
						AudioSource.PlayClipAtPoint (Close, transform.position);
					}
					isOpen = false;
				}
			}
		} else if (dynamicType == dynamic.Lever) {
			float currentAngle = this.gameObject.transform.eulerAngles.x;

			if (DebugLeverAngle) {
				Debug.Log (Mathf.Round (currentAngle));
			}

			if (upLock) {
				if (hold) {
					GetComponent<Rigidbody> ().isKinematic = true;
					GetComponent<Rigidbody> ().useGravity = false;
				} else {
					GetComponent<Rigidbody> ().isKinematic = false;
					GetComponent<Rigidbody> ().useGravity = true;
				}
			} else {
				if (isHolding) {
					GetComponent<Rigidbody> ().isKinematic = false;
					GetComponent<Rigidbody> ().useGravity = false;
				}

				if (!isHolding && hold) {
					GetComponent<Rigidbody> ().isKinematic = true;
					GetComponent<Rigidbody> ().useGravity = false;
				} else if(!hold) {
					GetComponent<Rigidbody> ().isKinematic = false;
					GetComponent<Rigidbody> ().useGravity = true;
				}
			}
				
			if (!DebugLeverAngle) {
				if (currentAngle >= angleStop && currentAngle > (angleStop - 5)) {
					InteractObject.SendMessage ("SwitcherUp", SendMessageOptions.DontRequireReceiver);
					if (!isPlayed && LeverUpSound) {
						AudioSource.PlayClipAtPoint (LeverUpSound, transform.position, 1f);
						isPlayed = true;
					}
					hold = true;
				} else {
					InteractObject.SendMessage ("SwitcherDown", SendMessageOptions.DontRequireReceiver);
					isPlayed = false;
					hold = false;
				}
			}
		}
	}

	public void PushDoor(float velocity){
		JointMotor motor = joint.motor;
		joint.useMotor = true;
		motor.targetVelocity = velocity;
		joint.motor = motor;
	}

	public void UseObject()
	{
		if (dynamicType == dynamic.HingeDoor || dynamicType == dynamic.Drawer) {
			if (hasKey && !isUnlocked) {
				if (UnlockSound) { AudioSource.PlayClipAtPoint(UnlockSound, transform.position, soundVolume); }
				if (inv && keyID >= 0) {
					inv.RemoveItem (keyID);
				}
				useType = type.Normal;
				isUnlocked = true;
            }

			if (LockedTry && !hasKey && !isUnlocked) {
				AudioSource.PlayClipAtPoint (LockedTry, transform.position, soundVolume);
			}
		}
	}

	public void UnlockDoor()
	{
		hasKey = true;
		if (hasKey && !isUnlocked) {
			if (UnlockSound) { AudioSource.PlayClipAtPoint(UnlockSound, transform.position, soundVolume); }
			useType = type.Normal;
			isUnlocked = true;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<Rigidbody>() && dynamicType == dynamic.Drawer)
		{
			obj = collision.transform;
			old_parent = obj.transform.parent;
			obj.transform.SetParent(this.transform);
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.GetComponent<Rigidbody>() && dynamicType == dynamic.Drawer)
		{
			obj.transform.SetParent(old_parent);
			obj = null;
		}
	}
}
