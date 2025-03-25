using UnityEngine;

public class DartMechanics : MonoBehaviour
{
    public float throwForce = 10f;
    private Rigidbody dartRb;
    private Collider dartCollider;
    private bool isThrown = false;
    private bool alreadyHit = false;

    public Vector3 rotationOffset = new Vector3(90, 90, 90);
    public Vector3 positionOffset = new Vector3(0, 0, -0.5f);

    public PickUpObjects pickUpObjects; // Reference to the pickup system

    void Start()
    {
        dartRb = GetComponent<Rigidbody>();
        dartCollider = GetComponent<Collider>();

        // Ensure collider is disabled initially if being held
        if (pickUpObjects != null && pickUpObjects.pickedObject == gameObject)
        {
            dartCollider.enabled = false;
        }
    }

    void Update()
    {
        // Enable collider when the dart is dropped
        if (pickUpObjects != null && pickUpObjects.pickedObject == null && !isThrown)
        {
            dartCollider.enabled = true;
        }
    }

    public void ThrowDart(Vector3 direction)
    {
        if (!isThrown)
        {
            dartCollider.enabled = true; // Enable collider before throwing
            isThrown = true;

            transform.SetParent(null);
            dartRb.isKinematic = false;
            dartRb.useGravity = true;
            dartRb.AddForce(direction * throwForce, ForceMode.VelocityChange);
            isThrown = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyHit) return; // Ignore extra hits

        if (other.CompareTag("Dartboard 10") || other.CompareTag("Dartboard 20") ||
            other.CompareTag("Dartboard 30") || other.CompareTag("Dartboard Center"))
        {
            alreadyHit = true;
            Debug.Log("Hit: " + other.gameObject.name);
            other.GetComponentInParent<DartboardMechanics>()?.CheckDartHit(other.tag);

            Invoke(nameof(ResetHit), 0.5f); // Reset after 0.5 seconds
        }
    }

    private void ResetHit()
    {
        alreadyHit = false;
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