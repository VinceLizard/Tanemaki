using UnityEngine;
using UnityEngine.SceneManagement;
//using InControl;
using UnityStandardAssets.CrossPlatformInput;

public class MainMenu : MonoBehaviour
{
    //InControl Class
    //PlayerActions playerActions;
    GameObject loading;

    private void Awake()
    {
        loading = GameObject.Find("Loading");
        loading.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        //playerActions = PlayerActions.CreateWithDefaultBindings();
    }

    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            loading.SetActive(true);
            SceneManager.LoadScene("Main");
        }
    }
}
