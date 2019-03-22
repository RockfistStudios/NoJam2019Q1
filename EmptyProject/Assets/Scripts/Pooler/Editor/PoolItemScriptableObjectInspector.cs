using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PoolItemScriptableObject))]
public class PoolItemScriptableObjectInspector : Editor
{
	public override void OnInspectorGUI()
	{
		PoolItemScriptableObject myTarget = (PoolItemScriptableObject)target;
		//GUI.enabled = false;
		base.DrawDefaultInspector();
		//GUI.enabled = true;
	}
}