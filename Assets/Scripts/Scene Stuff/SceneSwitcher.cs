using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class SceneSwitcher : MonoBehaviour
{
    // The name of the scene to load
    [SerializeField] private string sceneName;

    // Reference to the ConeManager
    private ConeManager coneManager;

    private void Start()
    {
        // Get the ConeManager instance
        coneManager = ConeManager.instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (coneManager != null)
            {
                coneManager.SaveGame();
            }

            SceneManager.LoadScene(sceneName);
        }
    }

}
