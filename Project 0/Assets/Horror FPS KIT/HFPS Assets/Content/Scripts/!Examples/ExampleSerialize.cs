using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TW.Configuration; //To use ConfigManager you have to add the namespace by adding "using TW.Configuration;"

public class ExampleSerialize : MonoBehaviour {
ConfigManager config = new ConfigManager(); //Then you must add in script behavior "ConfigManager config = new ConfigManager();"

	[Tooltip("Name of the config file")]
	public string ConfigName = "GameConfig"; 

	public bool showDebug;

	[Tooltip("Set this to true if you want use PlayerPrefs to save and load config path.")]
	public bool usePlayerPrefs;

	public InputField Section;
	public InputField Key;
	public InputField Value;

	void Start () {
		string configName = PlayerPrefs.GetString("GameConfig");

		if (usePlayerPrefs) {
			config.Setup (showDebug, configName);
		} else {
			config.Setup (showDebug, ConfigName);
		}
	}
	
	public void SerializeValue()
	{
		string m_section = Section.text;
		string m_key = Key.text;
		string m_value = Value.text;
		config.Serialize(m_section, m_key, m_value);
	}
	
	public void DeserializeValue()
	{
		string m_section = Section.text;
		string m_key = Key.text;
		Value.text = config.Deserialize(m_section, m_key);
	}
	
	public void RemoveKey()
	{
		string m_section = Section.text;
		string m_key = Key.text;
		config.RemoveSectionKey(m_section, m_key);
	}
	
	public void RemoveSection()
	{
		string m_section = Section.text;
		config.RemoveSection(m_section);
	}
	
	public void RemoveFile()
	{
		config.RemoveFile(ConfigName);
	}
}
