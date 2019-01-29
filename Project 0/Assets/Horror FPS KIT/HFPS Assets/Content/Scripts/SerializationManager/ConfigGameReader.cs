/* ConfigGameReader.cs by TWGames - One script full config actions.. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TW.Configuration;

public class ConfigGameReader : MonoBehaviour {
	ConfigManager config = new ConfigManager();

	private List<string> ConfigKeys = new List<string> ();

	public string ConfigName = "GameConfig";
	public bool showDebug;
	public bool isMainMenu;

	private string configName;
	private bool UsePlayerPrefs;

	private bool refreshStatus = false;

	void Start () {
		configName = PlayerPrefs.GetString("GameConfig");
		if(PlayerPrefs.HasKey("UsePlayerPrefs")){
			UsePlayerPrefs = System.Convert.ToBoolean(PlayerPrefs.GetString ("UsePlayerPrefs"));
		}

		if (!isMainMenu) {
			if (UsePlayerPrefs) {
				if (config.ExistFile (configName)) {
					config.Setup (showDebug, configName);
				} else {
					if (config.ExistFile (ConfigName)) {
						config.Setup (showDebug, ConfigName);
					} else {
						Debug.LogError ("\"" + configName + "\"" + " and " + "\"" + ConfigName + "\"" + " does not exist, try launching scene from Main Menu");
						Debug.LogError ("Player will not move if GameConfig does not exist in Data folder");
					}
				}
			} else {
				if (config.ExistFile (ConfigName)) {
					config.Setup (showDebug, ConfigName);
				} else {
					Debug.LogError ("\"" + ConfigName + "\"" + " does not exist, try launching scene from Main Menu or run scene again");
					Debug.LogError ("Player will not move if GameConfig does not exist in Data folder");
				}
			}
		} else {
			if (config.ExistFile (configName)) {
				config.Setup (showDebug, configName);
			} else {
				config.Setup (showDebug, ConfigName);
			}
		}

		for (int i = 0; i < config.DictKeys.Count; i++) {
			ConfigKeys.Add (config.DictKeys [i]);
		}
	}
		
	public string Deserialize(string Section, string Key)
	{
		return config.Deserialize (Section, Key);
	}

	public void Serialize(string Section, string Key, string Value)
	{
		config.Serialize (Section, Key, Value);
	}

	public bool ContainsSection(string Section) 
	{
		return config.ContainsSection (Section);
	}

	public bool ContainsSectionKey(string Section, string Key) {
		return config.ContainsSectionKey (Section, Key);
	}

	public bool ContainsKeyValue(string Section, string Key, string Value) {
		return config.ContainsKeyValue (Section, Key, Value);
	}

	public void RemoveSectionKey(string Section, string Key) {
		config.RemoveSectionKey (Section, Key);
	}

	public bool ExistFile(string file)
	{
		return config.ExistFile (file);
	}

	public string GetKey(int index)
	{
		return ConfigKeys [index];
	}

	public int GetKeysCount()
	{
		return config.DictKeys.Count;
	}

	public int GetKeysSectionCount(string Section)
	{
		return config.GetSectionKeys (Section);
	}

	public void Refresh()
	{
		StartCoroutine (WaitRefresh ());
	}

	IEnumerator WaitRefresh()
	{
		refreshStatus = true;
		yield return new WaitForSeconds (1f);
		refreshStatus = false;
	}

	public bool GetRefreshStatus()
	{
		return refreshStatus;
	}
}
