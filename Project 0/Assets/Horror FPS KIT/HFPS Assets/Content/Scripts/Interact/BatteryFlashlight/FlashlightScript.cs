/* FlashlightScript.cs by ThunderWire Games / script for Flashlight */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

	[System.Serializable]
	public class BatterySpritesClass{
		public Sprite Battery_0_red;
		public Sprite Battery_5;
		public Sprite Battery_10;
		public Sprite Battery_15;
		public Sprite Battery_20;
		public Sprite Battery_25;
		public Sprite Battery_30;
		public Sprite Battery_35;
		public Sprite Battery_40;
		public Sprite Battery_45;
		public Sprite Battery_50;
		public Sprite Battery_55;
		public Sprite Battery_60;
		public Sprite Battery_65;
		public Sprite Battery_70;
		public Sprite Battery_75;
		public Sprite Battery_80;
		public Sprite Battery_85;
		public Sprite Battery_90;
		public Sprite Battery_95;
		public Sprite Battery_100;
	}

public class FlashlightScript : MonoBehaviour {

	public ScriptManager scriptManager;
	private InputManager inputManager;
	private ItemSwitcher switcher;
	private UIManager ui;

	[Header("Setup")]
	public BatterySpritesClass BatterySprites = new BatterySpritesClass();
	public int BatteryInventoryID;

	public AudioClip ReloadBatteriesSound;
	public KeyCode BatteryReloadKey = KeyCode.B;

	private GameObject BatterySpriteUI;
	private GameObject BatteryCanvas;

	[Header("Flashlight Animations")]
	public bool useAnimation;
	public GameObject FlashlightGO;

	public string DrawAnimation;
	public string HideAnimation;

	public float DrawSpeed;
	public float HideSpeed;

	[Header("Flashlight Settings")]
	public Light Flashlight;
	public AudioClip ClickSound;
	public float batteryLifeInSec = 300f;
	public float batteryPercentage = 100;

	[Header("Batteries Quantity")]
	public int MinBatteries = 0;
	public int MaxBatteries = 5;

	[Header("Messages")]
	public bool enablePickupText;
	public string FlashPickupText = "Flashlight";
	public string MaxBatteryText = "You have Max Batteries";
	public string BatteryPickupText = "Battery +1";
	
	[HideInInspector]
	public KeyCode FlashlightKey;

	[HideInInspector]
	public bool PickedFlashlight = false;

	private int Batteries;

	[HideInInspector]
	public bool canPickup;

	private bool hide;
	private bool eventOn;
	private bool on;
	private float timer;
	private bool playAnim;
	private bool isPlayed = true;

	void Start () {
		inputManager = scriptManager.inputManager;
		switcher = transform.parent.gameObject.GetComponent<ItemSwitcher> ();
		ui = scriptManager.uiManager;
		BatterySpriteUI = ui.BatterySprites;
		BatteryCanvas = ui.BatteryUI;
		BatteryCanvas.SetActive (false);
	}

	public void AddBattery(int quantity)
	{
		if (canPickup) {
			Batteries += quantity;
			ui.AddPickupMessage (BatteryPickupText);
		} else {
			ui.WarningMessage (MaxBatteryText);
		}
	}

	public void Deselect()
	{
		if (FlashlightGO.activeSelf) {
			FlashlightGO.GetComponent<Animation>().Play(HideAnimation);
			if (ClickSound) {
				AudioSource.PlayClipAtPoint (ClickSound, transform.position, 0.75f);
			}
			on = false;
			hide = true;
			playAnim = false;
		}
	}

	public void Select()
	{
		if (useAnimation) {
			playAnim = true;
			isPlayed = false;
		} else {
			on = !on;
			if (ClickSound) {
				AudioSource.PlayClipAtPoint (ClickSound, transform.position, 0.75f);
			}
		}
		timer = 0;
	}

	public void PickupFlashlight()
	{
		if (enablePickupText) {
			ui.AddPickupMessage (FlashPickupText);
		}
		PickedFlashlight = true;
	}

	public void Event_FlashlightOn()
	{
		on = true;
		eventOn = true;
		if(ClickSound){AudioSource.PlayClipAtPoint(ClickSound, transform.position, 0.75f);}
	}

