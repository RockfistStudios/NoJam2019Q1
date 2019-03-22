using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pool_Item_", menuName = "Pooler/Pool Item", order = 1)]
public class PoolItemScriptableObject : ScriptableObject
{
	public PoolItem item;
	public int amount = 10;
	public bool initiallizeOnStart = true;
}