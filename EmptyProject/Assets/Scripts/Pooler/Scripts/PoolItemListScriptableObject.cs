using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pool_Item_List", menuName = "Pooler/Pool List", order = 1)]
public class PoolItemListScriptableObject : ScriptableObject
{
	//[HideInInspector]
	public List<PoolItemScriptableObject> ItemList = new List<PoolItemScriptableObject>();
}