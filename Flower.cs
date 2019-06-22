using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField] private Transform[] seedSpawnPoints = null;
    [SerializeField] private GameObject seed = null;
    [SerializeField] private GameObject flower;
    [SerializeField] private Transform flowerPoint = null;
    [SerializeField] private Color[] flowerColors = null;
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
        StartCoroutine(SpawnFlower());
    }

    IEnumerator SpawnFlower() {
        yield return new WaitForSeconds(.9f);
        GameObject spawnedFlower = Instantiate(flower, flowerPoint.position, Quaternion.identity);
        spawnedFlower.transform.parent = gameObject.transform;
        flower = spawnedFlower;
        Renderer flowerRend = spawnedFlower.GetComponent<Renderer>();
        flowerRend.material.SetFloat("_Seed", Random.Range(.1f, 10f));
        flowerRend.material.SetColor("_Color", flowerColors[Random.Range(0, 3)]);
        float n = 0;
        while (n < .82f) {
            n += .1f;
            flowerRend.material.SetFloat("_OverallSize", n);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator SpawnSeeds(float delay, int numberOfSeeds)
    {
        //shuffle the spawnPoints array
        //int n = seedSpawnPoints.Length;
        //while (n >= 1)
        //{
        //    n--;
        //    int k = Random.Range(0, n + 1);
        //    Transform value = seedSpawnPoints[k];
        //    seedSpawnPoints[k] = seedSpawnPoints[n];
        //    seedSpawnPoints[n] = value;
        //}

        yield return new WaitForSeconds(delay);

        //spawn seeds at the first numberOfSeeds indexes of the now shuffled array
        if(numberOfSeeds >= 5)
        {
            Vector3 spawnLocation = seedSpawnPoints[0].position;
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
        flower.SetActive(false);
        anim.Play("shrink");

        yield return new WaitForSeconds(.5f);

        Destroy(gameObject);
    }
}
