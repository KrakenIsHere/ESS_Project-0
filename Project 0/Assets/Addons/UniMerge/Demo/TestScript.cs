using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
// ReSharper disable UnusedMember.Global
// ReSharper disable NotAccessedField.Global

namespace UniMerge.Demo {
	public class TestScript : MonoBehaviour {
		[Serializable]
		public class Test {
			public Vector3 vec3;
			public Quaternion quat;
			public Test nested;
		}

		public GameObject ref1, ref2, ref3;
		public Collider col1;
		public MonoBehaviour mb;
		public Rigidbody rigid;
		public Bounds bounds;
		public Quaternion quat;
		public Rect rect;
		public Vector2 vec2;
		public Vector3 vec3;
		public Vector4 vec4;
		public string str;
		public int i;
		public bool b;
		public float f;
		public Color c;
		public LayerMask l;
#if UNITY_EDITOR
		public SerializedPropertyType type;
#endif
		public Transform[] arr;
		public char ch;
		public AnimationCurve anim;
#if UNITY_4 || UNITY_5 || UNITY_5_3_OR_NEWER
		public Gradient g;
#endif

		public Test test;
	}
}
