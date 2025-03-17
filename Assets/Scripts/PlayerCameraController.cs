using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public Transform playerBodyTransform;

    public float cameraSensitivity;
    
    private float xInput;
    private float yInput;

    private float sinVarA;
    private float sinVarB;

    private float xRotation;
    private float yRotation;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }
    private void Update()
    {
        HandleCamera();
    }
    public void HandleCamera()
    {
        xInput = Input.GetAxisRaw("Mouse X") * cameraSensitivity * Time.deltaTime;
        yInput = Input.GetAxisRaw("Mouse Y") * cameraSensitivity * Time.deltaTime;

        xRotation = xInput;
        yRotation -= yInput;

        playerBodyTransform.Rotate(Vector3.up * xRotation);

        yRotation = Mathf.Clamp(yRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);

        if(playerMovement.isGrounded)
        {
            if(playerMovement.isSprinting && playerMovement.movementInput != Vector3.zero)
            {
                sinVarA = 16f;
                sinVarB = 0.07f;
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 65f, Time.deltaTime * 5f); // Increase FOV when sprinting
            }
            else if(!playerMovement.isSprinting && playerMovement.movementInput != Vector3.zero)
            {
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 75f, Time.deltaTime * 5f); // Return to normal FOV
                sinVarA = 7f;
                sinVarB = 0.06f;
            }
            else if(playerMovement.movementInput == Vector3.zero)
            {
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 75f, Time.deltaTime * 5f); // Return to normal FOV
                sinVarA = 2f;
                sinVarB = 0.025f;
            }
        }
        float sprintCameraOffset = Mathf.Sin(Time.time * sinVarA) * sinVarB; // Sin wave for smoother shake
        transform.localPosition = new Vector3(transform.localPosition.x, 0.9f + sprintCameraOffset, transform.localPosition.z);
    }
}
