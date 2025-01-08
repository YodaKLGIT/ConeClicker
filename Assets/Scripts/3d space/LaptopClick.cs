using UnityEngine;
using UnityEngine.SceneManagement;

public class LaptopClick : MonoBehaviour
{
    // Name of the scene to load
    public string sceneToLoad = "2DConeClicker";  // Replace with your actual scene name

    // This function is called when the object is clicked
    private void OnMouseDown()
    {
        // Make the cursor visible and unlock it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Load the scene when the object is clicked
        SceneManager.LoadScene(sceneToLoad);
    }
}
