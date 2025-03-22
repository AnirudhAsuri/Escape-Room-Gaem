using UnityEngine;

public class HammerMechanics : MonoBehaviour
{
    public Vector3 rotationOffset = new Vector3(90, 90, 90); // Adjust these values as needed
    public Vector3 positionOffset = new Vector3(0, 0, -0.5f); // Adjust if needed
    private PickUpObjects pickUpScript;
    private Animator hammerAnimator;

    public GameObject hitbox;
    private Collider hitboxCollider;
    private bool isAttacking = false; // Prevents spamming attacks

    void Start()
    {
        pickUpScript = FindObjectOfType<PickUpObjects>();
        hammerAnimator = GetComponentInChildren<Animator>();

        // Get the Collider from the hitbox GameObject
        if (hitbox != null)
        {
            hitboxCollider = hitbox.GetComponent<Collider>();
            hitboxCollider.enabled = false; // Collider starts disabled
        }
        else
        {
            Debug.LogError("Hitbox is not assigned in HammerMechanics!");
        }
    }

    void Update()
    {
        if (pickUpScript.pickedObject == transform)
        {
            if (Input.GetMouseButtonDown(0)) // Left-click to attack
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        isAttacking = true; // Lock attacking
        hammerAnimator.SetTrigger("Swing"); // Play attack animation
    }

    // Called from animation event at the start of the attack
    public void EnableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = true; // Enable only the collider
    }

    // Called from animation event at the end of the attack
    public void DisableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = false; // Disable only the collider
    }

    // Called from animation event at the very end of the animation
    public void EndAttack()
    {
        isAttacking = false; // Unlock attacking
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
