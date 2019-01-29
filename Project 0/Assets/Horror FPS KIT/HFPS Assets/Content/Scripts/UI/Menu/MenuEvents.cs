using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEvents : MonoBehaviour {

	public void ChangeTextColor(string color)
	{
		Color col = Color.clear;
		ColorUtility.TryParseHtmlString(color, out col);
		GetComponent<Text> ().color = col;
	}

	public void ChangeImageColor(string color)
	{
		Color col = Color.clear;
		ColorUtility.TryParseHtmlString(color, out col);
		GetComponent<Image> ().color = col;
	}
}
