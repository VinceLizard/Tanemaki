using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Seed : MonoBehaviour
{

    Rigidbody rb;
	CharacterController controller;

    [SerializeField] int seedHealth = 10;
	[SerializeField] float escapeSpeed = default;
	[SerializeField] RangeFloat distToWalk;
	[SerializeField] float destroyTimeAfterEscape = 5.0f;
	[SerializeField] GameObject bobRoot = null;
	[SerializeField] GameObject[] facialFeatures = null;
	[SerializeField] float bobSpeed = 5f;
    [SerializeField] AudioClip[] seedDropping = null;
    [SerializeField] AudioClip[] seedRunning = null;

	public int SeedHealth { get { return seedHealth; } }
    private AudioSource audioSource;

    float X;
    float Y;
    float Z;

    // Start is called before the first frame update
    void Awake()
    {
		controller = GetComponent<CharacterController>();
		controller.enabled = false;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        X = Random.Range(-1, 1);
        Y = Random.Range(2, 4);
        Z = Random.Range(-1, 1);
		
		foreach (var f in facialFeatures)
			f.SetActive(false);

        audioSource = GetComponent<AudioSource>();
	}

    // Update is called once per frame
    public void Eject()
    {
        PlayRandomClip(seedDropping);
        rb.isKinematic = false;
        rb.AddForce(X, Y, Z, ForceMode.Impulse);
        StartCoroutine("CrazyWalk");
    }


    void PlayRandomClip(AudioClip[] audioclipArray)
    {
        int clip = Random.Range(0, audioclipArray.Length);
        audioSource.clip = audioclipArray[clip];
        audioSource.Play();
    }

    static Vector3 GetRandomDir()
	{
		var dir2d = Random.insideUnitCircle.normalized;
		return new Vector3(dir2d.x, 0, dir2d.y);
	}

	private IEnumerator CrazyWalk()
	{
		float timeSpent = 0.0f;

		bool comeToLife = false;
		while (!comeToLife)
		{
			timeSpent += Time.deltaTime;
			if (rb.IsSleeping() || (rb.velocity.magnitude < 0.1f && timeSpent > 5.0f))
			{
				rb.isKinematic = true;
				controller.enabled = true;

				this.transform.forward = GetRandomDir();
				RaycastHit hitInfo;
				if (GameManager.instance.GetGroundPosFromSky(this.transform.position, out hitInfo))
				{
					this.transform.position = hitInfo.point;
				}
				foreach (var f in facialFeatures)
					f.SetActive(true);
				comeToLife = true;
			}
			yield return null;
		}

		yield return new WaitForSeconds(3f);
		this.transform.forward = GetRandomDir();
        PlayRandomClip(seedRunning);
        runInDirection = true;

		yield return new WaitForSeconds(destroyTimeAfterEscape);
		GameObject.Destroy(this.gameObject);
	}

	bool runInDirection = false;

	private void Update()
	{
		if (runInDirection)
		{
			if (bobRoot)
			{
				bobRoot.transform.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(-8,8, Mathf.PingPong(bobSpeed * Time.time, 1)));
			}
		}
	}

	void FixedUpdate()
	{
		if (runInDirection)
		{
			controller.SimpleMove(this.transform.forward * escapeSpeed);
		}


	}
}
