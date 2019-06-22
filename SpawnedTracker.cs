using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;

public class SpawnedTracker : MonoBehaviour
{
	public Spawner ParentSpawner { get; set; }


	private void OnDestroy()
	{
		if(ParentSpawner != null)
			ParentSpawner.OnSpawnedDestroyed();
	}

	public void SetParentSpawner(Spawner spawner)
	{
		ParentSpawner = spawner;
	}
}
