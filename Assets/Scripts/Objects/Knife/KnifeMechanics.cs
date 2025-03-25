using UnityEngine;

public class KnifeMechanics : MonoBehaviour
{
    public Vector3 rotationOffset = new Vector3(90, 90, 90);
    public Vector3 positionOffset = new Vector3(0, 0, -0.5f);

    private PickUpObjects pickUpScript;
    private Animator knifeAnimator;

    public GameObject hitbox;
    private Collider hitboxCollider;

    void Start()
    {
        pickUpScript = FindObjectOfType<PickUpObjects>();
        knifeAnimator = GetComponentInChildren<Animator>();

        if (hitbox != null)
        {
            hitboxCollider = hitbox.GetComponent<Collider>();
            hitboxCollider.enabled = false; // Start disabled
        }
        else
        {
            Debug.LogError("Hitbox is not assigned in KnifeMechanics!");
        }
    }

    void Update()
    {
        if (pickUpScript.pickedObject == transform)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        knifeAnimator.SetTrigger("Slice");
    }

    // Animation event: Called when the hitbox should be active
    public void EnableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = true;
    }

    // Animation event: Called when the attack is finished
    public void DisableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
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