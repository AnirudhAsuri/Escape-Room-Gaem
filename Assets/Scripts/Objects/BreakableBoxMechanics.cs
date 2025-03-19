using UnityEngine;

public class BreakableBoxMechanics : MonoBehaviour
{
    public GameObject objectInside; // Assign the key or any object inside the box

    private Collider[] objectColliders;
    private Rigidbody objectRigidbody;

    void Start()
    {
        if (objectInside != null)
        {
            objectColliders = objectInside.GetComponentsInChildren<Collider>(); // Get all colliders
            objectRigidbody = objectInside.GetComponent<Rigidbody>();

            foreach (Collider col in objectColliders)
            {
                col.enabled = false; // Disable all colliders
            }

            if (objectRigidbody != null) objectRigidbody.isKinematic = true; // Disable physics
        }
    }

    public void BreakBox()
    {
        if (objectInside != null)
        {
            foreach (Collider col in objectColliders)
            {
                col.enabled = true; // Re-enable all colliders
            }

            if (objectRigidbody != null) objectRigidbody.isKinematic = false; // Enable physics

            objectInside.transform.parent = null; // Unparent so it falls freely
        }

        Destroy(gameObject); // Destroy the box
    }
}