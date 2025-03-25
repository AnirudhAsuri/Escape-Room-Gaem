using UnityEngine;

public class PendulumMechanics : MonoBehaviour
{
    public float raycastRange = 3f; // Adjust range as needed
    public LayerMask clockLayer; // Set this in the inspector to only hit clocks
    public PickUpObjects pickUpObjects; // Reference to the pickup system

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastRange, clockLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                GrandfatherClockMechanics clock = hit.collider.GetComponentInParent<GrandfatherClockMechanics>();

                if (clock != null)
                {
                    Debug.Log("Clock found");
                    if (pickUpObjects.pickedObject.name == "Pendulum")
                    {
                        clock.SwitchClock();
                        Destroy(pickUpObjects.pickedObject.gameObject); // Remove the pendulum
                        pickUpObjects.pickedObject = null; // Clear the reference
                    }
                }
            }
        }
    }
}