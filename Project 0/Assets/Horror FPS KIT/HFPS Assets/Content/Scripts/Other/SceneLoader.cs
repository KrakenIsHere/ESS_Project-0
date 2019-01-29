using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public void LoadScene (string scene) 
	{
		SceneManager.LoadScene(scene);
	}

	public void QuitApplication ()
	{
		Application.Quit ();
	}
}
