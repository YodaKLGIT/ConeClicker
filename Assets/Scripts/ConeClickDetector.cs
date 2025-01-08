using UnityEngine;

public class ConeClickDetector : MonoBehaviour
{
    [SerializeField] private Camera ConeCamera; // Reference to the specific camera that can see the cone

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            Ray ray = ConeCamera.ScreenPointToRay(Input.mousePosition); // Use the custom camera for the raycast
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Access the cone object through the public method
                if (hit.collider.gameObject == ConeManager.instance.GetConeObj())
                {
                    ConeManager.instance.OnConeClicked(); // Call the click method from GameManager
                }
            }
        }
    }
}
