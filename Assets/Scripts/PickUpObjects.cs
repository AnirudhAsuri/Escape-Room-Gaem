using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUpObjects : MonoBehaviour
{
    public Collider playerCollider;
    public PlayerMovement playerMovement;
    public Transform pickedObject;
    private Rigidbody pickedObjectRb;
    private Collider pickedObjectCollider;
    private GunMechanics gunMechanics;

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
            DartMechanics dartMechanics = pickedObject.GetComponent<DartMechanics>();

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
            else if (dartMechanics != null)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Vector3 throwDirection = Camera.main.transform.forward;
                    dartMechanics.ThrowDart(throwDirection);
                    pickedObject = null;
                }
            }
        }
    }

    public void PickOrDropObjects()
    {
        Vector3 rayOrigin = Camera.main.transform.position;
        RaycastHit hit;

        if (pickedObject == null) // Picking up objects
        {
            if (Physics.Raycast(rayOrigin, Camera.main.transform.forward, out hit, pickUpRange, grabableLayer | interactable))
            {
                if (hit.collider.CompareTag("Button"))
                {
                    ButtonMechanics buttonMechanics = hit.collider.GetComponent<ButtonMechanics>();

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        buttonMechanics.PressButton();
                    }
                }
                else if (hit.collider.CompareTag("Dial"))
                {
                    DialRotate dial = hit.collider.GetComponent<DialRotate>();

                    if (Input.GetKeyDown(KeyCode.Mouse0) && dial != null)
                    {
                        Debug.Log("If Activated");
                        dial.RotateDial();
                    }
                }

                else if (hit.rigidbody != null && hit.rigidbody.mass < 40f)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        pickedObject = hit.transform.root;
                        pickedObjectRb = pickedObject.GetComponent<Rigidbody>();
                        pickedObjectCollider = hit.collider;

                        if (pickedObjectRb != null && playerCollider != null)
                        {
                            pickedObjectRb.isKinematic = true;
                            pickedObjectRb.useGravity = false;

                            Collider[] allColliders = pickedObject.GetComponentsInChildren<Collider>();
                            foreach (Collider col in allColliders)
                            {
                                Physics.IgnoreCollision(col, playerCollider, true);
                            }
                        }

                        pickedObject.SetParent(handPosition, true);
                        pickedObject.localPosition = Vector3.zero;

                        // Check if the object has a HammerMechanics or KnifeMechanics script
                        HammerMechanics hammer = pickedObject.GetComponent<HammerMechanics>();
                        KnifeMechanics knife = pickedObject.GetComponent<KnifeMechanics>();
                        DartMechanics dart = pickedObject.GetComponent<DartMechanics>();
                        if (hammer != null)
                        {
                            pickedObject.localRotation = hammer.GetRotationOffset();
                            pickedObject.localPosition = hammer.GetPositionOffset();
                        }
                        else if (knife)
                        {
                            pickedObject.localRotation = knife.GetRotationOffset();
                            pickedObject.localPosition = knife.GetPositionOffset();
                        }
                        else if(dart)
                        { 
                            pickedObject.localRotation = dart.GetRotationOffset();
                            pickedObject.localPosition = dart.GetPositionOffset();

                            Collider dartCollider = dart.GetComponent<Collider>();
                            if (dartCollider != null)
                            {
                                dartCollider.enabled = false;
                            }
                        }
                        else
                        {
                            pickedObject.localRotation = Quaternion.identity; // Default rotation
                        }
                    }
                }
            }
        }
        else // Dropping objects
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Vector3 dropPosition = GetCrosshairDropPosition();

                if (pickedObjectRb != null && playerCollider)
                {
                    pickedObjectRb.isKinematic = false;
                    pickedObjectRb.useGravity = true;

                    Collider[] allColliders = pickedObject.GetComponentsInChildren<Collider>();
                    foreach (Collider col in allColliders)
                    {
                        Physics.IgnoreCollision(col, playerCollider, false);
                    }
                }

                pickedObject.SetParent(null);
                pickedObject.position = dropPosition;
                pickedObject.rotation = Quaternion.identity; // Reset rotation on drop

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