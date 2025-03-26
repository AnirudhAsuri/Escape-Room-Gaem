using UnityEngine;

public class DialInteraction : MonoBehaviour
{
    private Camera mainCamera;
    public PickUpObjects pickUpObjects;
    private void Start()
    {
        mainCamera = Camera.main;
        pickUpObjects = FindObjectOfType<PickUpObjects>(); // Automatically finds it
    }

    private void Update()
    {
        // Check if the dial is currently being held
        if (pickUpObjects.pickedObject == transform)
        {
            // Left-click to attempt inserting the dial
            if (Input.GetMouseButtonDown(0))
            {
                TryInsertDial();
            }
        }
    }

    private void TryInsertDial()
    {
        RaycastHit hit;
        Vector3 origin = mainCamera.transform.position;
        Vector3 direction = mainCamera.transform.forward;

        if (Physics.Raycast(origin, direction, out hit, 3f)) // Adjust range if needed
        {
            MainDialBoxMechanics dialBox = hit.collider.GetComponentInParent<MainDialBoxMechanics>();
            if (dialBox != null)
            {
                Debug.Log("Moo");
                dialBox.InsertDial(); // Call the function on the box
                Destroy(gameObject);  // Remove the dial from the scene
                pickUpObjects.pickedObject = null; // Reset pickup system
            }
        }
    }
}
