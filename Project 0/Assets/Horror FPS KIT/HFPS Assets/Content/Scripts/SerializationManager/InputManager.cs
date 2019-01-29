using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {
	private ConfigGameReader configReader;

	Dictionary<string,string> Inputs = new Dictionary<string,string>();

	private bool isRefreshed = false;
	private bool firstRead = false;

	void Start () {
		configReader = GetComponent<ConfigGameReader> ();
	}

	void Update ()
	{
		if (configReader && !firstRead && configReader.ContainsSection ("Input")) {
			Deserialize ();
			isRefreshed = true;
			firstRead = true;
		}
	}
	
	void Deserialize() {
		for (int i = 0; i < configReader.GetKeysSectionCount("Input"); i++){
			string Key = configReader.GetKey(i);
			string Value = configReader.Deserialize("Input", Key);
			Inputs.Add(Key, Value);
		}
	}

	public void RefreshInputs()
	{
		Inputs.Clear ();
		Deserialize ();
		isRefreshed = true;
		StartCoroutine (RefreshWait ());
	}

	public bool GetRefreshStatus()
	{
		if (Inputs.Count > 0 && isRefreshed) {
			return true;
		} else {
			return false;
		}
	}
	
	public int DictCount()
	{
		return Inputs.Count;
	}
	
	public string GetInput(string Key)
	{
		if (Inputs.ContainsKey (Key)) {
			return Inputs [Key];
		} else {
			if (configReader.showDebug) {Debug.LogError ("No key with this name found");}
			return null;
		}
	}

	IEnumerator RefreshWait()
	{
		yield return new WaitForSeconds (2f);
		if (configReader.showDebug) {Debug.Log("Refresh State = False");}
		isRefreshed = false;
	}
}