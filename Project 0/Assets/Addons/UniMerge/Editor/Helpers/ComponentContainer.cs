﻿//Matt Schoen
//1-29-2018
//
// This software is the copyrighted material of its author, Matt Schoen, and his company Defective Studios.
// It is available for sale on the Unity Asset store and is subject to their restrictions and limitations, as well as
// the following: You shall not reproduce or re-distribute this software without the express written (e-mail is fine)
// permission of the author. If permission is granted, the code (this file and related files) must bear this license
// in its entirety. Anyone who purchases the script is welcome to modify and re-use the code at their personal risk
// and under the condition that it not be included in any distribution builds. The software is provided as-is without
// warranty and the author bears no responsibility for damages or losses caused by the software.
// This Agreement becomes effective from the day you have installed, copied, accessed, downloaded and/or otherwise used
// the software.

//UniMerge 1.10
//ComponentContainer class

using UnityEditor;
using UnityEngine;

namespace UniMerge.Editor.Helpers {
	class ComponentContainer : ScriptableObject {
		[SerializeField]
		Component _mine;

		[SerializeField]
		Component _theirs;

		public Component mine { get { return _mine; } }
		public Component theirs { get { return _theirs; } }

		public static ComponentContainer Create(out SerializedProperty mineProp, out SerializedProperty theirsProp) {
			var componentContainer = CreateInstance<ComponentContainer>();
			var serializedObject = new SerializedObject(componentContainer);
			mineProp = serializedObject.FindProperty("_mine");
			theirsProp = mineProp.Copy();
			theirsProp.Next(false);
			return componentContainer;
		}
	}
}
