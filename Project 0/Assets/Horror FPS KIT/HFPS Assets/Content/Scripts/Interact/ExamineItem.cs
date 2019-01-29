using System;
using UnityEngine;

public class ExamineItem : MonoBehaviour {

	public float examineDistance;
	public AudioClip examineSound;

	[Header("Examine Object")]
	public bool isUsable;
    public string examineObjectName;

	[Header("Examine Paper")]

	[Multiline]
	public string PaperReadTexts;
	public Vector3 paperRotation;
}
