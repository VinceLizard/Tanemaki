using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
// using InControl; // removed InControl input manager to post on GitHub

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [Tooltip("Ball of Seeds prefab goes here.")]
    public GameObject player;
    [Tooltip("Player start location.")]
    public Transform playerSpawnPoint;
    public GameObject[] trees;
    public GameObject[] flowers;
    [SerializeField] private GameObject gameOverText = null;
    [SerializeField] private Text seedsPlantedText = null;
    [SerializeField] private Text scoreText = null;
    [Tooltip("Put gameplay intro and loop audio clips here.")]
    [SerializeField] private AudioClip[] gameplayClips = null;
    [Tooltip("Put game over intro and loop audio clips here.")]
    [SerializeField] private AudioClip[] gameoverClips = null;
    private bool gameOver = false;
    private int score;
    private AudioSource audioSource;
    // PlayerActions playerActions; // used by InControl

    public int GetScore() { return score; }

    public void ScorePoint() 
    {
        score++;
        scoreText.text = "Seeds Planted: " + score;
    }

    private void Awake()
    {
		// singleton
		if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // playerActions = PlayerActions.CreateWithDefaultBindings();
        audioSource = GetComponent<AudioSource>();
        SpawnPlayer();

    }

    void SpawnPlayer()
    {
        if (BallOfSeeds.Current != null)
            GameObject.Destroy(BallOfSeeds.Current.gameObject);

        var go = GameObject.Instantiate(player, playerSpawnPoint.position, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
	{
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameOverText.SetActive(false);
        gameOver = false;
        StartCoroutine("PlayGameplayMusic");
    }

    // Update is called once per frame
    void Update()
    {

        if(gameOver)
        {
            if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            {
                SceneManager.LoadScene("Main");
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quitting");
            Cursor.visible = true;
            Application.Quit();
        }
    }

    public GameObject GetRandomTree()
    {
        int index = Random.Range(0, (trees.Length - 1));
        return trees[index];
    }

    public GameObject GetRandomFlower()
    {
        int index = Random.Range(0, (flowers.Length - 1));
        return flowers[index];
    }

    public void EndGame()
    {
        StopCoroutine("PlayGameplayMusic");
        StartCoroutine("PlayGameoverMusic");
        seedsPlantedText.text = "You planted " + score.ToString() + " seeds";
        gameOverText.SetActive(true);
        gameOver = true;
        Cursor.visible = true;
    }

	string[] spawnableLayers = new string[2] { "Island", "Water" };

	public bool GetGroundPosFromSky(Vector3 pos, out RaycastHit result)
	{
		pos.y = 500.0f;

		RaycastHit hitInfo;
		Ray ray = new Ray(
			pos,
			Vector3.down
		);
		if(
			Physics.Raycast(ray, out hitInfo, 1000.0f, LayerMask.GetMask(spawnableLayers)) && 
			hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Island")
		)
		{
			result = hitInfo;
			return true;
		}
		result = new RaycastHit();
		return false;
	}


    IEnumerator PlayGameplayMusic()
    {
        audioSource.loop = false;
        audioSource.clip = gameplayClips[0];
        audioSource.Play();
        yield return new WaitForSeconds(gameplayClips[0].length);
        audioSource.loop = true;
        audioSource.clip = gameplayClips[1];
        audioSource.Play();
    }

    IEnumerator PlayGameoverMusic()
    {
        audioSource.loop = false;
        audioSource.clip = gameoverClips[0];
        audioSource.Play();
        yield return new WaitForSeconds(gameoverClips[0].length);
        audioSource.loop = true;
        audioSource.clip = gameoverClips[1];
        audioSource.Play();
    }
}
