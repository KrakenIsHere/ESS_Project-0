using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleItem : MonoBehaviour {

	[Header("Setup")]
	public ScriptManager scriptManager;
	private InputManager inputManager;

	private Inventory inventory;

	[Header("Candle Others")]
	public int InventoyID;
	public AudioClip BlowOut;
	public GameObject CandleFlame;
	public GameObject CandleLight;

	[Header("Candle Animations")]
	public GameObject CandleGO;
	public string DrawAnimation;
	public string HideAnimation;

	public float DrawSpeed = 1f;
	public float HideSpeed = 1f;

	private KeyCode BlowOutKey;

	private bool isSelected;
	private bool IsPressed;

	void Start () {
		inputManager = scriptManager.inputManager;
	}

	public void Select() {
		isSelected = true;
		CandleGO.SetActive (true);
		CandleGO.GetComponent<Animation>().Play(DrawAnimation);
		CandleFlame.SetActive (true);
		CandleLight.SetActive (true);
		if(inventory.CheckItemIDInventory(InventoyID)){
			inventory.RemoveItem (InventoyID);
		}
	}

	public void Deselect()
	{
		if (CandleGO.activeSelf) {
			CandleGO.GetComponent<Animation>().Play(HideAnimation);
			IsPressed = true;
		}
	}

	public void BlowOut_Event()
	{
		AudioSource.PlayClipAtPoint (BlowOut, Camera.main.transform.position, 0.35f);
		CandleFlame.SetActive (false);
		CandleLight.SetActive (false);
		inventory.AddItemToSlot (InventoyID, 1);
	}

	void Update () {
		if (!inventory) {
			inventory = scriptManager.inventory;
		}

		if(inputManager.DictCount() > 0)
		{
			BlowOutKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Flashlight"));
		}

		if (Input.GetKeyDown (BlowOutKey) && !IsPressed && isSelected && !(CandleGO.GetComponent<Animation> ().isPlaying)) {
			CandleGO.GetComponent<Animation>().Play(HideAnimation);
			IsPressed = true;
		}

		if (IsPressed && !(CandleGO.GetComponent<Animation> ().isPlaying)) {
			CandleGO.SetActive (false);
			IsPressed = false;
		}
	}
}
