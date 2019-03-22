using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(PoolItemListScriptableObject))]
public class PoolItemListScriptableObjectInspector : Editor
{
	PoolItemListScriptableObject myTarget;
	Vector2 scrollPos;

	public void OnEnable()
	{
		myTarget = (PoolItemListScriptableObject)target;

	}

	public override void OnInspectorGUI()
	{
		base.DrawDefaultInspector();

		//DisplayList();
	}

	void DisplayList()
	{
		EditorGUILayout.LabelField("List of Pool Items : " + myTarget.ItemList.Count);
		scrollPos =	EditorGUILayout.BeginScrollView(scrollPos);
		
		for(int i = 0; i < myTarget.ItemList.Count; i++)
		{
			DisplayItem(myTarget.ItemList[i]);
		}

		
		EditorGUILayout.EndScrollView();

	}

	void DisplayItem(PoolItemScriptableObject piso)
	{
		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.LabelField(piso.name);

		GUI.enabled = false;
		piso.item = EditorGUILayout.ObjectField("Item", piso.item, typeof(PoolItem), true) as PoolItem;
		piso.amount = EditorGUILayout.IntField("Amount", piso.amount);
		piso.initiallizeOnStart = EditorGUILayout.Toggle("Initiallize On Start", piso.initiallizeOnStart);
		GUI.enabled = true;
		EditorGUILayout.EndVertical();


	}
}