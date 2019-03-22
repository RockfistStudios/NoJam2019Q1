using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
	public static Pool Manager;
	public PoolItemListScriptableObject poolList;

	public Dictionary<PoolItemScriptableObject, List<PoolItem>> poolItems = new Dictionary<PoolItemScriptableObject, List<PoolItem>>();

	void Awake()
	{
		Manager = this;

        StartCoroutine(SpawnObjects());
	}

    IEnumerator SpawnObjects()
    {
        //Gross, remove this
        GameManager.Instance.ActionsToLoad += poolList.ItemList.Count;
        
        foreach(PoolItemScriptableObject item in poolList.ItemList)
        {
            poolItems.Add(item, new List<PoolItem>());

            if(item.initiallizeOnStart)
            {
                for(int i = 0; i < item.amount; i++)
                {
                    InstantiateObject(item);
                }
            }

            GameManager.Instance.ActionsCompleted++;
            GameManager.Instance.UpdateProgressBar();
            yield return new WaitForEndOfFrame();
        }

        GameManager.Instance.StartPopulatingWorld();
    }

	PoolItem InstantiateObject(PoolItemScriptableObject _item)
	{
		PoolItem pi = Instantiate(_item.item);
		
		pi.transform.SetParent(this.transform);
		pi.RePool();

		poolItems[_item].Add(pi);
		return pi;
	}

	PoolItem TempPoolItem;
	public PoolItem SpawnItem(PoolItemScriptableObject _item, Vector3 _position, Quaternion _rotation, Vector3 _scale, float timeToRepool = 0)
	{
		TempPoolItem = null;

		for(int i = 0; i < poolItems[_item].Count; i++)
		{
			if(poolItems[_item][i].IsActive == false)
			{
				TempPoolItem = poolItems[_item][i];
			}
		}

		if(TempPoolItem == null)
		{
			TempPoolItem = InstantiateObject(_item);
		}

		TempPoolItem.Spawn(_position, _rotation, _scale);

		if(timeToRepool != 0)
		{
			TempPoolItem.RePool(timeToRepool);
		}
		return TempPoolItem;
	}
}