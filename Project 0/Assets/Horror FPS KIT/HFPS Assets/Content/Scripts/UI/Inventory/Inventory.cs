using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IPointerClickHandler {

	private InventoryDatabase inventoryData;
	private UIManager uiManager;
	private ItemSwitcher switcher;

	[Header("Main")]
	public GameObject SlotsPanel;
	public GameObject UseButton;
	public GameObject CombineButton;
	public Text ItemLabel;
	public Text ItemDescription;

	[Header("Inventory Sprites")]
	public Sprite inventorySlotFilled;
	public GameObject inventorySlot;
	public GameObject inventoryItem;

	[Header("Inventory Items")]
	public int slotAmout;
	public int maxSlots = 16;

	[HideInInspector]
	public int selectedID;

	public List<Item> items = new List<Item> ();
	public List<GameObject> slots = new List<GameObject> ();

	[Header("Inventory Settings")]
	public Color normalColor = Color.white;
	public Color selectedColor = Color.white;
	public Color combineColor = Color.white;

	void Start () {
		inventoryData = GetComponent<InventoryDatabase> ();
		uiManager = GetComponent<UIManager>();

		for (int i = 0; i < slotAmout; i++) {
			slots.Add(Instantiate(inventorySlot));
			slots [i].GetComponent<InventorySlot> ().id = i;
			slots [i].transform.SetParent (SlotsPanel.transform);
		}

		ItemLabel.text = "";
		ItemDescription.text = "";
	}

	void Update()
	{
		if (!switcher) {
			switcher = uiManager.scriptManager.itemSwitcher;
		}

		if (!uiManager.TabButtonPanel.activeSelf) {
			CombineButton.SetActive (false);
			UseButton.SetActive (false);
			for (int i = 0; i < slots.Count; i++) {
				slots [i].GetComponent<InventorySlot> ().isCombining = false;
				slots [i].GetComponent<InventorySlot> ().isCombinable = false;
			}
		}
	}

	public void AddItemToSlot(int id, int amount)
	{
		Item itemToAdd = inventoryData.GetItemByID (id);
		if (CheckInventorySpace()) {
			if (itemToAdd.Stackable && CheckItemInventory (itemToAdd) && GetSlotByItem (itemToAdd) != -1) {
				InventoryItemData itemData = slots [GetSlotByItem (itemToAdd)].transform.GetChild (0).GetComponent<InventoryItemData> ();
				itemData.amount = itemData.amount + amount;
			} else {
				for (int i = 0; i < slots.Count; i++) {
					if (slots [i].transform.childCount < 1) {
						items.Add (itemToAdd);
						GameObject item = Instantiate (inventoryItem);
						item.GetComponent<InventoryItemData> ().item = itemToAdd;
						item.GetComponent<InventoryItemData> ().amount = amount;
						item.GetComponent<InventoryItemData> ().slotID = i;
						item.transform.SetParent (slots [i].transform);         
						item.GetComponent<Image> ().sprite = itemToAdd.ItemSprite;
						item.GetComponent<RectTransform> ().position = Vector2.zero;
						item.name = itemToAdd.Title;
						break;
					}
				}
			}
		}
	}

	public void SetWeaponID(int id, int weaponID){
		Item item = inventoryData.GetItemByID (id);
		item.weaponID = weaponID;
	}

	int GetSlotByItem(Item item)
	{
		for (int i = 0; i < slots.Count; i++) {
			if (slots [i].transform.childCount > 0 && slots [i].transform.GetChild (0).GetComponent<InventoryItemData> ().item == item)
				return i;
		}
		return -1;
	}

	public void RemoveItem (int id)
	{
		Item itemToRemove = inventoryData.GetItemByID (id);
		if (itemToRemove.Stackable && CheckItemInventory (itemToRemove)) {
			InventoryItemData data = slots [GetSlotByItem (itemToRemove)].transform.GetChild (0).GetComponent<InventoryItemData> ();
			data.amount--;
			data.transform.GetChild (0).GetComponent<Text> ().text = data.amount.ToString ();
			if (data.amount == 0) {
				Destroy (slots [GetSlotByItem (itemToRemove)].transform.GetChild (0).gameObject);
				items.Remove (itemToRemove);
			}
			if (data.amount == 1) {
				slots [GetSlotByItem (itemToRemove)].transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = "";
			}
		} else {
			Destroy (slots [GetSlotByItem (itemToRemove)].transform.GetChild (0).gameObject);
			items.Remove (itemToRemove);
		}
	}

	public void RemoveItemAmount (int id, int amount)
	{
		Item itemToRemove = inventoryData.GetItemByID (id);
		if (CheckItemInventory (itemToRemove)) {
			InventoryItemData data = slots [GetSlotByItem (itemToRemove)].transform.GetChild (0).GetComponent<InventoryItemData> ();
			data.amount = data.amount - amount;
			data.transform.GetChild (0).GetComponent<Text> ().text = data.amount.ToString ();
			if (data.amount <= 0 && itemToRemove.m_itemType != itemType.Weapon) {
				Destroy (slots [GetSlotByItem (itemToRemove)].transform.GetChild (0).gameObject);
				items.Remove (itemToRemove);
			}
		}
	}

	public void ExpandSlots(int slotsAmount)
	{
		int extendedSlots = slotAmout + slotsAmount;
		if (extendedSlots > maxSlots) {
			uiManager.WarningMessage ("Cannot carry more backpacks");
			return;
		}
		for (int i = slotAmout; i < extendedSlots; i++) {
			slots.Add(Instantiate(inventorySlot));
			slots [i].GetComponent<InventorySlot> ().id = i;
			slots [i].transform.SetParent (SlotsPanel.transform);
		}
		slotAmout = extendedSlots;
	}

	public bool CheckInventorySpace()
	{
		for (int i = 0; i < slots.Count; i++) {
			if (slots [i].transform.childCount < 1)
				return true;
		}
		return false;
	}

	bool CheckItemInventory(Item item)
	{
		for (int i = 0; i < items.Count; i++) {
			if (items [i].ID == item.ID)
				return true;
		}
		return false;
	}

	public bool CheckItemIDInventory(int ItemID)
	{
		for (int i = 0; i < items.Count; i++) {
			if (items [i].ID == ItemID)
				return true;
		}
		return false;
	}

	int GetSlotId(int itemID)
	{
		for (int i = 0; i < slots.Count; i++) {
			if (slots [i].transform.childCount > 0)
				if (slots [i].transform.GetChild (0).GetComponent<InventoryItemData> ().item.ID == itemID)
					return i;
		}
		return -1;
	}

	public int GetItemAmount(int itemID)
	{
		for (int i = 0; i < slots.Count; i++) {
			if (slots [i].transform.childCount > 0)
			if (slots [i].transform.GetChild (0).GetComponent<InventoryItemData> ().item.ID == itemID)
				return slots [i].transform.GetChild (0).GetComponent<InventoryItemData> ().amount;
		}
		return -1;
	}

	public void SetItemAmount(int itemID, int amount)
	{
		for (int i = 0; i < slots.Count; i++) {
			if (slots [i].transform.childCount > 0)
			if (slots [i].transform.GetChild (0).GetComponent<InventoryItemData> ().item.ID == itemID)
				slots [i].transform.GetChild (0).GetComponent<InventoryItemData> ().amount = amount;
		}
	}

	public void UseItem()
	{
		Item usableItem = slots [selectedID].transform.GetChild (0).GetComponent<InventoryItemData> ().item;
		if (usableItem.m_itemType == itemType.Heal) {
			uiManager.hm.ApplyHeal (usableItem.healAmount);
			if (!uiManager.hm.isMaximum) {
				if (usableItem.healSound) {
					AudioSource.PlayClipAtPoint (usableItem.healSound, Camera.main.transform.position, 1.0f);
				}
				RemoveItem (usableItem.ID);
			}
		} else if (usableItem.m_itemType == itemType.Weapon) {
			switcher.selectItem (usableItem.weaponID);
		}
		CombineButton.SetActive (false);
		UseButton.SetActive (false);
		UseButton.transform.GetChild (0).GetComponent<MenuEvents> ().ChangeTextColor ("Black");
	}

	public void CombineItem()
	{
		int combineWithID = slots [selectedID].transform.GetChild (0).GetComponent<InventoryItemData> ().item.combineWithID;
		for (int i = 0; i < slots.Count; i++) {
			slots [i].GetComponent<InventorySlot> ().isCombining = true;
			if (slots [i] != slots [GetSlotId (combineWithID)]) {
				slots [i].GetComponent<Image> ().color = combineColor;
				slots [i].GetComponent<InventorySlot> ().isCombinable = false;
			} else {
				slots [i].GetComponent<InventorySlot> ().isCombinable = true;
			}
		}
	}

	public void CombineWith(Item item, int id)
	{
		if (id != selectedID) {
			int CombinedItemID = item.combinedID;
			Item A = slots [selectedID].transform.GetChild(0).GetComponent<InventoryItemData> ().item;

			for (int i = 0; i < slots.Count; i++) {
				slots [i].GetComponent<InventorySlot> ().isCombining = false;
				slots [i].GetComponent<Image> ().color = normalColor;
			}

			CombineButton.SetActive (false);
			CombineButton.transform.GetChild (0).GetComponent<MenuEvents> ().ChangeTextColor ("Black");
			UseButton.SetActive (false);

			if (A.combineSound) {
				AudioSource.PlayClipAtPoint (A.combineSound, Camera.main.transform.position, 1.0f);
			}

			if(item.combineSound){
				AudioSource.PlayClipAtPoint (item.combineSound, Camera.main.transform.position, 1.0f);
			}

			if (A.m_itemType == itemType.CombineGetWeapon) {
				int weaponID = A.combinedWeaponID;
				RemoveItem (A.ID);

				switcher.selectItem (weaponID);
			} else {
				RemoveItem (A.ID);
				RemoveItem (item.ID);

				StartCoroutine (WaitForRemove (item, CombinedItemID));
			}
		}
	}

	IEnumerator WaitForRemove(Item item, int combinedID)
	{
		yield return new WaitUntil (() => !CheckItemInventory(item));
		AddItemToSlot (combinedID, 1);
	}

	public void Deselect(int id){
		slots [id].GetComponent<Image> ().color = normalColor;
		slots [id].transform.GetChild (0).GetComponent<InventoryItemData> ().selected = false;
		ItemLabel.text = "";
		ItemDescription.text = "";
		selectedID = -1;
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if (uiManager.TabButtonPanel.activeSelf) {
			for (int i = 0; i < slots.Count; i++) {
				slots [i].GetComponent<InventorySlot> ().isCombining = false;
				slots [i].GetComponent<InventorySlot> ().isCombinable = false;
			}
			if (selectedID != -1) {
				slots [selectedID].GetComponent<Image> ().color = normalColor;
				slots [selectedID].transform.GetChild (0).GetComponent<InventoryItemData> ().selected = false;
				CombineButton.SetActive (false);
				UseButton.SetActive (false);
				ItemLabel.text = "";
				ItemDescription.text = "";
				selectedID = -1;
			}
		}
	}
}
