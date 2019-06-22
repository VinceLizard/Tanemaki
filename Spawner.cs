using System.Collections;
using UnityEngine;


public class Spawner : MonoBehaviour
{
	[SerializeField] private float initialDelay = 1.0f;
	[SerializeField] private RangeFloat destroyTime; 
	[SerializeField] private GameObject[] prefabsToSpawn = null;
	[SerializeField] public Color color = Color.white;
	[SerializeField] private int maxSpawns = 10;
	[SerializeField] private AnimationCurve intervalCurve;
	[SerializeField] private RangeFloat intervalRange;

	int currentNumberSpawned = 0;

	const int MAX_SPAWN_TRIES = 20;

	SpawnerLocation[] spawners;

	int spawnableMask;
	int islandLayer;

	private void Awake()
	{
		spawners = this.GetComponentsInChildren<SpawnerLocation>();
		spawnableMask = LayerMask.GetMask(new string[1] { "Island" });
		islandLayer = LayerMask.NameToLayer("Island");
	}

    void Start()
    {
        StartCoroutine("Spawn");
    }

	IEnumerator Spawn()
	{
		yield return new WaitForSeconds(initialDelay);

		if (spawners.Length == 0 || prefabsToSpawn.Length <= 0)
		{
			Debug.LogError("A spawner needs SpawnerLocations added to it, Planter prefabs set and a positive interval!");
		}
		else
		{
			while (true)
			{
				if (currentNumberSpawned >= maxSpawns)
				{
					currentNumberSpawned = 0;
					yield return new WaitForSeconds(Random.Range(90, 300));
				}
				else
				{
					var spawner = this.spawners[Random.Range(0, this.spawners.Length)];
					var spawnLocation = spawner.transform.position;
					int numTries = 0;
					bool found = false;

					while (numTries < MAX_SPAWN_TRIES && !found)
					{
						var randCircle = Random.insideUnitCircle * spawner.Radius;
						Vector3 rayStart = spawnLocation + new Vector3(randCircle.x, 0, randCircle.y);
						rayStart.y = 500.0f;

						RaycastHit hitInfo;
						Ray ray = new Ray(
							rayStart,
							Vector3.down
						);
						if (Physics.Raycast(ray, out hitInfo, 1000.0f) && hitInfo.collider.gameObject.layer == islandLayer)
						{
							var prefab = prefabsToSpawn[Random.Range(0, this.prefabsToSpawn.Length)];
							var go = GameObject.Instantiate(prefab, hitInfo.point + new Vector3(0.0f, 0.01f, 0.0f), Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
							go.AddComponent<SpawnedTracker>().SetParentSpawner(this);
							currentNumberSpawned++;
							found = true;
						}
						else
						{
							numTries++;
						}
					}

					if (!found)
					{
						Debug.LogWarning("Trouble spawning at: " + spawnLocation.ToString() + " tries: " + numTries.ToString());
					}

					float interval = Mathf.Max(0.01f, intervalRange.Lerp(currentNumberSpawned / (float)maxSpawns));
					yield return new WaitForSeconds(interval);

				}
			}
		}
	}

	public void OnSpawnedDestroyed()
	{
		// currentNumberSpawned--; //commented out -Nathan
	}

}
