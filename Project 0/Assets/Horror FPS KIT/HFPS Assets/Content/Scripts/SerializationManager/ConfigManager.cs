/*
namespace(Configuration) - ConfigManager.cs by ThunderWire Games (Script for saving and reading config files)
use "using Configuration;" to top of the script;
*/

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace TW.Configuration {
	public class ConfigManager {

		public static string filepath;
		public static string filename;
		private string config;
		
		public static Dictionary<string, Dictionary<string, string>> ConfigDictionary = new Dictionary<string, Dictionary<string, string>>();
		public List<string> DictKeys = new List<string>();

		private bool showDebug;

		public void Setup(bool debug, string file)
		{
			showDebug = debug;
			filepath = Application.dataPath + "/Data/";
			filename = filepath + file + ".cfg";
			config = file;
			if (!Directory.Exists(filepath)){
				Directory.CreateDirectory(filepath);
			}
			ReadFromConfig();
			if(showDebug){Debug.Log("<color=green>Config File Setup:</color> " + filename);}
			if(!(File.Exists(filename))){
				if(showDebug){Debug.Log("<color=red>Config File Not Found:</color> " + filename);}
			}
		}

		public void SetupFolder(bool debug, string folder, string file)
		{
			showDebug = debug;
			filepath = Application.dataPath + folder + "/Data/";
			filename = filepath + file + ".cfg";
			if (!Directory.Exists(filepath)){
				Directory.CreateDirectory(filepath);
			}
			ReadFromConfig();
			if(showDebug){Debug.Log("<color=green>Config File Setup:</color> " + filename);}
			if(!(File.Exists(filename))){
				if(showDebug){Debug.Log("<color=red>Config File Not Found:</color> " + filename);}
			}
		}

		public void Serialize(string Section, string Key, string Value) {
			if(!string.IsNullOrEmpty(Section))
			{
				if(!string.IsNullOrEmpty(Key))
				{
					if(!string.IsNullOrEmpty(Value))
					{
						UpdateDictionary (Section, Key, Value);
						WriteDictionary ();
					}
					else
						if(showDebug){Debug.Log("<color=yellow>Add key value!</color>");}
				}
				else
					if(showDebug){Debug.Log("<color=yellow>Add key name!</color>");}
			}
			else
				if(showDebug){Debug.Log("<color=yellow>Add section name!</color>");}
		}
			
		
		public string Deserialize(string Section, string Key) {
			if (ConfigDictionary.ContainsKey (Section)) {
				if (ConfigDictionary [Section].ContainsKey (Key)) {
					return ConfigDictionary [Section] [Key];
				} else {
					if (showDebug) {
						Debug.Log ("<color=yellow>No key in this section found</color>");
					}
					return null;
				}
			} else {
				if(showDebug){Debug.Log("<color=yellow>No section with this name found</color>");}
				return null;
			}
		}
		
		public bool ContainsSection(string Section) {
			if (ExistFile (config)) {
				return ConfigDictionary.ContainsKey (Section);
			} else {
				return false;
			}
		}
		
		public bool ContainsSectionKey(string Section, string Key) {
			if (ExistFile (config)) {
				return ConfigDictionary [Section].ContainsKey (Key);
			} else {
				return false;
			}
		}

		public bool ContainsKeyValue(string Section, string Key, string Value) {
			if (ExistFile (config)) {
				return ConfigDictionary [Section] [Key].Contains (Value);
			} else {
				return false;
			}
		}
		
		public void RemoveSectionKey(string Section, string Key) {
			if(ConfigDictionary[Section].ContainsKey(Key))
			{
				Debug.Log(ConfigDictionary[Section].Count);
				if (ConfigDictionary [Section].Count == 1) {
					ConfigDictionary.Remove (Section);
					ReplaceValues ();					
				} else {
					ConfigDictionary [Section].Remove (Key);
					ReplaceValues ();
				}
			}
		}
		
		public void RemoveSection(string Section) {
			if (ConfigDictionary.ContainsKey(Section)) 
			{
				ConfigDictionary.Remove(Section);
				ReplaceValues();
			}
		}

		public int GetSectionKeys(string Section)
		{
			return ConfigDictionary [Section].Count;
		}
		
		public bool ExistFile(string file) {
			if (File.Exists(GetFileAndPath(file))){
				return true;
			}else{
				return false;
			}
		}
			
		public bool ExistFileInFolder(string file, string folder) {
			if (File.Exists(GetFileAndPathFolder(file, folder))){
				return true;
			}else{
				return false;
			}
		}

		public bool ExistFileWithPath(string path, string file) {
			string defaultPath = path + file + ".cfg";
			if (File.Exists(defaultPath)) {
				return true;
			}else{
				return false;
			}
		}
		
		public void RemoveFile(string file) {
			string pathfile = filepath + file + ".cfg";
			if (File.Exists(pathfile)) {
				File.Delete(pathfile);
				if(showDebug){Debug.Log("<color=red>Config File Removed:</color> " + pathfile);}
			}else{
				if(showDebug){Debug.Log("<color=yellow>Config File Not Found:</color> " + pathfile);}
			}
		}

		public void DuplicateFile(string file, string name){
			string pathfile = filepath + file + ".cfg";
			if (File.Exists(pathfile)) {
				File.Copy(pathfile, filepath + name + ".cfg");
			}else{
				if(showDebug){Debug.Log("<color=yellow>Config File Not Found:</color> " + pathfile);}
			}
		}

		public string GetDataPath ()
		{
			return Application.dataPath + "/Data/";
		}
		
		public string GetFileAndPath(string file)
		{
			return Application.dataPath + "/Data/" + file + ".cfg";
		}

		public string GetFileAndPathFolder(string file, string folder)
		{
			return Application.dataPath + folder + "/Data/" + file + ".cfg";
		}
		
		private static void ReplaceValues() {
			using (StreamWriter sw = new StreamWriter(filename)) {
				foreach (KeyValuePair<string, Dictionary<string, string>> Section in ConfigDictionary) {
					sw.WriteLine("[" + Section.Key + "]");
					foreach (KeyValuePair<string, string> InSection in Section.Value) {
						// value must be in one line
						string key = InSection.Key.ToString();
						string value = InSection.Value.ToString();
						sw.WriteLine(key + " \"" + value + "\"");
					}
				}
			}
		}
		
		private void ReadFromConfig()
		{
			if (File.Exists (filename)) {
				using (StreamReader reader = new StreamReader (filename)) {
					string line;
					string m_Section = "";
					string m_Key = "";
					string m_Value = "";
					while (!string.IsNullOrEmpty (line = reader.ReadLine ())) {
						line.Trim ();
						if (line.StartsWith ("[") && line.EndsWith ("]")) {
							m_Section = line.Substring (1, line.Length - 2);
							m_Key = "";
							m_Value = "";
						} else {
							string[] ln_Input = line.Split (new char[] { '"' });
							m_Key = ln_Input [0].Trim ();
							m_Value = ln_Input [1].Trim ();
						}
						if (m_Section == "" || m_Key == "" || m_Value == "")
							continue;
						UpdateDictionary (m_Section, m_Key, m_Value);
						DictKeys.Add (m_Key);
					}
				}
			}
		}
		
		private static void UpdateDictionary(string Section, string Key, string Value) {
			if (ConfigDictionary.ContainsKey(Section)) {
				if (ConfigDictionary[Section].ContainsKey(Key)){
					ConfigDictionary[Section][Key] = Value;
				}else{
					ConfigDictionary[Section].Add(Key, Value);
				}
			}else{
				Dictionary<string, string> KeyValueDict = new Dictionary<string, string>();
				KeyValueDict.Add(Key, Value);
				ConfigDictionary.Add(Section, KeyValueDict);
			}
		}

		private static void WriteDictionary() {
			using (StreamWriter sw = new StreamWriter (filename)) {
				foreach (KeyValuePair<string, Dictionary<string, string>> Section in ConfigDictionary) {
					string section = Section.Key.ToString ();
					sw.WriteLine ("[" + section + "]");
					foreach (KeyValuePair<string, string> InSection in Section.Value) {
						string key = InSection.Key.ToString ();
						string value = InSection.Value.ToString ();
						value = value.Replace (Environment.NewLine, " ");
						value = value.Replace ("\r\n", " ");
						sw.WriteLine (key + " \"" + value + "\"");
					}
				}
			}
		}
	}
}
