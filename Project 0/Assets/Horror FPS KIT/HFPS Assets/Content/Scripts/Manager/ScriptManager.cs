using UnityEngine;
using System.Collections;

public class ScriptManager : MonoBehaviour {

    [Header("Script Connections")]
	public ItemSwitcher itemSwitcher;
	public InputManager inputManager;
	public UIManager uiManager;

	[HideInInspector]
	public InteractManager interact;

	[HideInInspector]
	public Inventory inventory;

	[HideInInspector]
	public PlayerFunctions pFunctions;

	void Start(){
		interact = GetComponent<InteractManager> ();
		inventory = uiManager.inventoryScript;
		pFunctions = GetComponent<PlayerFunctions> ();
	}
}
