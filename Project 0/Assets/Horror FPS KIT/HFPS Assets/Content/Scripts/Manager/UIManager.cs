/* UIManager.cs - by ThunderWire Games * Script for all UI Action */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	[Header("Main")]
	public GameObject Player;
	public InputManager inputManager;
	public Inventory inventoryScript;

	[HideInInspector]
	public ScriptManager scriptManager;

	[HideInInspector]
	public HealthManager hm;

	[Header("Cursor")]
	public bool m_ShowCursor = false;

	[Header("Game Panels")]
	public GameObject PauseGamePanel;
	public GameObject MainGamePanel;
	public GameObject TabButtonPanel;

	[Header("Pause UI")]
	public KeyCode ShowPauseMenuKey = KeyCode.Escape;
	private bool isPaused = false;

    [Header("Paper UI")]
    public GameObject PaperTextUI;
	public Text PaperReadText;

	[Header("Flashlight & Battery")]
	public GameObject BatterySprites;
	public GameObject BatteryUI;

    [Header("Notification UI")]
	public GameObject NotificationPanel;
	public GameObject NotificationPrefab;
	public Sprite WarningSprite;

	private List<GameObject> Notifications = new List<GameObject>();
	
	[Header("Hints UI")]
	public Text HintText;

	[Header("Crosshair")]
	public Image Crosshair;

	[Header("Health")]
	public Text HealthText;

	[Header("Right Buttons")]
	public bool useSprites;
	public GameObject InteractSprite;
	public GameObject InteractSprite1;

	[Header("Down Examine Buttons")]
	public GameObject DownExamineUI;
	public GameObject ExamineButton1;
	public GameObject ExamineButton2;
	public GameObject ExamineButton3;

	[Header("Down Grab Buttons")]
	public GameObject DownGrabUI;
	public GameObject GrabButton1;
	public GameObject GrabButton2;
	public GameObject GrabButton3;
	public GameObject GrabButton4;

	public Sprite DefaultSprite;
	
	[HideInInspector]
	public bool isHeld;
	
	[HideInInspector]
	public bool canGrab;
	
	private float fadeHint;
	private bool startFadeHint = false;

	private string GrabKey;
	private string ThrowKey;
	private string RotateKey;
	private KeyCode InventoryKey;

	private bool isPressed;
	private bool isSet;

	void SetKeys()
	{
		GrabKey = inputManager.GetInput("Grab");
		ThrowKey = inputManager.GetInput("Throw");
		RotateKey = inputManager.GetInput("Fire");
		InventoryKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Inventory"));
		isSet = true;
	}

	void Start()
	{
		hm = Camera.main.transform.root.gameObject.GetComponent<HealthManager>();
		scriptManager = Player.transform.GetChild (0).transform.GetChild (0).GetComponent<ScriptManager>();
		TabButtonPanel.SetActive (false);
		HideSprites ("Interact");
		HideSprites ("Grab");
		HideSprites ("Examine");

		if (m_ShowCursor) {
			Cursor.visible = (true);
			Cursor.lockState = CursorLockMode.None;					
		} else {
			Cursor.visible = (false);
			Cursor.lockState = CursorLockMode.Locked;						
		}
    }

	void Update()
	{
		HintText.gameObject.GetComponent<CanvasRenderer>().SetAlpha(fadeHint);

		if(inputManager.DictCount() > 0 && !isSet)
		{
			SetKeys();
		}

		if (inputManager.GetRefreshStatus () && isSet) {
			isSet = false;
		}

		if(Input.GetKeyDown(ShowPauseMenuKey) && !isPressed){
			isPressed = true;
			PauseGamePanel.SetActive (!PauseGamePanel.activeSelf);
			MainGamePanel.SetActive (!MainGamePanel.activeSelf);
			isPaused = !isPaused;
		}else if(isPressed){
			isPressed = false;
		}

		if (PauseGamePanel.activeSelf && isPaused && isPressed) {
			LockStates (true, true, true, true, true);
		} else if(isPressed) {
			LockStates (false, true, true, true, true);
		}

		if(Input.GetKeyDown(InventoryKey) && !isPressed && !isPaused){
			isPressed = true;
			TabButtonPanel.SetActive (!TabButtonPanel.activeSelf);
		}else if(isPressed){
			isPressed = false;
		}

		if (TabButtonPanel.activeSelf && isPressed) {
			LockStates (true, true, true, true, false);
			HideSprites ("Interact");
			HideSprites ("Grab");
			HideSprites ("Examine");
		} else if(isPressed) {
			LockStates (false, true, true, true, false);
		}

		if (Notifications.Count > 4) {
			Notifications.RemoveAt (1);
		}
		
		//Fade Out Hint
		if(fadeHint > 0 && startFadeHint)
		{
			fadeHint -= Time.deltaTime;
		}else{
			startFadeHint = false;
		}
	}

	public void Unpause()
	{
		LockStates (false, true, true, true, true);
		PauseGamePanel.SetActive (false);
		MainGamePanel.SetActive (true);
		isPaused = false;
	}

	public void ChangeScene(string SceneName)
	{
		SceneManager.LoadScene(SceneName);
	}

	public void LockStates(bool LockState, bool Interact, bool Controller, bool CursorVisible, bool UseBlur){
		switch (LockState) {
		case true:
			Player.GetComponent<MouseLook> ().enabled = false;
			Player.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<MouseLook> ().enabled = false;
			if (Interact) {
				Player.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<InteractManager> ().inUse = true;
			}
			if (Controller) {
				Player.GetComponent<PlayerController> ().controllable = false;
			}
			if (UseBlur) {
				Player.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<InteractManager> ().blur.enabled = true;
			}
			if (CursorVisible) {
				ShowCursor (true);
			}
			break;
		case false:
			Player.GetComponent<MouseLook> ().enabled = true;
			Player.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<MouseLook> ().enabled = true;
			if (Interact) {
				Player.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<InteractManager> ().inUse = false;
			}
			if (Controller) {
				Player.GetComponent<PlayerController> ().controllable = true;
			}
			if (UseBlur) {
				Player.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<InteractManager> ().blur.enabled = false;
			}
			if (CursorVisible) {
				ShowCursor (false);
			}
			break;
		}
	}

	public void MouseLookState(bool State)
	{
		switch (State) {
		case true:
			Player.GetComponent<MouseLook> ().enabled = true;
			Player.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<MouseLook> ().enabled = true;
			break;
		case false:
			Player.GetComponent<MouseLook> ().enabled = false;
			Player.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<MouseLook> ().enabled = false;
			break;
		}
	}

	public void ShowCursor(bool state)
	{
		switch (state) {
		case true:
			Cursor.visible = (true);
			Cursor.lockState = CursorLockMode.None;
			break;
		case false:
			Cursor.visible = (false);
			Cursor.lockState = CursorLockMode.Locked;	
			break;
		}
	}

	public void AddPickupMessage(string itemName)
	{
		GameObject PickupMessage = Instantiate (NotificationPrefab);
		Notifications.Add (PickupMessage);
		PickupMessage.transform.SetParent (NotificationPanel.transform);
		PickupMessage.GetComponent<ItemPickupNotification> ().SetPickupNotification (itemName);
	}

	public void AddMessage(string message)
	{
		GameObject Message = Instantiate (NotificationPrefab);
		Notifications.Add (Message);
		Message.transform.SetParent (NotificationPanel.transform);
		Message.GetComponent<ItemPickupNotification> ().SetNotification (message);
	}

	public void WarningMessage(string warning)
	{
		GameObject Message = Instantiate (NotificationPrefab);
		Notifications.Add (Message);
		Message.transform.SetParent (NotificationPanel.transform);
		Message.GetComponent<ItemPickupNotification> ().SetNotificationIcon (warning, WarningSprite);
	}
	
	public void ShowHint(string hint)
	{
		fadeHint = 1f;
		startFadeHint = false;
		StopCoroutine(FadeWaitHint());
		HintText.gameObject.SetActive(true);
		HintText.text = hint;
		HintText.color = Color.white;
		StartCoroutine(FadeWaitHint());
	}

	public bool CheckController()
	{
		return Player.GetComponent<PlayerController> ().controllable;
	}

    public void ShowInteractSprite(int num, string name, string Key)
    {
		if (!isHeld) {
			switch (num) {
				case 1:
					InteractSprite.SetActive (true);
					Image bg = InteractSprite.transform.GetChild (0).GetComponent<Image> ();
					Text buttonKey = InteractSprite.transform.GetChild (1).gameObject.GetComponent<Text> ();
					Text txt = InteractSprite.gameObject.GetComponent<Text> ();
					buttonKey.text = Key;
					txt.text = name;
					if (Key == "Mouse0" || Key == "Mouse1" || Key == "Mouse2") {
						bg.sprite = GetKeySprite (Key);
						buttonKey.gameObject.SetActive (false);
					} else {
						bg.sprite = DefaultSprite;
						buttonKey.gameObject.SetActive (true);
					}
				break;
				case 2:
					InteractSprite1.SetActive (true);
					Image bg1 = InteractSprite1.transform.GetChild (0).GetComponent<Image> ();
					Text buttonKey1 = InteractSprite1.transform.GetChild (1).gameObject.GetComponent<Text> ();
					Text txt1 = InteractSprite1.gameObject.GetComponent<Text> ();
					buttonKey1.text = Key;
					txt1.text = name;
					if (Key == "Mouse0" || Key == "Mouse1" || Key == "Mouse2") {
						bg1.sprite = GetKeySprite (Key);
						buttonKey1.gameObject.SetActive (false);
					} else {
						bg1.sprite = DefaultSprite;
						buttonKey1.gameObject.SetActive (true);
					}
				break;
			}
		}
    }

	public void ShowExamineSprites(string useKey, string examineKey)
    {
		ExamineButton1.SetActive(true);
		ExamineButton1.transform.GetChild(1).gameObject.GetComponent<Text>().text = useKey;
		ExamineButton2.SetActive(true);
		ExamineButton2.transform.GetChild(1).gameObject.GetComponent<Text>().text = RotateKey;
		ExamineButton3.SetActive(true);
		ExamineButton3.transform.GetChild(1).gameObject.GetComponent<Text>().text = examineKey;

		//Use Key
		if (useKey == "Mouse0" || useKey == "Mouse1" || useKey == "Mouse2")
        {
			ExamineButton1.transform.GetChild(0).GetComponent<Image>().sprite = GetKeySprite(useKey);
			ExamineButton1.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(25, 25);
			ExamineButton1.transform.GetChild (0).GetComponent<Image> ().type = Image.Type.Simple;
			ExamineButton1.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
			ExamineButton1.transform.GetChild(0).GetComponent<Image>().sprite = DefaultSprite;
			ExamineButton1.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(34, 34);
			ExamineButton1.transform.GetChild (0).GetComponent<Image> ().type = Image.Type.Sliced;
			ExamineButton1.transform.GetChild(1).gameObject.SetActive(true);
        }
		//Rotate Key
		if (RotateKey == "Mouse0" || RotateKey == "Mouse1" || RotateKey == "Mouse2")
        {
			ExamineButton2.transform.GetChild(0).GetComponent<Image>().sprite = GetKeySprite(RotateKey);
			ExamineButton2.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(25, 25);
			ExamineButton2.transform.GetChild (0).GetComponent<Image> ().type = Image.Type.Simple;
			ExamineButton2.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
			ExamineButton2.transform.GetChild(0).GetComponent<Image>().sprite = DefaultSprite;
			ExamineButton2.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(34, 34);
			ExamineButton2.transform.GetChild (0).GetComponent<Image> ().type = Image.Type.Sliced;
			ExamineButton2.transform.GetChild(1).gameObject.SetActive(true);
        }
		//Examine Key
		if (examineKey == "Mouse0" || examineKey == "Mouse1" || examineKey == "Mouse2")
		{
			ExamineButton3.transform.GetChild(0).GetComponent<Image>().sprite = GetKeySprite(examineKey);
			ExamineButton3.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(25, 25);
			ExamineButton3.transform.GetChild (0).GetComponent<Image> ().type = Image.Type.Simple;
			ExamineButton3.transform.GetChild(1).gameObject.SetActive(false);
		}
		else
		{
			ExamineButton3.transform.GetChild(0).GetComponent<Image>().sprite = DefaultSprite;
			ExamineButton3.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(34, 34);
			ExamineButton3.transform.GetChild (0).GetComponent<Image> ().type = Image.Type.Sliced;
			ExamineButton3.transform.GetChild(1).gameObject.SetActive(true);
		}
		DownExamineUI.SetActive(true);
    }

	public void ShowGrabSprites()
	{
		GrabButton1.SetActive(true);
		GrabButton1.transform.GetChild(1).gameObject.GetComponent<Text>().text = GrabKey;
		GrabButton2.SetActive(true);
		GrabButton2.transform.GetChild(1).gameObject.GetComponent<Text>().text = RotateKey;
		GrabButton3.SetActive(true);
		GrabButton3.transform.GetChild(1).gameObject.GetComponent<Text>().text = ThrowKey;

		//Zoom Key
		GrabButton4.SetActive(true);

		//Use Key
		if (GrabKey == "Mouse0" || GrabKey == "Mouse1" || GrabKey == "Mouse2")
		{
			GrabButton1.transform.GetChild(0).GetComponent<Image>().sprite = GetKeySprite(GrabKey);
			GrabButton1.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(25, 25);
			GrabButton1.transform.GetChild (0).GetComponent<Image> ().type = Image.Type.Simple;
			GrabButton1.transform.GetChild(1).gameObject.SetActive(false);
		}
		else
		{
			GrabButton1.transform.GetChild(0).GetComponent<Image>().sprite = DefaultSprite;
			GrabButton1.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(34, 34);
			GrabButton1.transform.GetChild (0).GetComponent<Image> ().type = Image.Type.Sliced;
			GrabButton1.transform.GetChild(1).gameObject.SetActive(true);
		}
		//Rotate Key
		if (RotateKey == "Mouse0" || RotateKey == "Mouse1" || RotateKey == "Mouse2")
		{
			GrabButton2.transform.GetChild(0).GetComponent<Image>().sprite = GetKeySprite(RotateKey);
			GrabButton2.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(25, 25);
			GrabButton2.transform.GetChild (0).GetComponent<Image> ().type = Image.Type.Simple;
			GrabButton2.transform.GetChild(1).gameObject.SetActive(false);
		}
		else
		{
			GrabButton2.transform.GetChild(0).GetComponent<Image>().sprite = DefaultSprite;
			GrabButton2.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(34, 34);
			GrabButton2.transform.GetChild (0).GetComponent<Image> ().type = Image.Type.Sliced;
			GrabButton2.transform.GetChild(1).gameObject.SetActive(true);
		}
		//Throw Key
		if (ThrowKey == "Mouse0" || ThrowKey == "Mouse1" || ThrowKey == "Mouse2")
		{
			GrabButton3.transform.GetChild(0).GetComponent<Image>().sprite = GetKeySprite(ThrowKey);
			GrabButton3.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(25, 25);
			GrabButton3.transform.GetChild (0).GetComponent<Image> ().type = Image.Type.Simple;
			GrabButton3.transform.GetChild(1).gameObject.SetActive(false);
		}
		else
		{
			GrabButton3.transform.GetChild(0).GetComponent<Image>().sprite = DefaultSprite;
			GrabButton3.transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(34, 34);
			GrabButton3.transform.GetChild (0).GetComponent<Image> ().type = Image.Type.Sliced;
			GrabButton3.transform.GetChild(1).gameObject.SetActive(true);
		}
		DownGrabUI.SetActive(true);
	}

	public void HideSprites(string type)
	{
		switch (type) {
		case "Interact":
			InteractSprite.SetActive (false);
			InteractSprite1.SetActive (false);
			break;
		case "Grab":
			DownGrabUI.SetActive(false);
			break;
		case "Examine":
			DownExamineUI.SetActive(false);		
			break;
		}
	}

	public Sprite GetKeySprite(string Key)
	{
		return Resources.Load<Sprite>(Key);
	}
	
	IEnumerator FadeWaitHint()
	{
		yield return new WaitForSeconds(3f);
		startFadeHint = true;
	}
}