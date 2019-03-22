using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PoolItemCreatorWindow : EditorWindow
{
	PoolItemListScriptableObject Pilso;
	PoolItemScriptableObject selectedPiso;

	Vector2 scrollPos;

	[MenuItem("Pool/Pool Editor")]
	public static void ShowWindow()
	{
		PoolItemCreatorWindow window = (PoolItemCreatorWindow)EditorWindow.GetWindow(typeof(PoolItemCreatorWindow), false, "Pool Editor");
	}

	void OnGUI()
	{
		if(PilsoField())
		{
			return;
		}

		EditorGUILayout.BeginHorizontal();

		DisplayPoolItems();
		DisplaySelectedPoolItem();

		EditorGUILayout.EndHorizontal();

		SaveEverything();
	}

	void SaveEverything()
	{
		if(Pilso!=null)
		{
			EditorUtility.SetDirty(Pilso);

			for(int i = 0; i < Pilso.ItemList.Count; i++)
			{
				EditorUtility.SetDirty(Pilso.ItemList[i]);
			}
		}
	}

	void DisplayPoolItems()
	{
		EditorGUILayout.BeginVertical("Box", GUILayout.Width(200));
		if(GUILayout.Button("Create New Item"))
		{
			CreatePoolItem();
			Pilso.ItemList.Add(selectedPiso);
		}

		//scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		
		if(Pilso.ItemList.Count>0)
		{
			for(int i = 0; i < Pilso.ItemList.Count; i++)
			{
				if(Pilso.ItemList[i] == selectedPiso)
				{
					GUI.enabled = false;
				}

				if(GUILayout.Button(Pilso.ItemList[i].name))
				{
					selectedPiso = Pilso.ItemList[i];
				}

				GUI.enabled = true;
			}
		}


		//EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}

	void DisplaySelectedPoolItem()
	{
		if(selectedPiso==null)
		{
			return;
		}

		EditorGUILayout.BeginVertical("Box");
		selectedPiso.name = EditorGUILayout.TextField("Name: ", selectedPiso.name);
		
		AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(selectedPiso.GetInstanceID()), selectedPiso.name);

		selectedPiso.item = EditorGUILayout.ObjectField("Item: ",selectedPiso.item, typeof(PoolItem), false) as PoolItem;
		selectedPiso.amount = EditorGUILayout.IntField("Amount:", selectedPiso.amount);
		selectedPiso.initiallizeOnStart = EditorGUILayout.Toggle("Initiallize On Start: ", selectedPiso.initiallizeOnStart);

		GUI.color = Color.red;

		if(GUILayout.Button("Delete") && EditorUtility.DisplayDialog("Warning!",
				"This will perminantly delete " + selectedPiso.name + "!", "Delete", "Cancel"))
		{
			Pilso.ItemList.Remove(selectedPiso);
			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedPiso.GetInstanceID()));
		}

		GUI.color = Color.white;

		EditorGUILayout.EndVertical();
	}




	bool PilsoField()
	{
		EditorGUILayout.LabelField("Pool Item List Scriptable Object");

		EditorGUILayout.BeginHorizontal();
		Pilso = EditorGUILayout.ObjectField(Pilso, typeof(PoolItemListScriptableObject), false) as PoolItemListScriptableObject;
		if(GUILayout.Button("Create New Item List") && EditorUtility.DisplayDialog("Warning!",
				"This will replace any pool list of the same name", "Create", "Do Not Create"))
		{
			CreatePoolItemList();
		}
		EditorGUILayout.EndHorizontal();

		if(Pilso ==null)
		{
			EditorGUILayout.HelpBox("Please assign Pool Item List", MessageType.Error);
		}

		return Pilso == null;
	}

	public void CreatePoolItemList()
	{
		PoolItemListScriptableObject asset = ScriptableObject.CreateInstance("PoolItemListScriptableObject") as PoolItemListScriptableObject;
		AssetDatabase.CreateAsset(asset, "Assets/PoolItemListScriptableObject.asset");
		AssetDatabase.SaveAssets();
		Pilso = asset;
	}

	public void CreatePoolItem()
	{
		PoolItemScriptableObject asset = ScriptableObject.CreateInstance("PoolItemScriptableObject") as PoolItemScriptableObject;
		AssetDatabase.CreateAsset(asset, "Assets/New_Pool_Item.asset");
		AssetDatabase.SaveAssets();
		selectedPiso = asset;
	}
}