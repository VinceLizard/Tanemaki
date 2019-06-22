using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private Transform[] seedSpawnPoints = null;
    [SerializeField] private GameObject seed = null;
    [SerializeField] private float lifeTime = 45f;
 
    private Animator anim;
    private float startTime;
    private List<GameObject> spawnedSeeds;

    [HideInInspector] public float ssdMin;
    [HideInInspector] public float ssdMax;
    [HideInInspector] public float easMin;
    [HideInInspector] public float easMax;

	// Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        spawnedSeeds = new List<GameObject>();
        int numberOfSeeds = Random.Range(1, 6);
        float spawnSeedsDelay = Random.Range(ssdMin, ssdMax);
        float ejectAfterSpawning = Random.Range(easMin, easMax);
        StartCoroutine(SpawnSeeds(spawnSeedsDelay, numberOfSeeds));
        StartCoroutine(Shrink(spawnSeedsDelay + ejectAfterSpawning));
    }

    IEnumerator SpawnSeeds(float delay, int numberOfSeeds)
    {
        //shuffle the spawnPoints array
        int n = seedSpawnPoints.Length;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Transform value = seedSpawnPoints[k];
            seedSpawnPoints[k] = seedSpawnPoints[n];
            seedSpawnPoints[n] = value;
        }

        yield return new WaitForSeconds(delay);

        //spawn seeds at the first numberOfSeeds indexes of the now shuffled array
        for (int i = 0; i < numberOfSeeds; i++)
        {
            Vector3 spawnLocation = seedSpawnPoints[i].position;
            GameObject seedling = Instantiate(seed, spawnLocation, Quaternion.identity);
            spawnedSeeds.Add(seedling);
        }

    }

    IEnumerator Shrink(float delay)
    {
        yield return new WaitForSeconds(delay);

        //eject the seeds!
        foreach(GameObject seedling in spawnedSeeds)
        {
            seedling.GetComponent<Seed>().Eject();
        }

        yield return new WaitForSeconds(lifeTime);
        anim.Play("shrink");

        yield return new WaitForSeconds(.5f);

        Destroy(gameObject);
    }
}
