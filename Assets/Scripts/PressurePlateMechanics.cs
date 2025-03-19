using UnityEngine;
using UnityEngine.Events;

public class PressurePlateMechanics : MonoBehaviour
{
    [Header("Weight Settings")]
    public float requiredWeight = 25f;
    private float currentWeight = 0f;

    [Header("Movement Settings")]
    public float pressDepth = 0.096f; // How far the plate moves down
    public float pressSpeed = 5f;   // How fast it moves
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float lerpProgress = 0f; // Track progress

    [Header("Events")]
    public UnityEvent onPress;
    public UnityEvent onRelease;

    private bool isPressed = false;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Allows movement without affecting physics
        initialPosition = transform.position;
        targetPosition = initialPosition;
    }

    private void Update()
    {
        lerpProgress = Mathf.MoveTowards(lerpProgress, 1f, Time.deltaTime * pressSpeed);
        rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, lerpProgress));
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name != "Pressure plate Table")
        {
            Debug.Log("Collided with: " + collision.gameObject.name);
            if (collision.rigidbody)
            {
                currentWeight += collision.rigidbody.mass;
                CheckWeight();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody.name != "Pressure plate Table")
        {
            if (collision.rigidbody)
            {
                currentWeight -= collision.rigidbody.mass;
                CheckWeight();
            }
        }
            
    }

    private void CheckWeight()
    {
        if (currentWeight >= requiredWeight && !isPressed)
        {
            isPressed = true;
            targetPosition = initialPosition + Vector3.down * pressDepth; // Move down
            lerpProgress = 0f; // Reset Lerp progress
            onPress.Invoke();
        }
        else if (currentWeight < requiredWeight && isPressed)
        {
            isPressed = false;
            targetPosition = initialPosition; // Move back up
            lerpProgress = 0f; // Reset Lerp progress
            onRelease.Invoke();
        }
    }
}
