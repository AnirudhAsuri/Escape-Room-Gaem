using UnityEngine;

public class ForceDartMechanics : MonoBehaviour
{
    public float throwForce = 10f;
    private Rigidbody dartRb;
    private Collider dartCollider;
    private bool isThrown = false;
    private bool alreadyHit = false;

    public Vector3 rotationOffset = new Vector3(0, 0, 90);
    public Vector3 positionOffset = new Vector3(-1, 0, 0);

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

    private void OnCollisionEnter(Collision collision)
    {
        if (alreadyHit) return; // Ignore extra hits

        Rigidbody hitRb = collision.rigidbody;
        if (hitRb != null) // Apply force only if the object has a Rigidbody
        {
            Vector3 impactForce = dartRb.velocity * dartRb.mass; // Transfer dart's momentum
            hitRb.AddForce(impactForce, ForceMode.Impulse);
        }

        alreadyHit = true;

        collision.gameObject.GetComponentInParent<DartboardMechanics>()?.CheckDartHit(collision.gameObject.tag);
        Invoke(nameof(ResetHit), 0.5f);
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