using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum itemType { Normal, Heal, Weapon, CombineGetWeapon, Bullets }

public class InventoryDatabase : MonoBehaviour {

	public List<ItemMapper> ItemDatabase = new List<ItemMapper> ();
	public List<Item> Items = new List<Item> ();

	[System.Serializable]
	public class ItemMapper{
		private int ID;
		public string Title;
		[Multiline]
		public string Description;
		public Sprite ItemSprite;
		public bool Stackable;
		public bool Combinable;
		public bool isUsable;
		public itemType m_itemType = itemType.Normal;

		[System.Serializable]
		public class Properties{
			public AudioClip healSound;
			public AudioClip combineSound;
			public int healAmount;
			public int combineWithID;
			public int combinedID;
			public int combinedWeaponID;

			 [HideInInspector]
			public int weaponID;
		}

		public Properties properties = new Properties ();
	}

	void Start()
	{
		for (int i = 0; i < ItemDatabase.Count; i++) {
			Items.Add (new Item (i, ItemDatabase [i].Title, ItemDatabase [i].Description, ItemDatabase [i].ItemSprite, ItemDatabase[i].Stackable, ItemDatabase[i].Combinable, ItemDatabase[i].isUsable, ItemDatabase[i].m_itemType, ItemDatabase[i].properties.healSound, ItemDatabase[i].properties.combineSound, ItemDatabase[i].properties.healAmount, ItemDatabase[i].properties.combineWithID, ItemDatabase[i].properties.combinedID, ItemDatabase[i].properties.combinedWeaponID, ItemDatabase[i].properties.weaponID));		
		}
	}

	public Item GetItemByID(int id)
	{
		for (int i = 0; i < Items.Count; i++)
			if (Items [i].ID == id)
				return Items[i];
		return null;
	}
}

public class Item
{
	public int ID{ get; set; }
	public string Title{ get; set; }
	public string Description{ get; set; }
	public Sprite ItemSprite{ get; set; }
	public bool Stackable{ get; set; }
	public bool Combinable{ get; set; }
	public bool IsUsable{ get; set; }
	public itemType m_itemType{ get; set; }

	public AudioClip healSound { get; set; }
	public AudioClip combineSound { get; set; }
	public int healAmount{ get; set; }
	public int combineWithID{ get; set; }
	public int combinedID{ get; set; }
	public int combinedWeaponID{ get; set; }

	public int weaponID{ get; set; }

	public Item(int id, string title, string description, Sprite itemsprite, bool stackable, bool combinable,  bool isusable, itemType itemtype, AudioClip healsound, AudioClip combinesound, int healamount, int combinewithid, int combinedid, int combinedweapid, int weaponid)
	{
		this.ID = id;
		this.Title = title;
		this.Description = description;
		this.ItemSprite = itemsprite;
		this.Stackable = stackable;
		this.Combinable = combinable;
		this.IsUsable = isusable;
		this.m_itemType = itemtype;

		this.healSound = healsound;
		this.combineSound = combinesound;

		this.healAmount = healamount;
		this.combineWithID = combinewithid;
		this.combinedID = combinedid;
		this.combinedWeaponID = combinedweapid;

		this.weaponID = weaponid;
	}
}
