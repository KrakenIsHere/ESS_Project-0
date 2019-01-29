using UnityEngine;

public class ItemID : MonoBehaviour {

	public enum Type { NoInventoryItem, InventoryItem, WeaponItem, BackpackExpand }
	public enum WeaponType { Weapon, Ammo }

	public Type ItemType = Type.InventoryItem;
	public WeaponType weaponType = WeaponType.Weapon;
	public int Amount = 1;

	public int WeaponID;
	public int InventoryID;
	public int BackpackExpand;
	public bool DestroyOnPickup;

	public void UseObject()
	{
		if(DestroyOnPickup)
		Destroy (this.gameObject);
	}
}
