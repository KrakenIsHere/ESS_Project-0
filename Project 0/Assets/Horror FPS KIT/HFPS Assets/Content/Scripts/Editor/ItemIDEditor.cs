using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemID)), CanEditMultipleObjects]
public class ItemIDEditor : Editor {

	public SerializedProperty
	ItemType_Prop,
	WeaponType_Prop,
	Amount_Prop,
	WeaponID_Prop,
	InventoryID_Prop,
	BackpackExpand_Prop,
	Destroy_Prop;

	void OnEnable () {
		ItemType_Prop = serializedObject.FindProperty ("ItemType");
		WeaponType_Prop = serializedObject.FindProperty ("weaponType");
		Amount_Prop = serializedObject.FindProperty("Amount");
		WeaponID_Prop = serializedObject.FindProperty ("WeaponID");
		InventoryID_Prop = serializedObject.FindProperty ("InventoryID");
		BackpackExpand_Prop = serializedObject.FindProperty ("BackpackExpand");    
		Destroy_Prop = serializedObject.FindProperty ("DestroyOnPickup");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update ();

		EditorGUILayout.PropertyField( ItemType_Prop );
		ItemID.Type type = (ItemID.Type)ItemType_Prop.enumValueIndex;
		EditorGUILayout.PropertyField( Destroy_Prop, new GUIContent("Destroy") );

		switch( type ) {
		case ItemID.Type.NoInventoryItem:
			EditorGUILayout.PropertyField (WeaponID_Prop, new GUIContent ("SwitcherID:"));
			break;

		case ItemID.Type.InventoryItem:            
			EditorGUILayout.PropertyField( Amount_Prop, new GUIContent("Amount:") );
			EditorGUILayout.PropertyField( InventoryID_Prop, new GUIContent("InventoryID:") );
			break;

		case ItemID.Type.WeaponItem:            
			EditorGUILayout.PropertyField (WeaponType_Prop);
			ItemID.WeaponType weapType = (ItemID.WeaponType)ItemType_Prop.enumValueIndex;

			EditorGUILayout.PropertyField (Amount_Prop, new GUIContent ("Amount:"));
			EditorGUILayout.PropertyField (WeaponID_Prop, new GUIContent ("WeaponID:"));
			EditorGUILayout.PropertyField (InventoryID_Prop, new GUIContent ("InventoryID:"));
			break;

		case ItemID.Type.BackpackExpand:            
			EditorGUILayout.PropertyField( BackpackExpand_Prop, new GUIContent("Expand Amount:") );
			break;
		}

		serializedObject.ApplyModifiedProperties ();
	}
}
