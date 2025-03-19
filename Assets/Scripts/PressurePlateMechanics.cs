using UnityEngine;
using UnityEngine.Events;

public class PressurePlateMechanics : MonoBehaviour
{
    [Header("Weight Settings")]
    public float requiredWeight = 25f;
    private float currentWeight = 0f;

    [Header("Movement Settings")]
    public float maxPressDepth = 0.07f; // Maximum depth the plate can be pressed
    public float pressSpeed = 5f;
    private Vector3 initialPosition;
    private float currentDepth = 0f; // How far the plate is pushed
    private bool isPressing = false;

    [Header("Events")]
    public UnityEvent onPress;
    public UnityEvent onRelease;

    public bool isPressed = false;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Allows movement without affecting physics
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isPressing)
        {
            // Smoothly move the plate down
            Vector3 targetPosition = initialPosition + Vector3.down * currentDepth;
            rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, Time.deltaTime * pressSpeed));

            // Check if the plate has been pushed far enough
            if (currentDepth >= maxPressDepth && !isPressed)
            {
                isPressed = true;
                onPress.Invoke();
            }
        }
        else
        {
            // Reset position when no weight is present
            rb.MovePosition(Vector3.Lerp(rb.position, initialPosition, Time.deltaTime * pressSpeed));

            if (isPressed)
            {
                isPressed = false;
                onRelease.Invoke();
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name != "Pressure plate Table" && collision.rigidbody)
        {
            currentWeight = collision.rigidbody.mass;
            currentDepth = Mathf.Clamp(currentWeight / requiredWeight * maxPressDepth, 0, maxPressDepth);
            isPressing = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name != "Pressure plate Table")
        {
            isPressing = false;
            currentWeight = 0;
            currentDepth = 0;
        }
    }
}
