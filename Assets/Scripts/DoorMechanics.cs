using UnityEngine;

public class DoorMechanics : MonoBehaviour
{
    public Animator doorAnimator; // Assign in Inspector
    public PickUpObjects pickUpScript; // Assign your pickup system script
    public string requiredObjectName = "Key"; // Name of the required object

    private bool isOpen = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Assuming left-click to interact
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickUpScript.pickUpRange))
            {
                if (hit.collider.gameObject.name == "Door")
                {
                    TryOpenDoor();
                }
            }
        }
    }

    void TryOpenDoor()
    {
        if (isOpen) return; // Prevent multiple openings

        if (pickUpScript.pickedObject != null &&
            pickUpScript.pickedObject.name == requiredObjectName)
        {
            doorAnimator.Play("Door Open"); // Directly play the animation
            isOpen = true;
        }
    }
}
