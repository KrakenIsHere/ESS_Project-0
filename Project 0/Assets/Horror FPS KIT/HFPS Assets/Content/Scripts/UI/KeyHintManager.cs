using UnityEngine;
using System.Collections;

public class KeyHintManager : MonoBehaviour {

	private UIManager uiManager;
	private InputManager inputManager;

	[Header("Main Setup")]
	public ScriptManager scriptManager;
	public string InputKey;
	
	[Header("Text")]
	public string  HintText;
	
	private KeyCode Key;

	void Start()
	{
		uiManager = scriptManager.uiManager;
		inputManager = scriptManager.inputManager;
	}

	void Update()
	{
		if(inputManager.DictCount() > 0)
		{
			Key = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput(InputKey));
		}
	}
	
	public void UseObject()
	{
		uiManager.ShowHint("Press " + "\"" + Key.ToString() + "\" " + HintText);
	}
}
