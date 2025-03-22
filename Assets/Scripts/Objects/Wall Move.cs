using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMove : MonoBehaviour
{
    public ButtonMechanics buttonMechanics;
    public PressurePlateMechanics pressurePlateMechanics;

    public float moveDistance = 2f; // Distance to move
    public float moveSpeed = 2f; // Speed of movement

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition;

        buttonMechanics = FindAnyObjectByType<ButtonMechanics>();
        pressurePlateMechanics = FindAnyObjectByType<PressurePlateMechanics>();
    }

    public void WallMoveController()
    {
        if (buttonMechanics.isPressed && pressurePlateMechanics.isPressed)
        {
            SetTargetPosition();
        }
        else
        {
            ResetPosition();
        }
    }

    public void SetTargetPosition()
    {
        if (!isMoving)
        {
            targetPosition = initialPosition + Vector3.back * moveDistance;
            StartCoroutine(MoveWall());
        }
    }

    public void ResetPosition()
    {
        if (!isMoving)
        {
            targetPosition = initialPosition;
            StartCoroutine(MoveWall());
        }
    }

    private IEnumerator MoveWall()
    {
        isMoving = true;

        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }
}