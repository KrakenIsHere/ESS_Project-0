using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerVolume : MonoBehaviour {
	
	private ConfigGameReader configReader;
	
	void Start()
	{
		configReader = GameObject.Find ("GAMEMANAGER").GetComponent<ConfigGameReader> ();
		if (configReader && configReader.ContainsSection("Game")) {
			float volume = float.Parse(configReader.Deserialize ("Game", "Volume"));
			AudioListener.volume = volume;
		}
	}

	void Update () {
		if (configReader && configReader.ContainsSection("Game") && configReader.GetRefreshStatus()) {
			float volume = float.Parse(configReader.Deserialize ("Game", "Volume"));
			AudioListener.volume = volume;
		}
	}
}
