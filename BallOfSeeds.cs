using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Animator))]
public class BallOfSeeds : MonoBehaviour
{
    public static BallOfSeeds Current { get; private set; }

    [SerializeField] private float life = 50;
    [SerializeField] private float lifeLossRate = 1;
    [Range(0, 1.0f)]
    [Tooltip("Percentile of starting life at which player dies, should be tiny number.")]
    [SerializeField] private float dieAtPercentile = .2f;
    [Tooltip("Use to change the rate at which player dies as seedball gets smaller.")]
    public AnimationCurve deathCurve;

    private Rigidbody rigidBody;
    private Transform trans;
    private AudioSource[] audioSources;
    Animator seedBallAnimator;
    SphereCollider sphereCollider;

    // accessors for camera to access to determine if it needs to move back
    public float Life { get { return life; }}
    public float StartLife { get; private set; }

    void Awake()
	{
		Current = this;
		this.rigidBody = GetComponent<Rigidbody>();
        this.trans = GetComponent<Transform>();
        this.audioSources = GetComponents<AudioSource>(); // plural components is correct, do not change
        this.sphereCollider = GetComponent<SphereCollider>();

        StartLife = life;

        seedBallAnimator = GetComponentInChildren<Animator>();
    }

	private void OnDestroy()
	{
		if(Current == this)
			Current = null;
	}

    public void Update()
    {
        DieALittle();

        if(life <= dieAtPercentile * StartLife)
        {
            GameManager.instance.EndGame();
            BallDeath();
        }

        if (rigidBody.velocity.magnitude > .2f)
        {
            if (!audioSources[1].isPlaying)
            {
                audioSources[1].Play();
            }
        }
        else
        {
            audioSources[1].Stop();
        }
    }

    void FixedUpdate()
	{
        // FYI - movement is handled using StandardAsset Ball & User Ball Control scripts

        // die if ball goes under water
		if(this.transform.position.y < -1.5f)
		{
            GameManager.instance.EndGame();
            BallDeath();
        }
	}

	public static void Respawn()
	{
		if(Current != null)
			GameObject.Destroy(Current.gameObject);

		var go = GameObject.Instantiate(Resources.Load("BallOfSeeds") as GameObject, Vector3.up * 1.0f, Quaternion.identity);
	}

    private void OnTriggerEnter(Collider other)
    {
        // plant a tree when you are near a Planter object
        if(other.tag == "Planter")
        {
            Debug.Log("Planter trigger entered");
            GameObject.Instantiate(GameManager.instance.GetRandomTree(), other.transform.position, other.transform.rotation);
            GameManager.instance.ScorePoint();
            Destroy(other.gameObject);
        }

        // plant a flower when you are near a FlowerPlanter object
        if (other.tag == "FlowerPlanter")
        {
            Debug.Log("FlowerPlanter trigger entered");
            GameObject.Instantiate(GameManager.instance.GetRandomFlower(), other.transform.position, other.transform.rotation);
            GameManager.instance.ScorePoint();
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    { 
        // when you collid with a seed, increase your health and destroy seed
        if(collision.gameObject.tag == "Seed")
        {   if (life < StartLife)
            {
                life += collision.gameObject.GetComponent<Seed>().SeedHealth * (StartLife * lifeLossRate);
            }
            audioSources[0].Play();
            Destroy(collision.gameObject);
        }    
    }
    // this shrinks and grows the ball of seeds by moving the point in the shrinking ball animation 
    // relative to current life
    void DieALittle()
    {   
        life -= Time.deltaTime * (StartLife * lifeLossRate);
        float currentScale = life / StartLife;
        sphereCollider.radius = Mathf.Max(currentScale, .4f);
        seedBallAnimator.Play("Take 001", 0, deathCurve.Evaluate(1 - currentScale));
    }

    void BallDeath()
    {
        Destroy(gameObject);
    }
}
