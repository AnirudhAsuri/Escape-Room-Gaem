using UnityEngine;

public class BreakableBoxMechanics : MonoBehaviour
{
    [Header("Breakable Object Settings")]
    public GameObject crackedVersion; // Optional: Assign a cracked version for visual swap
    public GameObject objectInside; // The item that was inside the box
    public AudioClip breakSound; // Optional: Assign a breaking sound
    public ParticleSystem breakEffect; // Optional: Assign particle effect

    private Collider[] objectColliders;
    private Rigidbody objectRigidbody;
    private bool isBroken = false;

    void Start()
    {
        if (objectInside != null)
        {
            objectColliders = objectInside.GetComponentsInChildren<Collider>();
            objectRigidbody = objectInside.GetComponent<Rigidbody>();

            foreach (Collider col in objectColliders)
                col.enabled = false; // Disable all colliders

            if (objectRigidbody != null) objectRigidbody.isKinematic = true; // Disable physics
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isBroken) return;

        if (other.CompareTag("Hammer")) // Make sure hammer has the correct tag
        {
            BreakBox();
        }
    }

    public void BreakBox()
    {
        if (isBroken) return;
        isBroken = true;

        // Enable object inside
        if (objectInside != null)
        {
            foreach (Collider col in objectColliders)
                col.enabled = true; // Enable all colliders

            if (objectRigidbody != null) objectRigidbody.isKinematic = false; // Enable physics

            objectInside.transform.parent = null; // Unparent so it falls freely
        }

        // Play sound effect
        if (breakSound != null)
            AudioSource.PlayClipAtPoint(breakSound, transform.position);

        // Play break effect
        if (breakEffect != null)
            Instantiate(breakEffect, transform.position, Quaternion.identity);

        // Replace with cracked version if assigned
        if (crackedVersion != null)
        {
            Instantiate(crackedVersion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject); // If no cracked version, just destroy the box
        }
    }
}