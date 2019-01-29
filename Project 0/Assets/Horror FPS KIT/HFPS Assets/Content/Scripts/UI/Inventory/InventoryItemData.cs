using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public Item item;
	public string itemTitle;
	public int amount;
    public int slotID;

	[HideInInspector]
	public bool selected;

	[HideInInspector]
	public bool isDisabled;

	private Text textAmount;
	private Inventory inventory;
    private Vector2 offset;

	void Start()
	{
		inventory = transform.root.GetComponent<UIManager> ().inventoryScript;
		this.transform.position = transform.parent.transform.position;
	}

	void Update()
	{
		textAmount = transform.GetChild (0).gameObject.GetComponent<Text> ();
		if (item.m_itemType == itemType.Bullets || item.m_itemType == itemType.Weapon) {
			textAmount.text = amount.ToString ();
		} else {
			if (amount > 1) {
				textAmount.text = amount.ToString ();
			} else if (amount == 1) {
				textAmount.text = "";
			}
		}

		itemTitle = item.Title;
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
		if (item != null && !isDisabled)
        {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.position = eventData.position - offset;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
		if (item != null && !isDisabled)
        {
            this.transform.position = eventData.position - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
		if (!isDisabled) {
			this.transform.SetParent (inventory.slots [slotID].transform);
			this.transform.position = inventory.slots [slotID].transform.position;
			GetComponent<CanvasGroup> ().blocksRaycasts = true;
		}
    }

	void OnDisable()
	{
		if(selected)
		inventory.Deselect (slotID);
	}
}
