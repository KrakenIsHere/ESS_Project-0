using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Keypad : MonoBehaviour {

	[Header("Setup")]
	public int AccessCode;
	public TextMesh AccessCodeText;
	private MeshRenderer textRenderer;

	[Header("Sounds")]
	public AudioClip enterCode;
	public AudioClip accessGranted;
	public AudioClip accessDenied;

	[Space(15)]
	public UnityEvent OnAccessGranted;
	[Space(7)]
	public UnityEvent OnAccessDenied;

	private string numberInsert = "";
	private bool confirmCode;
	private bool enableInsert = true;

	void Start()
	{
		textRenderer = AccessCodeText.gameObject.GetComponent<MeshRenderer> ();
	}

	public void InsertCode(int number)
	{
		if (!(numberInsert.Length >= AccessCode.ToString ().Length) && enableInsert && number != 10 && number != 11) {
			numberInsert = numberInsert + number;
			if(enterCode){AudioSource.PlayClipAtPoint(enterCode, Camera.main.transform.position);}
		}
		switch (number) {
		case 10:
			// Back
			if (numberInsert.Length > 0) {
				numberInsert = numberInsert.Remove (numberInsert.Length - 1);
				if(enterCode){AudioSource.PlayClipAtPoint(enterCode, Camera.main.transform.position);}
			}
			break;
		case 11:
			// Enter Code
			confirmCode = true;
			break;
		}
	}

	void Update () {
		if (enableInsert) {
			textRenderer.material.SetColor ("_Color", Color.white);
			AccessCodeText.text = numberInsert;
		}

		if (numberInsert == AccessCode.ToString () && confirmCode) {
			OnAccessGranted.Invoke ();
			confirmCode = false;
			enableInsert = false;
			numberInsert = "";
			StartCoroutine (WaitGranted ());
		} else if(confirmCode) {
			OnAccessDenied.Invoke ();
			confirmCode = false;
			enableInsert = false;
			numberInsert = "";
			StartCoroutine (WaitDenied ());
		}
	}

	IEnumerator WaitGranted()
	{
		if(accessGranted){AudioSource.PlayClipAtPoint(accessGranted, Camera.main.transform.position);}
		textRenderer.material.SetColor ("_Color", Color.green);
		AccessCodeText.text = "Granted";
		yield return new WaitForSeconds (1);
		enableInsert = true;
	}

	IEnumerator WaitDenied()
	{
		if(accessDenied){AudioSource.PlayClipAtPoint(accessDenied, Camera.main.transform.position);}
		textRenderer.material.SetColor ("_Color", Color.red);
		AccessCodeText.text = "Denied";
		yield return new WaitForSeconds (1);
		enableInsert = true;
	}
}
