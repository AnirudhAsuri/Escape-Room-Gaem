using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CapsuleCollider playerCapsuleCollider;
    private Rigidbody playerRigidBody;
    public Transform firstPersonCamTransform;

    public Vector3 movementInput;
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public bool isGrounded;
    public bool isSprinting;
    public float jumpForce;
    void Start()
    {
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
        playerRigidBody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleJumping();
    }

    public void HandleMovement()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.z = Input.GetAxisRaw("Vertical");
        movementInput.y = 0;
        movementInput.Normalize();

        Vector3 moveDirection = Vector3.zero;
        moveDirection = transform.right * movementInput.x + transform.forward * movementInput.z;

        if(OnSlope())
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, GetSlopeNormal()).normalized;
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = sprintSpeed;
            isSprinting = true;
        }
        else
        {
            moveSpeed = walkSpeed;
            isSprinting = false;
        }
        playerRigidBody.velocity = new Vector3(moveDirection.x * moveSpeed, playerRigidBody.velocity.y, moveDirection.z * moveSpeed);
    }

    public void HandleJumping()
    {
        float groundCheckDistance = (playerCapsuleCollider.height / 2) + 0.2f; // Slightly increased distance
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // Move ray origin slightly up

        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundCheckDistance);

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    public bool OnSlope()
    {
        float groundCheckDistance = (playerCapsuleCollider.height / 2) + 0.2f; // Same increase
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // Same ray origin fix

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundCheckDistance))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            return angle > 5f && angle < 45f; // Avoid near-flat slopes
        }
        return false;
    }


    private Vector3 GetSlopeNormal()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f))
        {
            return hit.normal;
        }
        return Vector3.up;
    }

}
