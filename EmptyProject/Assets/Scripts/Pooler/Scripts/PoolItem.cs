using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItem : MonoBehaviour
{
	bool _isActive;
	public bool IsActive;
    bool waitingToBeRePooled = false;
	float timeToWait;
	float timeWaited;

	void Update()
	{
		if(waitingToBeRePooled)
		{
			if(timeWaited>= timeToWait)
			{
				RePool();
			}
            else
            {
                timeWaited += Time.deltaTime;
            }
		}
	}

	public void Spawn(Vector3 position, Quaternion rotation, Vector3 scale)
	{
		IsActive = true;
		transform.position = position;
		transform.localScale = scale;
		transform.rotation = rotation;

		gameObject.SetActive(true);
	}

	public void RePool()
	{
        transform.SetParent(Pool.Manager.transform);
        gameObject.SetActive(false);
		Reset();
	}

	public void RePool(float waitTime)
	{
		timeToWait = waitTime;
		waitingToBeRePooled = true;
	}

	void Reset()
	{
		IsActive = false;
		transform.position = Vector3.zero;
		transform.localScale = Vector3.one;
		transform.rotation = Quaternion.identity;
		
		waitingToBeRePooled = false;
		timeWaited = 0;
		timeToWait = 0;
	}
}