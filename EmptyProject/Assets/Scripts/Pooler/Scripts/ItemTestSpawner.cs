using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTestSpawner : MonoBehaviour
{
	public PoolItemScriptableObject leftClick;
	public PoolItemScriptableObject rightClick;


	// Update is called once per frame
	void Update ()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Pool.Manager.SpawnItem(leftClick, this.transform.position, this.transform.rotation, this.transform.localScale, 4);
		}

		if(Input.GetMouseButtonDown(1))
		{
			Pool.Manager.SpawnItem(rightClick, this.transform.position, this.transform.rotation, this.transform.localScale, 4f);
		}
	}
}
