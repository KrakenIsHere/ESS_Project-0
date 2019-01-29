/* InputController.cs by ThunderWire Games (Simple script for rebindable Input) * FIXED */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using TW.Configuration;

public class InputController : MonoBehaviour {
	ConfigManager config = new ConfigManager ();

	[Tooltip("Name of the config file")]
	public string ConfigName = "GameConfig";
	public GameObject ExistText;

	public bool showDebug;

	[Tooltip("Set this to true if you want use PlayerPrefs to save and load config name from PlayerPrefs.")]
	public bool UsePlayerPrefs;
	private bool usePP;

	[Tooltip("Set this to true if you want to save config name to PlayerPrefs.")]
	public bool SetPlayerPrefs;

	[System.Serializable]
	public class m_inputs
	{
		public string Input;
		public KeyCode DefaultKey;
		public GameObject InputButton;
	}

	[Tooltip("All rebindable buttons must be added here")]
	public m_inputs[] Inputs;
	private List<string> InputKeysCache = new List<string> ();

	private bool rebind;
	private Text buttonText;
	private Button button;
	private string inputName;
	private string defaultKey;

	private string inputConfig;
	private string configFolder;
	private bool isExist;
	
	void Start () {
		string configName = PlayerPrefs.GetString("GameConfig");
		if(PlayerPrefs.HasKey("UsePlayerPrefs")){
			usePP = System.Convert.ToBoolean(PlayerPrefs.GetString ("UsePlayerPrefs"));
		}

		if (SetPlayerPrefs) {
			if (config.ExistFile (ConfigName)) {
				config.Setup (showDebug, ConfigName);
				isExist = true;
			} else {
				config.Setup (showDebug, ConfigName);
				Debug.LogError (ConfigName + " does not exist in the Data folder (Recreating Config File)");
				isExist = false;
			}

			PlayerPrefs.SetString ("GameConfig", ConfigName);
			if (UsePlayerPrefs) {
				PlayerPrefs.SetString ("UsePlayerPrefs", "True");
			} else {
				PlayerPrefs.SetString ("UsePlayerPrefs", "False");
			}
		} else {
			if (usePP) {
				if (config.ExistFile (configName)) {
					config.Setup (showDebug, configName);
					isExist = true;
				} else {
					if (config.ExistFile (ConfigName)) {
						config.Setup (showDebug, ConfigName);
						Debug.LogWarning (configName + " does not exist in the Data folder (Using default GameConfig)");
						isExist = true;
					} else {
						config.Setup (showDebug, ConfigName);
						Debug.LogError (ConfigName + " does not exist in the Data folder");
						isExist = false;
					}
				}
			} else {
				if (config.ExistFile (ConfigName)) {
					config.Setup (showDebug, ConfigName);
					isExist = true;
				} else {
					config.Setup (showDebug, ConfigName);
					isExist = false;
				}
			}
		}
			
		ExistText.SetActive (false);

		if(isExist)
		{
			Deserialize();
		}else{
			UseDefault();
		}

		LoadInputsToList ();
	}
	
	public void RebindSelected()
	{
        var go = EventSystem.current.currentSelectedGameObject;	
		for (int i= 0; i < Inputs.Length; i++)
		{
			if (go.name == Inputs[i].InputButton.name && !rebind)
			{
				buttonText = Inputs[i].InputButton.transform.GetChild(0).gameObject.GetComponent<Text>();
				button = Inputs[i].InputButton.GetComponent<Button>();
				defaultKey = buttonText.text;
				buttonText.text = "Press Button";
				inputName = Inputs[i].Input;
				button.interactable = false;
				rebind = true;
			}
		}
	}
	
	void Update()
	{
		foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown (kcode) && rebind) {
				if (kcode.ToString () == defaultKey) {
					buttonText.text = defaultKey;
					buttonText = null;
					inputName = null;
					button.interactable = true;
					button = null;
					rebind = false;
				} else {
					RebindKey (kcode.ToString ());
				}
			}
		}
	}

	void RebindKey(string kcode)
	{
		if (!CheckForExistButton(kcode)) {
			buttonText.text = kcode;
			SerializeInput (inputName, kcode);
			UpdateInputCache();
			buttonText = null;
			inputName = null;
			button.interactable = true;
			button = null;
			rebind = false;
		} else {
			ExistText.GetComponent<Text> ().text = "Input button \"" + kcode + "\" is already used!";
			buttonText.text = defaultKey;
			StartCoroutine (ShowTextExist ());
			buttonText = null;
			inputName = null;
			button.interactable = true;
			button = null;
			rebind = false;
		}
	}

	IEnumerator ShowTextExist()
	{
		ExistText.SetActive (true);
		yield return new WaitForSeconds (3);
		ExistText.SetActive (false);
	}

	bool CheckForExistButton(string Key)
	{
		if (InputKeysCache.Contains (Key)) {
			return true;
		} else {
			return false;
		}
	}
	
	void SerializeInput(string input, string button)
	{
		config.Serialize("Input", input, button);
	}

	void LoadInputsToList()
	{
		for (int i= 0; i < Inputs.Length; i++)
		{
			string value = config.Deserialize("Input", Inputs[i].Input);
			InputKeysCache.Add (value);
		}
	}

	void UpdateInputCache()
	{
		InputKeysCache.Clear ();
		for (int i= 0; i < Inputs.Length; i++)
		{
			string value = config.Deserialize("Input", Inputs[i].Input);
			InputKeysCache.Add (value);
		}
	}
	
	void Deserialize()
	{
		for (int i= 0; i < Inputs.Length; i++)
		{
			string value = config.Deserialize("Input", Inputs[i].Input);
			Text bText = Inputs[i].InputButton.transform.GetChild(0).gameObject.GetComponent<Text>();
			bText.text = value;
			if(string.IsNullOrEmpty(bText.text))
			{
				SerializeInput(Inputs[i].Input, Inputs[i].DefaultKey.ToString());
				Deserialize();
			}
		}
	}
	
	void UseDefault()
	{
		for (int i= 0; i < Inputs.Length; i++)
		{
			Text bText = Inputs[i].InputButton.transform.GetChild(0).gameObject.GetComponent<Text>();
			string keycode = Inputs[i].DefaultKey.ToString();
			if(!(keycode == "None"))
			{
				bText.text = keycode;
				SerializeInput(Inputs[i].Input, keycode);
			}else{
				bText.text = "Set DefaultKey!";
			}
		}
	}
}
