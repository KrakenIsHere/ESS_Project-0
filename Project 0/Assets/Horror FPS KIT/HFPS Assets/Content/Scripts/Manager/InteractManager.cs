using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class InteractManager : MonoBehaviour {

	private InputManager inputManager;
    private UIManager uiManager;
	private ItemSwitcher itemSelector;
	private Inventory inventory;

	[Header("Raycast")]
	public float RayLength = 3;
	public LayerMask cullLayers;
	public string InteractLayer;
	
	[Header("Crosshair Textures")]
	public Sprite defaultCrosshair;
	public Sprite interactCrosshair;
	private Sprite default_interactCrosshair;
	
	[Header("Crosshair")]
	private Image CrosshairUI;
	public int crosshairSize = 5;
	public int interactSize = 10;

	private int default_interactSize;
    private int default_crosshairSize;

    [Header("Other")]
    public BlurOptimized blur;

    [HideInInspector]
	public bool isHeld = false;

    [HideInInspector]
    public bool inUse;

    [HideInInspector]
	public Ray playerAim;

	[HideInInspector]
	public GameObject RaycastObject;
	
	private KeyCode UseKey;
	private string GrabKey;
	
	private Camera playerCam;
	private DynamicObjectScript m_door;

	private GameObject LastRaycastObject;

	private string RaycastTag;

	private bool correctLayer;

	private bool isPressed;
    private bool useTexture;
	private bool isSet;

    void Start () {
		inputManager = GetComponent<ScriptManager>().inputManager;
		uiManager = GetComponent<ScriptManager>().uiManager;
		itemSelector = GetComponent<ScriptManager>().itemSwitcher;
		CrosshairUI = uiManager.Crosshair;
        default_interactCrosshair = interactCrosshair;
        default_crosshairSize = crosshairSize;
        default_interactSize = interactSize;
        playerCam = Camera.main;
		RaycastObject = null;
		m_door = null;
	}

	void SetKeys()
	{
		UseKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Use"));
		GrabKey = inputManager.GetInput ("Grab");
		isSet = true;
	}
	
	void Update () {
		inventory = GetComponent<ScriptManager>().inventory;

		if(inputManager.DictCount() > 0 && !isSet)
		{
			SetKeys();
		}

		if (inputManager.GetRefreshStatus () && isSet) {
			isSet = false;
		}

		if(Input.GetKey(UseKey) && RaycastObject && !isPressed && !isHeld && !inUse){
			Interact();
			isPressed = true;
		}

		if(Input.GetKeyUp(UseKey) && isPressed){
			isPressed = false;
		}
			
        Ray playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;

		if (Physics.Raycast (playerAim, out hit, RayLength, cullLayers)) {
			RaycastTag = hit.collider.gameObject.tag;
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer(InteractLayer)) {
                RaycastObject = hit.collider.gameObject;
                correctLayer = true;

				if (RaycastObject.GetComponent<DynamicObjectScript> ()) {
					m_door = RaycastObject.GetComponent<DynamicObjectScript> ();
				} else {
					m_door = null;
				}
			
				if (RaycastObject.GetComponent<CrosshairReticle> ()) {
					CrosshairReticle ChangeReticle = RaycastObject.GetComponent<CrosshairReticle> ();
					interactCrosshair = ChangeReticle.interactSprite;
					interactSize = ChangeReticle.size;
				}
					
				useTexture = true;

				if (LastRaycastObject) {
					if (!(LastRaycastObject == RaycastObject)) {
						ResetCrosshair ();
					}
				}
				LastRaycastObject = RaycastObject;
			
				if (!inUse) {
					if (m_door) {
						if (m_door.useType == DynamicObjectScript.type.Locked) {
							if (m_door.hasKey) {
								uiManager.ShowInteractSprite (1, "Unlock", UseKey.ToString ());
							} else {
								uiManager.ShowInteractSprite (1, "Use", UseKey.ToString ());
							}
						} else {
							uiManager.ShowInteractSprite (1, "Use", UseKey.ToString ());
						}
					} else {
						if (!(RaycastTag == "OnlyGrab")) {
							uiManager.ShowInteractSprite (1, "Use", UseKey.ToString ());
						}
					}
					if (RaycastTag == "OnlyGrab") {
						uiManager.ShowInteractSprite (1, "Grab", GrabKey);
					} else if (RaycastTag == "Grab") {
						uiManager.ShowInteractSprite (1, "Use", UseKey.ToString ());
						uiManager.ShowInteractSprite (2, "Grab", GrabKey);
					} else if (RaycastTag == "Paper") {
						uiManager.ShowInteractSprite (1, "Examine", UseKey.ToString ());
					}
					if (RaycastObject.GetComponent<ExamineItem> ()) {
						if (RaycastObject.GetComponent<ExamineItem> ().isUsable) {
							uiManager.ShowInteractSprite (1, "Use", UseKey.ToString ());
							uiManager.ShowInteractSprite (2, "Examine", GrabKey);
						} else {
							uiManager.ShowInteractSprite (1, "Examine", UseKey.ToString ());
						}
					}

				}
			} else if(RaycastObject) {
				correctLayer = false;
			}
		} else if(RaycastObject) {
			correctLayer = false;
		}

		if(!correctLayer){
			ResetCrosshair ();
			useTexture = false;
			RaycastTag = null;
			RaycastObject = null;
			m_door = null;
		}
		
		if(!RaycastObject)
		{
			uiManager.HideSprites("Interact");
            useTexture = false;
			m_door = null;
		}
	}

    void FixedUpdate()
    {
        if(useTexture)
        {
			CrosshairUI.rectTransform.sizeDelta = new Vector2(interactSize, interactSize);
            CrosshairUI.sprite = interactCrosshair;
        }
        else
        {
			CrosshairUI.rectTransform.sizeDelta = new Vector2(crosshairSize, crosshairSize);
            CrosshairUI.sprite = defaultCrosshair;
        }
    }

	private void ResetCrosshair(){
		crosshairSize = default_crosshairSize;
		interactSize = default_interactSize;
		interactCrosshair = default_interactCrosshair;
	}

	public void CrosshairVisible(bool state)
	{
		switch (state) 
		{
		case true:
			CrosshairUI.enabled = true;
			break;
		case false:
			CrosshairUI.enabled = false;
			break;
		}
	}

	public bool GetInteractBool()
	{
		if (RaycastObject) {
			return true;
		} else {
			return false;
		}
	}

	void Interact(){
		if (RaycastObject.GetComponent<ItemID> ()) {
			ItemID iID = RaycastObject.GetComponent<ItemID> ();
			if (iID.ItemType == ItemID.Type.BackpackExpand) {
				inventory.ExpandSlots (iID.BackpackExpand);
				Pickup ();
			}

			if (inventory.CheckInventorySpace ()) {
				if (iID.ItemType == ItemID.Type.NoInventoryItem) {
					itemSelector.selectItem (iID.WeaponID);
				} else if (iID.ItemType == ItemID.Type.InventoryItem) {
					inventory.AddItemToSlot (iID.InventoryID, iID.Amount);
				} else if (iID.ItemType == ItemID.Type.WeaponItem) {
					if (iID.weaponType == ItemID.WeaponType.Weapon) {
						inventory.AddItemToSlot (iID.InventoryID, iID.Amount);
						inventory.SetWeaponID (iID.InventoryID, iID.WeaponID);
						itemSelector.selectItem (iID.WeaponID);
					} else if (iID.weaponType == ItemID.WeaponType.Ammo) {
						inventory.AddItemToSlot (iID.InventoryID, iID.Amount);
					}
				}
				Pickup ();
			} else {
				uiManager.ShowHint ("No Inventory Space!");
			}
		} else {
			Pickup ();
		}
	}

	void Pickup()
	{
		uiManager.HideSprites ("Interact");
		RaycastObject.SendMessage ("UseObject", SendMessageOptions.DontRequireReceiver);
	}
}
