using UnityEngine;

public class HammerMechanics : MonoBehaviour
{
    public Vector3 rotationOffset = new Vector3(90, 90, 90); // Adjust these values as needed
    public Vector3 positionOffset = new Vector3(0, 0, -0.5f); // Adjust if needed

    public float raycastDistance = 3f; // Adjust based on interaction range

    private PickUpObjects pickUpScript;
    private Camera playerCamera; // Store reference to the main camera

    void Start()
    {
        pickUpScript = FindObjectOfType<PickUpObjects>(); // Find pickup system
        playerCamera = Camera.main; // Get the main camera
    }

    void Update()
    {
        HammerHit();
    }

    private void HammerHit()
    {
        if (pickUpScript.pickedObject == gameObject.transform)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("Breakable") && Input.GetMouseButtonDown(0)) // If looking at a breakable box and left-click is pressed
                {
                    BreakableBoxMechanics breakable = hit.collider.GetComponent<BreakableBoxMechanics>();
                    if (breakable != null)
                    {
                        breakable.BreakBox();
                    }
                }
            }
        }
    }
 
    public Quaternion GetRotationOffset()
    {
        return Quaternion.Euler(rotationOffset);
    }

    public Vector3 GetPositionOffset()
    {
        return positionOffset;
    }
}
