using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObjects : MonoBehaviour
{
    public Collider playerCollider;
    public PlayerMovement playerMovement;
    public Transform pickedObject;
    private Rigidbody pickedObjectRb;
    private Collider pickedObjectCollider;
    private GunMechanics gunMechanics; // Reference to GunMechanics

    public Transform handPosition;
    private Vector3 originalHandPosition;

    public LayerMask grabableLayer;
    public LayerMask interactable;

    public float pickUpRange = 3f;
    public float maxHoldDistance = 2.5f; // Max distance object can be held
    public float minHoldDistance = 0.25f; // Minimum distance before pulling it back

    private void Awake()
    {
        playerCollider = GetComponent<Collider>();
        playerMovement = GetComponent<PlayerMovement>();
        originalHandPosition = new Vector3(0.48f, -0.02f, 1.08f);
    }

    private void Update()
    {
        PickOrDropObjects();

        if (pickedObject != null)
        {
            GunMechanics gunMechanics = pickedObject.GetComponent<GunMechanics>();
            if (gunMechanics != null)
            {
                if (Input.GetMouseButtonDown(0)) // Left-click to shoot
                {
                    gunMechanics.Shoot();
                }
                else if (Input.GetKeyDown(KeyCode.R)) // 'R' to reload
                {
                    gunMechanics.Reload();
                }
            }
        }

    }

    public void PickOrDropObjects()
    {
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward * pickUpRange;

        RaycastHit hit;

        if (pickedObject == null) // Picking up objects
        {
            if (Physics.Raycast(rayOrigin, Camera.main.transform.forward, out hit, pickUpRange, grabableLayer | interactable))
            {
                if(hit.collider.CompareTag("Button"))
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        ButtonMechanics buttonMechanics = hit.collider.GetComponent<ButtonMechanics>();
                        if(buttonMechanics != null)
                        {
                            buttonMechanics.PressButton();
                        }
                    }
                }
                if (hit.rigidbody != null && hit.rigidbody.mass < 40f) // Check mass before picking up
                {
                    Debug.DrawRay(rayOrigin, rayDirection, Color.green);

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        pickedObject = hit.transform.root;
                        pickedObjectRb = pickedObject.GetComponent<Rigidbody>();
                        pickedObjectCollider = hit.collider;

                        if (pickedObjectRb != null && playerCollider != null)
                        {
                            pickedObjectRb.isKinematic = true;
                            pickedObjectRb.useGravity = false;
                            Physics.IgnoreCollision(pickedObjectCollider, playerCollider, true);
                        }

                        pickedObject.SetParent(handPosition, true);
                        pickedObject.localPosition = Vector3.zero;
                        pickedObject.localRotation = Quaternion.identity;

                        // Check if picked object is a gun
                        gunMechanics = pickedObject.GetComponent<GunMechanics>();
                    }
                }
                else
                {
                    Debug.DrawRay(rayOrigin, rayDirection, Color.red);
                }
            }
        }
        else // Dropping objects (should be outside the picking block)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Vector3 dropPosition = GetCrosshairDropPosition();

                if (pickedObjectRb != null && playerCollider)
                {
                    pickedObjectRb.isKinematic = false;
                    pickedObjectRb.useGravity = true;
                    Physics.IgnoreCollision(pickedObjectCollider, playerCollider, false);
                }

                pickedObject.SetParent(null);
                pickedObject.position = dropPosition;

                // Reset gun mechanics reference when dropping
                gunMechanics = null;

                pickedObject = null;
                pickedObjectRb = null;
            }
        }

        HandleHandBob(); // Handles sprint bobbing
    }

    private void HandleHandBob()
    {
        // Smoothly move back to the original position before applying bobbing
        handPosition.localPosition = Vector3.Lerp(handPosition.localPosition, originalHandPosition, 0.1f);

        if (playerMovement.movementInput != Vector3.zero) // If moving
        {
            float bobOffset;

            if (playerMovement.isSprinting)
            {
                bobOffset = Mathf.Sin(Time.time * 12f) * 0.015f;
                handPosition.localPosition += new Vector3(0, bobOffset, 0);
            }
            else
            {
                bobOffset = Mathf.Sin(Time.time * 7f) * 0.002f;
                handPosition.localPosition += new Vector3(0, bobOffset, 0);
            }
        }
    }

    private Vector3 GetCrosshairDropPosition()
    {
        float dropRange = 1.7f;
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, dropRange))
        {
            return hit.point;
        }

        return rayOrigin + rayDirection * dropRange;
    }
}