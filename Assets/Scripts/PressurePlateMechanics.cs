using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class PressurePlateMechanics : MonoBehaviour
{
    [Header("Weight Settings")]
    public float requiredWeight = 25f;  // The weight required to press the plate
    private float currentWeight = 0f;   // The total weight on the plate

    [Header("Movement Settings")]
    public float maxPressDepth = 0.07f;  // Maximum depth the plate can be pressed
    public float pressSpeed = 5f;        // Speed at which the plate moves down
    private Vector3 initialPosition;     // Initial position of the plate
    private float currentDepth = 0f;     // How far the plate is pressed down
    private bool isPressing = false;     // Flag to indicate whether the plate is being pressed

    [Header("Events")]
    public UnityEvent onPress;   // Event triggered when plate is pressed
    public UnityEvent onRelease; // Event triggered when plate is released

    public bool isPressed = false; // Whether the plate is fully pressed

    private Rigidbody rb;                             // Rigidbody for the plate
    private HashSet<Rigidbody> objectsOnPlate = new HashSet<Rigidbody>(); // Track objects on plate

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;  // Allows movement without affecting physics
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Accumulate the total weight of all objects on the plate
        currentWeight = 0f;
        foreach (var obj in objectsOnPlate)
        {
            if (obj != null)
                currentWeight += obj.mass; // Add the mass of each object to the total weight
        }

        // Check if weight exceeds the required weight
        bool isWeightEnough = currentWeight >= requiredWeight;

        // Calculate depth based on total weight if it's enough
        if (isWeightEnough)
        {
            currentDepth = Mathf.Clamp(currentWeight / requiredWeight * maxPressDepth, 0, maxPressDepth);
            isPressing = true;
        }
        else
        {
            currentDepth = 0f;
            isPressing = false;
        }

        // Move the plate down smoothly if pressing
        if (isPressing)
        {
            Vector3 targetPosition = initialPosition + Vector3.down * currentDepth;
            rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, Time.deltaTime * pressSpeed));

            // Trigger event if fully pressed
            if (currentDepth >= maxPressDepth && !isPressed)
            {
                isPressed = true;
                onPress.Invoke();
            }
        }
        else
        {
            // Reset position when no weight is present or weight is not enough
            rb.MovePosition(Vector3.Lerp(rb.position, initialPosition, Time.deltaTime * pressSpeed));

            if (isPressed)
            {
                isPressed = false;
                onRelease.Invoke();
            }
        }
    }

    private Dictionary<Rigidbody, int> colliderCounts = new Dictionary<Rigidbody, int>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Pressure plate Table" && collision.rigidbody)
        {
            Rigidbody rb = collision.rigidbody;

            if (!colliderCounts.ContainsKey(rb))
            {
                colliderCounts[rb] = 0;
                objectsOnPlate.Add(rb);
            }

            colliderCounts[rb]++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name != "Pressure plate Table" && collision.rigidbody)
        {
            Rigidbody rb = collision.rigidbody;

            if (colliderCounts.ContainsKey(rb))
            {
                colliderCounts[rb]--;

                if (colliderCounts[rb] <= 0)
                {
                    colliderCounts.Remove(rb);
                    objectsOnPlate.Remove(rb);
                }
            }
        }
    }

}