	void Update() {
		if(inputManager.DictCount() > 0)
		{
			FlashlightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Flashlight"));
		}

		Image BatterySprite = BatterySpriteUI.GetComponent<Image>();
		Light lite = Flashlight;
		timer += Time.deltaTime;

		if (FlashlightGO.activeSelf) {
			if (Input.GetKeyDown (BatteryReloadKey) && Batteries > 0 && Batteries <= 5) {
				if (batteryPercentage < 90.0f) {
					batteryPercentage = 100;
					Batteries --;
					ui.inventoryScript.RemoveItem (BatteryInventoryID);
					if (ReloadBatteriesSound) {
						AudioSource.PlayClipAtPoint (ReloadBatteriesSound, transform.position, 0.75f);
					}
				}
			}
		}
		
		if(PickedFlashlight && switcher.ItemList[1].GetComponent<ActiveState>().activeState() == false){
			BatteryCanvas.SetActive (true);

			if(Input.GetKeyDown(FlashlightKey) && timer >= 0.3f && batteryPercentage > 0 && !FlashlightGO.GetComponent<Animation>().isPlaying) {
				if (!on && !(switcher.currentItem == 0)) {
					switcher.selectItem (0);
				} else {
					if (useAnimation) {
						playAnim = !playAnim;
						isPlayed = false;
						if (eventOn) {
							on = false;
							if(ClickSound){AudioSource.PlayClipAtPoint(ClickSound, transform.position, 0.75f);}
							eventOn = false;
						}
					} else {
						on = !on;
						if (ClickSound) {
							AudioSource.PlayClipAtPoint (ClickSound, transform.position, 0.75f);
						}
					}
					timer = 0;
				}
			}
		}

		if (!playAnim && hide && !(FlashlightGO.GetComponent<Animation> ().isPlaying)) {
			FlashlightGO.SetActive (false);
			hide = false;
		}

		if(playAnim && !isPlayed) {
			FlashlightGO.SetActive (true);
			FlashlightGO.GetComponent<Animation>().Play(DrawAnimation);
			isPlayed = true;
		}else if(!isPlayed){
			FlashlightGO.GetComponent<Animation>().Play(HideAnimation);
			hide = true;
			isPlayed = true;
		}

		if(on) {
			lite.enabled = true;
			batteryPercentage -= Time.deltaTime * (100 / batteryLifeInSec);
		}else{
			lite.enabled = false;
		}

		Batteries = Mathf.Clamp(Batteries, 0, MaxBatteries);

		if (Batteries <= MinBatteries)
		{
			Batteries = MinBatteries;
			canPickup = true;
		}

		//Setting for a max batteries
		else if(Batteries > 5)
		{
			Batteries = MaxBatteries;
			canPickup = false;
		}
	
		batteryPercentage = Mathf.Clamp(batteryPercentage, 0, 100);
	
			if (batteryPercentage > 95.0f)
			{
				BatterySprite.color = Color.white;
				BatterySprite.sprite = BatterySprites.Battery_100;
				lite.intensity = Mathf.Lerp(lite.intensity, 1, Time.deltaTime);
			}
			else if (batteryPercentage <= 95.0f && batteryPercentage > 90.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_95;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.95f, Time.deltaTime);
			}
			else if (batteryPercentage <= 90.0f && batteryPercentage > 85.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_90;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.9f, Time.deltaTime);
			}
			else if (batteryPercentage <= 85.0f && batteryPercentage > 80.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_85;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.85f, Time.deltaTime);
			}
			else if (batteryPercentage <= 80.0f && batteryPercentage > 75.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_80;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.8f, Time.deltaTime);
			}
			else if (batteryPercentage <= 75.0f && batteryPercentage > 70.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_75;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.75f, Time.deltaTime);
			}
			else if (batteryPercentage <= 70.0f && batteryPercentage > 65.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_70;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.7f, Time.deltaTime);
			}
			else if (batteryPercentage <= 65.0f && batteryPercentage > 60.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_65;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.65f, Time.deltaTime);
			}
			else if (batteryPercentage <= 60.0f && batteryPercentage > 55.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_60;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.6f, Time.deltaTime);
			}
			else if (batteryPercentage <= 55.0f && batteryPercentage > 50.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_55;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.55f, Time.deltaTime);
			}
			else if (batteryPercentage <= 50.0f && batteryPercentage > 45.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_50;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.5f, Time.deltaTime);
			}
			else if (batteryPercentage <= 45.0f && batteryPercentage > 40.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_45;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.45f, Time.deltaTime);
			}		
			else if (batteryPercentage <= 40.0f && batteryPercentage > 35.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_40;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.4f, Time.deltaTime);
			}	
			else if (batteryPercentage <= 35.0f && batteryPercentage > 30.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_35;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.35f, Time.deltaTime);
			}	
			else if (batteryPercentage <= 30.0f && batteryPercentage > 25.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_30;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.3f, Time.deltaTime);
			}			
			else if (batteryPercentage <= 25.0f && batteryPercentage > 20.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_25;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.25f, Time.deltaTime);
			}
			else if (batteryPercentage <= 20.0f && batteryPercentage > 15.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_20;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.2f, Time.deltaTime);
			}
			else if (batteryPercentage <= 15.0f && batteryPercentage > 10.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_15;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.15f, Time.deltaTime);
			}
			else if (batteryPercentage <= 10.0f && batteryPercentage > 5.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_10;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.1f, Time.deltaTime);
			}
			else if (batteryPercentage <= 5.0f && batteryPercentage > 1.0f)
			{
				BatterySprite.sprite = BatterySprites.Battery_5;
				lite.intensity = Mathf.Lerp(lite.intensity, 0.05f, Time.deltaTime);
			}
			else if (batteryPercentage <= 1.0f)
			{
				BatterySprite.color = Color.red;
				BatterySprite.sprite = BatterySprites.Battery_0_red;
				lite.intensity = Mathf.Lerp(lite.intensity, 0, Time.deltaTime * 2);
			}
	}
}